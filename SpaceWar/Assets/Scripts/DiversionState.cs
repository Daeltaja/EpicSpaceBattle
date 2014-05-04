using UnityEngine;
using System.Collections;
namespace BGE.States
{
	public class DiversionState : State
	{
		GameObject allyPatrolShip;
		float rangeModifier;
		
		public override string Description()
		{
			return "Diversion State";
		}
		
		public DiversionState(GameObject myGameObject, GameObject allyPatrolShip) : base(myGameObject)
		{
			this.allyPatrolShip = allyPatrolShip;
		}
		
		public override void Enter()
		{
			myGameObject.GetComponent<SteeringBehaviours>().DisableAll();
			myGameObject.GetComponent<SteeringBehaviours>().evadeTarget = allyPatrolShip;
		}

		public override void Exit()
		{

		}
		
		public override void Update()
		{
			float range = 38.0f;           

			if ((allyPatrolShip.transform.position - myGameObject.transform.position).magnitude < range)
			{
				//GameObject targetPos = GameObject.Find ("TargetPos");
				myGameObject.GetComponent<StateMachine>().SwitchState(new EvadeState(myGameObject, allyPatrolShip)); //enemy = ally in this case
			}
		}
	}
}
