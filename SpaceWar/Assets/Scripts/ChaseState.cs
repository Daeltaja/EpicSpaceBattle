using UnityEngine;
using System.Collections;
namespace BGE.States
{
	public class ChaseState : State
	{
		GameObject enemyGameObject, laserGO;
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
			myGameObject.GetComponent<SteeringBehaviours>().obstacleAvoidEnabled = true;
			laserGO = myGameObject.GetComponent<SteeringBehaviours>().laser;
			myGameObject.GetComponent<SteeringBehaviours>().maxSpeed = 12f;
		}
		
		public override void Exit()
		{
			
		}
		
		public override void Update()
		{
			if(enemyGameObject == null)
			{
				myGameObject.GetComponent<StateMachine>().SwitchState(new PatrolState(myGameObject));
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
					GameObject laser = MonoBehaviour.Instantiate(laserGO, myGameObject.transform.position, Quaternion.identity)as GameObject;
					laser.name = myGameObject.GetComponent<SteeringBehaviours>().laser.name;
					laser.transform.position = myGameObject.transform.position;
					laser.transform.forward = myGameObject.transform.forward;
					
					shootTime = 0.0f;
				}
			}
		}
	}
}
