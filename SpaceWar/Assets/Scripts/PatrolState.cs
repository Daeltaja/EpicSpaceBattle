using UnityEngine;
using System.Collections;
namespace BGE.States
{
	public class PatrolState : State
	{
		public override string Description()
		{
			return "Patrolling State";
		}
		
		public PatrolState(GameObject myGameObject) : base(myGameObject)
		{

		}
		
		public override void Enter()
		{
			myGameObject.GetComponent<SteeringBehaviours>().DisableAll();
			myGameObject.GetComponent<SteeringBehaviours>().followPathEnabled = true;
			myGameObject.GetComponent<SteeringBehaviours>().maxSpeed = 2f;
		}
		
		public override void Exit()
		{
			
		}
		
		public override void Update()
		{
			if(GameManager.warpedDiversion) //one ship should go alert, other stay patrolling, do this here!
			{
				//("Huh");
				
			}

		}	
	}
}
