using UnityEngine;
using System.Collections;
namespace BGE.States
{
	public class ChaseState : State
	{
		GameObject enemyGameObject;
		float shootTime = 0.25f;
		
		public override string Description()
		{
			return "Chase State";
		}
		
		public ChaseState(GameObject myGameObject, GameObject enemyGameObject) : base(myGameObject)
		{
			this.enemyGameObject = enemyGameObject;
		}
		
		public override void Enter()
		{
			myGameObject.GetComponent<SteeringBehaviours>().DisableAll();
			myGameObject.GetComponent<SteeringBehaviours>().offsetPursuitEnabled = true;
			myGameObject.GetComponent<SteeringBehaviours>().offsetPursuitTarget = enemyGameObject;
			myGameObject.GetComponent<SteeringBehaviours>().maxSpeed += 2f;
		}
		
		public override void Exit()
		{
			
		}
		
		public override void Update()
		{
			if(enemyGameObject == null)
			{
				enemyGameObject = GameObject.Find ("EnemyForce");
				myGameObject.GetComponent<StateMachine>().SwitchState(new AlertState(myGameObject, enemyGameObject));
			}
			shootTime += Time.deltaTime;
			float fov = Mathf.PI / 4.0f;
			float angle;
			Vector3 toEnemy = (enemyGameObject.transform.position - myGameObject.transform.position);
			toEnemy.Normalize();
			angle = (float) Mathf.Acos(Vector3.Dot(toEnemy, myGameObject.transform.forward));

			if (angle < fov)
			{
				if (shootTime > 0.25f)
				{
					if(myGameObject.name.Contains("Ally"))
					{
						GameObject laserGO = GameObject.Find ("LaserAlly");
						GameObject laser = MonoBehaviour.Instantiate(laserGO, myGameObject.transform.position, Quaternion.identity)as GameObject;
						laser.transform.position = myGameObject.transform.position;
						laser.transform.forward = myGameObject.transform.forward;
						shootTime = 0.0f;
					}
					else if(myGameObject.name.Contains("Enemy"))
					{
						GameObject laserGO = GameObject.Find ("LaserEnemy");
						GameObject laser = MonoBehaviour.Instantiate(laserGO, myGameObject.transform.position, Quaternion.identity)as GameObject;
						laser.transform.position = myGameObject.transform.position;
						laser.transform.forward = myGameObject.transform.forward;
						shootTime = 0.0f;
					}
				}
			}

		}
	}
}
