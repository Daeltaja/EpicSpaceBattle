using UnityEngine;
using System.Collections.Generic;
using BGE.Geom;

namespace BGE
{
    public class SteeringBehaviours : MonoBehaviour
    {
        //behaviour flags
        public bool seekEnabled;
        public bool fleeEnabled;
        public bool arriveEnabled;
        public bool pursuitEnabled;
        public bool evadeEnabled;
        public bool offsetPursuitEnabled;
        public bool followPathEnabled;
        public bool obstacleAvoidEnabled;

        //integrator 
        public float mass;
        public float maxSpeed;
        public Vector3 velocity;
        public Vector3 force;

        //behaviour targets
        public Vector3 seekPos;
        public Vector3 fleePos;
        public Vector3 arrivePos;
        public Vector3 offsetPursuitOffset;
        public GameObject seekTarget;
        public GameObject pursuitTarget;
        public GameObject evadeTarget;
        public GameObject offsetPursuitTarget;

		public float defaultRadius = 5.0f;
		public Path path = new Path();

        public SteeringBehaviours() //constructor
        {
            force = Vector3.zero;
            velocity = Vector3.zero;
            mass = 1.0f;
            maxSpeed = 5.0f;
        }

        public void DisableAll() //disabler
        {
            seekEnabled = false;
            fleeEnabled = false;
            pursuitEnabled = false;
            evadeEnabled = false;
            offsetPursuitEnabled = false;
            arriveEnabled = false;
            followPathEnabled = false;
            obstacleAvoidEnabled = false;
        }

        #region Steering Behaviours
        Vector3 Seek(Vector3 targetPos)
        {
            Vector3 desiredPos = targetPos - transform.position;
            desiredPos.Normalize();
            desiredPos *= maxSpeed;
            return (desiredPos - velocity);
        }

        Vector3 Flee(Vector3 targetPos)
        {
            Vector3 desiredPos = transform.position - targetPos;
            desiredPos.Normalize();
            desiredPos *= maxSpeed;
            return (desiredPos - velocity);
        }

        Vector3 Arrive(Vector3 targetPos)
        {
            Vector3 toTarget = targetPos - transform.position;
            float distance = toTarget.magnitude;
            float slowingDistance = 2.0f;
            if (distance == 0)
            {
                return Vector3.zero;
            }
            const float decelTweak = 0.0f;
            float rampedSpeed = maxSpeed * (distance / (slowingDistance * decelTweak));
            float clampedSpeed = Mathf.Min(rampedSpeed, maxSpeed);
            Vector3 desiredPos = clampedSpeed * (toTarget / distance);
            return (desiredPos - velocity);
        }

        Vector3 Pursue()
        {
            Vector3 toTarget = pursuitTarget.transform.position - transform.position;
            float distance = toTarget.magnitude;
            float time = distance / maxSpeed;
            Vector3 desiredPos = pursuitTarget.transform.position + (time * pursuitTarget.GetComponent<SteeringBehaviours>().velocity);
            return Seek(desiredPos);
        }

        Vector3 Evade()
        {
            float lookAhead = maxSpeed;
            Vector3 targetPos = evadeTarget.transform.position + (lookAhead * evadeTarget.GetComponent<SteeringBehaviours>().velocity);
            return Flee(targetPos);
        }

        Vector3 OffsetPursuit(Vector3 offset)
        {
            Vector3 target = Vector3.zero;
            target = offsetPursuitTarget.transform.TransformPoint(offset);

            float distance = (target - transform.position).magnitude;
            float lookAhead = distance / maxSpeed;

            target = target + (lookAhead * offsetPursuitTarget.GetComponent<SteeringBehaviours>().velocity);
            return Arrive(target);
        }

		Vector3 FollowPath()
		{
			path.CreatePath(4, 12);
			float epsilon = 5.0f;
			float dist = (transform.position - path.NextWaypoint()).magnitude;
			if (dist < epsilon)
			{
				path.AdvanceToNext();
			}
			return Seek(path.NextWaypoint());
			/*Vector3 desired = path.waypoints[path.currWaypoint] - transform.position; //vector for distance between current waypoint and spheres position
			float changeDistance = 1.0f; 
			if(desired.magnitude < changeDistance) //when sphere gets within 1 unit of waypoint, iterate to next waypoint
			{
				path.currWaypoint++; 
			}
			if(path.currWaypoint == path.waypoints.Count) //if sphere reaches the final waypoint, reset it to zero, so the FollowPath() keeps looping
			{
				path.currWaypoint = 0;
				return Arrive (path.waypoints[path.currWaypoint]); //call Arrive for the final waypoint
			}
			else
			{
				return Seek (path.waypoints[path.currWaypoint]); //seek for all waypoint except final waypoint
			}*/return Vector3.zero;
		}

