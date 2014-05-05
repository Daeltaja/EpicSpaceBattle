using System;
using System.Collections.Generic;
using UnityEngine;
namespace BGE.States
{
	class EvadeState:State
	{
	    GameObject chaser;

	    public override string Description()
	    {
	        return "Evade State";
	    }

		public EvadeState(GameObject entity, GameObject chaser):base(entity)
	    {
			this.chaser = chaser;
	    }

	    public override void Enter()
	    {
	        myGameObject.GetComponent<SteeringBehaviours>().DisableAll();
			myGameObject.GetComponent<SteeringBehaviours>().evadeEnabled = true;
			myGameObject.GetComponent<SteeringBehaviours>().evadeTarget = chaser;
			myGameObject.GetComponent<SteeringBehaviours>().obstacleAvoidEnabled = true;
			myGameObject.GetComponent<SteeringBehaviours>().maxSpeed = 12f;
		}
		
		public override void Update()
	    {
			if (Vector3.Distance(myGameObject.transform.position, chaser.transform.position) > 25f)
	        {
				myGameObject.GetComponent<StateMachine>().SwitchState(new DiversionState(myGameObject, chaser));
			}
		}

	    public override void Exit()
	    {
	        myGameObject.GetComponent<SteeringBehaviours>().DisableAll();            
	    }
	}
}
