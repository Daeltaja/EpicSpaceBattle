using UnityEngine;
using System.Collections;
namespace BGE.States
{
	public class FireState : State 
	{
		GameObject target;
		float shootTime;
		float delayStateChange;

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
		}
		
		public override void Exit()
		{
			
		}
		
		public override void Update()
		{
			float range = 50.0f;           
			
			if ((target.transform.position - myGameObject.transform.position).magnitude < range)
			{
				shootTime += Time.deltaTime;
				float fov = Mathf.PI / 4.0f;
				float angle;
				Vector3 toEnemy = (target.transform.position - myGameObject.transform.position);
				toEnemy.Normalize();
				angle = (float) Mathf.Acos(Vector3.Dot(toEnemy, myGameObject.transform.forward));
				
				if (angle < fov)
				{
					if (shootTime > 0.25f)
					{
						GameObject laserGO = GameObject.Find ("Laser");
						GameObject laser = MonoBehaviour.Instantiate(laserGO, myGameObject.transform.position, Quaternion.identity)as GameObject;
						laser.transform.position = myGameObject.transform.position;
						laser.transform.forward = myGameObject.transform.forward;
						shootTime = 0.0f;
					}
				}
			}

			if(GameManager.warpedAllies)
			{
				if(myGameObject.name == "EnemyForce")
				{
					myGameObject.GetComponent<StateMachine>().SwitchState(new AlertState(myGameObject, target)); 
				}
			}
		}
	}
}
