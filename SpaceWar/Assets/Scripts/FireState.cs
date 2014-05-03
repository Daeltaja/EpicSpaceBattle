using UnityEngine;
using System.Collections;
namespace BGE.States
{
	public class FireState : State 
	{
		GameObject target, laserGO;
		float shootTime;
		float delayStateChange;
		float randomShoot;
		int randomTarget;

		public override string Description()
		{
			return "Firing State";
		}
		
		public FireState(GameObject myGameObject, GameObject target) : base(myGameObject)
		{
			this.target = target;
		}
		
		public override void Enter()
		{
			myGameObject.GetComponent<SteeringBehaviours>().DisableAll();
			laserGO = myGameObject.GetComponent<SteeringBehaviours>().laser;
			randomShoot = Random.Range (.4f, .7f);
			if(myGameObject.name.Contains("AllyPatrol"))
			{
				myGameObject.GetComponent<SteeringBehaviours>().lookAtShootEnabled = true;
				myGameObject.GetComponent<SteeringBehaviours>().seekPos = target.transform.position;
				myGameObject.GetComponent<SteeringBehaviours>().maxSpeed = 0f;
			}

			if(GameManager.warpedAllies)
			{
				if(myGameObject.name.Contains("EnemyForce"))
				{
					myGameObject.GetComponent<SteeringBehaviours>().lookAtShootEnabled = true;
					randomTarget = Random.Range(0, GameManager.allyForces.Count);
					target = GameManager.allyForces[randomTarget];
					myGameObject.GetComponent<SteeringBehaviours>().seekPos = target.transform.position;
					myGameObject.GetComponent<SteeringBehaviours>().maxSpeed = 0f;
				}
				if(myGameObject.name.Contains("AllyForce"))
				{
					myGameObject.GetComponent<SteeringBehaviours>().lookAtShootEnabled = true;
					randomTarget = Random.Range(0, GameManager.enemyForces.Count);
					target = GameManager.enemyForces[randomTarget];
					myGameObject.GetComponent<SteeringBehaviours>().seekPos = target.transform.position;
					myGameObject.GetComponent<SteeringBehaviours>().maxSpeed = 0f;
				}
			}
		}

		public override void Exit()
		{
			
		}
	
		public override void Update()
		{
			if(target != null)
			{
				Shoot();
			}
			else
			{
				if(GameManager.warpedAllies)
				{
					if(myGameObject.name.Contains("AllyForce"))
					{
						if(GameManager.enemyForces.Count > 0)
						{
							randomTarget = Random.Range(0, GameManager.enemyForces.Count);
							target = GameManager.enemyForces[randomTarget];
							myGameObject.GetComponent<SteeringBehaviours>().seekPos = target.transform.position;
						}
						else
						{
							myGameObject.GetComponent<SteeringBehaviours>().seekPos = myGameObject.transform.position;
						}
					}
					if(myGameObject.name == "EnemyForce")
					{
						if(GameManager.allyForces.Count > 0)
						{
							randomTarget = Random.Range(0, GameManager.allyForces.Count);
							target = GameManager.allyForces[randomTarget];
							myGameObject.GetComponent<SteeringBehaviours>().seekPos = target.transform.position;
						}
						else
						{
							myGameObject.GetComponent<SteeringBehaviours>().seekPos = myGameObject.transform.position;
						}
					}
				}
			}
			if(myGameObject.name.Contains("AllyPatrolShip"))
			{
				JammerSpotter();
			}
			if(myGameObject.name.Contains("EnemyForce"))
			{
				if(GameManager.warpedAllies && target != GameObject.Find("AllyForce"))
				{
					myGameObject.GetComponent<StateMachine>().SwitchState(new FireState(myGameObject, myGameObject));
				}
			}
		}

		void Shoot()
		{
			float range = 100.0f;           	

			if ((myGameObject.transform.position - target.transform.position).magnitude < range)
			{
				shootTime += Time.deltaTime;
				float fov = Mathf.PI / 4.0f;
				float angle;
				Vector3 toEnemy = (target.transform.position - myGameObject.transform.position);
				toEnemy.Normalize();
				angle = (float) Mathf.Acos(Vector3.Dot(toEnemy, myGameObject.transform.forward));
				
				if (angle < fov)
				{
					if (shootTime > randomShoot)
					{
						GameObject laser = MonoBehaviour.Instantiate(laserGO, myGameObject.transform.position+myGameObject.transform.forward*1f, Quaternion.identity)as GameObject;
						laser.transform.position = myGameObject.transform.position;
						laser.transform.forward = myGameObject.transform.forward;
						laser.name = myGameObject.GetComponent<SteeringBehaviours>().laser.name;
						shootTime = 0.0f;
					}
				}
			}
		}


		void JammerSpotter()
		{
			if(myGameObject.name.Contains("Ally"))
			{
				if(target == null)
				{
					if(GameObject.Find ("Tower2") != null)
					{
						target = GameObject.Find ("Tower2");
						myGameObject.GetComponent<SteeringBehaviours>().seekPos = target.transform.position;
					}
					else
					{
						target = myGameObject;
					}
				}
			}
		}
	}
}
