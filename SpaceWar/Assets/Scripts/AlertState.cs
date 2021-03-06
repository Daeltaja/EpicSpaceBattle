﻿using UnityEngine;
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
			myGameObject.GetComponent<SteeringBehaviours>().maxSpeed = 12f;

		}
		
		public override void Exit()
		{
			
		}
		
		public override void Update()
		{
				if(enemyGameObject.name.StartsWith("EnemyTeaser"))
				{
					if((myGameObject.transform.position - enemyGameObject.transform.position).magnitude < 40f)
					{
						GameManager.warpedDiversion = false;
						myGameObject.GetComponent<StateMachine>().SwitchState(new ChaseState(myGameObject, enemyGameObject));
					}
					if(Vector3.Distance(myGameObject.transform.position, enemyGameObject.transform.position) < 1f)
					{
						enemyGameObject = GameObject.Find ("AllyPatrolShip1");
						myGameObject.GetComponent<StateMachine>().SwitchState(new EvadeState(myGameObject, enemyGameObject));
					}
				}


			if(myGameObject.name.StartsWith("EnemyForce"))
			{
				myGameObject.GetComponent<StateMachine>().SwitchState(new FireState(myGameObject, enemyGameObject));

			}
			if(myGameObject.name.StartsWith("AllyForce"))
			{
				myGameObject.GetComponent<StateMachine>().SwitchState(new FireState(myGameObject, enemyGameObject));
			}
		}
	}
}
