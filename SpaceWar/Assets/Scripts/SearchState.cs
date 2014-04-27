using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;
namespace BGE.States
{
	public class SearchState : State
	{
		GameObject target;
		float timer = 0f;
		
		public override string Description()
		{
			return "Search State";
		}
		
		public SearchState(GameObject myGameObject, GameObject target) : base(myGameObject)
		{
			this.target = target;
		}
		
		public override void Enter()
		{
			myGameObject.GetComponent<SteeringBehaviours>().DisableAll();
			myGameObject.GetComponent<SteeringBehaviours>().arriveEnabled = true;
			myGameObject.GetComponent<SteeringBehaviours>().obstacleAvoidEnabled = true;
			myGameObject.GetComponent<SteeringBehaviours>().arrivePos = target.transform.position;
			myGameObject.GetComponent<SteeringBehaviours>().maxSpeed = 12f;
		}
		
		public override void Exit()
		{
			
		}
		
		public override void Update()
		{
			if(Vector3.Distance(myGameObject.transform.position, target.transform.position) <= 0.3f)
			{
				myGameObject.GetComponent<SteeringBehaviours>().velocity *= 0f;

				timer += Time.deltaTime;
				Debug.Log (timer);
				if(timer > 5f)
				{
					DelayChange();
				}

			}
		}
		void DelayChange()
		{
			target = GameObject.Find ("Tower1");
			myGameObject.GetComponent<StateMachine>().SwitchState(new FireState(myGameObject, target));
		}
	}
}