        Vector3 ObstacleAvoidance()
        {
            Vector3 force = Vector3.zero;
    
            List<GameObject> tagged = new List<GameObject>();
            float minBoxLength = 50.0f;
            float boxLength = minBoxLength + ((velocity.magnitude / maxSpeed) * minBoxLength * 2.0f);

            GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

            foreach (GameObject obstacle in obstacles)
            {
                Vector3 toCentre = transform.position - obstacle.transform.position;
                float dist = toCentre.magnitude;
                if (dist < boxLength)
                {
                    tagged.Add(obstacle);
                }
            }
            float distToClosestIP = float.MaxValue;
            GameObject closestIntersectingObstacle = null;
            Vector3 localPosOfClosestObstacle = Vector3.zero;
            Vector3 intersection = Vector3.zero;

            foreach (GameObject o in tagged)
            {
                Vector3 localPos = transform.InverseTransformPoint(o.transform.position);

                if (localPos.z >= 0)
                {
                    float obstacleRadius = o.transform.localScale.x / 2;
                    float expandedRadius = GetRadius() + obstacleRadius;
                    if ((Mathf.Abs(localPos.y) < expandedRadius) && (Mathf.Abs(localPos.x) < expandedRadius))
                    {
                        Sphere tempSphere = new Sphere(expandedRadius, localPos);
                        BGE.Geom.Ray ray = new BGE.Geom.Ray();
                        ray.pos = new Vector3(0, 0, 0);
                        ray.look = Vector3.forward;

                        if (tempSphere.closestRayIntersects(ray, Vector3.zero, ref intersection) == false)
                        {
                            continue;
                        }
                        float dist = intersection.magnitude;
                        if (dist < distToClosestIP)
                        {
                            dist = distToClosestIP;
                            closestIntersectingObstacle = o;
                            localPosOfClosestObstacle = localPos;
                        }
                    }
                }
            }

            if (closestIntersectingObstacle != null)
            {
                float multiplier = 1.0f + (boxLength - localPosOfClosestObstacle.z) / boxLength;

                float obstacleRadius = closestIntersectingObstacle.transform.localScale.x / 2; // closestIntersectingObstacle.GetComponent<Renderer>().bounds.extents.magnitude;
                float expandedRadius = GetRadius() + obstacleRadius;
                force.x = (expandedRadius - Mathf.Abs(localPosOfClosestObstacle.x)) * multiplier;
                force.y = (expandedRadius - Mathf.Abs(localPosOfClosestObstacle.y)) * multiplier;

                if (localPosOfClosestObstacle.x > 0)
                {
                    force.x = -force.x;
                }

                if (localPosOfClosestObstacle.y > 0)
                {
                    force.y = -force.y;
                }

                Debug.DrawLine(transform.position, transform.position + transform.forward * boxLength, Color.grey);
                
                const float brakingWeight = 0.01f;
                force.z = (expandedRadius -
                                   localPosOfClosestObstacle.z) *
                                   brakingWeight;
                 
                force = transform.TransformDirection(force);
            }
            return force;
        }
        #endregion

        #region Update
        void Update()
        {
            if (seekEnabled)
            {
                force += Seek(seekPos);
            }
            if (fleeEnabled)
            {
                force += Flee(fleePos);
            }
            if (arriveEnabled)
            {
                force += Arrive(arrivePos);
            }
            if (pursuitEnabled)
            {
                force += Pursue();
            }
            if (evadeEnabled)
            {
                force += Evade();
            }
            if (offsetPursuitEnabled)
            {
                force += OffsetPursuit(offsetPursuitOffset);
            }
            if (followPathEnabled)
            {
                force += FollowPath();
            }
			if (obstacleAvoidEnabled)
			{
				force += ObstacleAvoidance();
			}
            //Force Integrator
            Vector3 accel = force / mass; //new vector for acceleration, which is the forceAcc divided by the mass of object
            velocity = velocity + accel * Time.deltaTime;
            transform.position = transform.position + velocity * Time.deltaTime;
            force = Vector3.zero; //reset forceAcc to zero so each cycle
            if (velocity.magnitude > float.Epsilon) //if there is any velocity past smallest possible number, normalize its forward direction 
            {
                transform.forward = Vector3.Normalize(velocity);
            }
            velocity *= 0.99f;
			path.DrawPath();
        }
        #endregion

        private float GetRadius()
        {
            Renderer r = GetComponent<Renderer>();
            if (r == null)
            {
                return defaultRadius;
            }
            else
            {
                return r.bounds.extents.magnitude;
            }
        }
    }
}
