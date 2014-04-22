using UnityEngine;
using System.Collections;
namespace BGE.States
{
	public class AlertState : State
	{
		GameObject enemyGameObject;
		
		public override string Description()
		{
			return "Alerted State";
		}
		
		public AlertState(GameObject myGameObject, GameObject enemyGameObject) : base(myGameObject)
		{
			this.enemyGameObject = enemyGameObject;
		}
		
		public override void Enter()
		{
			myGameObject.GetComponent<SteeringBehaviours>().DisableAll();
			myGameObject.GetComponent<SteeringBehaviours>().seekEnabled = true;
			myGameObject.GetComponent<SteeringBehaviours>().obstacleAvoidEnabled = true;
			myGameObject.GetComponent<SteeringBehaviours>().seekPos = enemyGameObject.transform.position;
		}
		
		public override void Exit()
		{
			
		}
		
		public override void Update()
		{
			if(enemyGameObject.name.StartsWith("EnemyTeaser"))
			{
				if((myGameObject.transform.position - enemyGameObject.transform.position).magnitude < 25f)
				{
					GameManager.warpedDiversion = false;
					myGameObject.GetComponent<StateMachine>().SwitchState(new ChaseState(myGameObject, enemyGameObject));
				}
			}

			//this is for ally patrol ship spotting the enemy jammers
			if(GameManager.jammerSearch)
			{
				if(enemyGameObject.name.StartsWith("Jammer"))
				{
					if(Vector3.Distance(myGameObject.transform.position, enemyGameObject.transform.position) < 30f)
					{
						myGameObject.GetComponent<StateMachine>().SwitchState(new FireState(myGameObject, enemyGameObject));
					}
				}
			}

			if(myGameObject.name.StartsWith("EnemyForce"))
			{
				myGameObject.GetComponent<SteeringBehaviours>().seekPos = new Vector3(myGameObject.transform.position.x, myGameObject.transform.position.y, -200f);

			}
			if(myGameObject.name.StartsWith("AllyForce"))
			{
				myGameObject.GetComponent<SteeringBehaviours>().seekPos = new Vector3(myGameObject.transform.position.x, myGameObject.transform.position.y, 200f);
			}
		}
	}
}
