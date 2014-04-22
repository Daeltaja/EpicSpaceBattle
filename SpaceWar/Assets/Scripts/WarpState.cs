using UnityEngine;
using System.Collections;
namespace BGE.States
{
	public class WarpState : State
	{
		GameObject enemyGameObject;
		float delayTimer;

		public override string Description()
		{
			return "Warped in!";
		}
		
		public WarpState(GameObject myGameObject, GameObject enemyGameObject) : base(myGameObject)
		{
			this.enemyGameObject = enemyGameObject;
		}
		
		public override void Enter()
		{
			myGameObject.GetComponent<SteeringBehaviours>().DisableAll();
			GameManager.warpedDiversion = true; //only for warped diversion, not warped forces, FIX!
		}
		
		public override void Exit()
		{
			
		}
		
		public override void Update()
		{
			if(enemyGameObject.name.StartsWith("AllyPatrolShip"))
			{
				myGameObject.GetComponent<StateMachine>().SwitchState(new DiversionState(myGameObject, enemyGameObject));
			}
			if(enemyGameObject.name.StartsWith("Mothership"))
			{
				delayTimer += Time.deltaTime;
				if(delayTimer >= GameManager.assaultDelay) //this is the time the enemy forces wait before attacking Mothership
				{
					delayTimer = 0;
					myGameObject.GetComponent<StateMachine>().SwitchState(new FireState(myGameObject, enemyGameObject));
				}
			}
		}
	}
}
