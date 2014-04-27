using UnityEngine;
using System.Collections;
namespace BGE.States
{
	public class RadioState : State
	{
		GameObject jammer;
		float timer = 2f;
		
		public override string Description()
		{
			return "Radio State";
		}
		
		public RadioState(GameObject myGameObject, GameObject jammer) : base(myGameObject)
		{
			this.jammer = jammer;
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
			timer += Time.deltaTime;
			if(timer >= GameManager.assaultDelay)
			{
				timer = 0;
				jammer = GameObject.Find ("Jammer");
				myGameObject.GetComponent<StateMachine>().SwitchState(new SearchState(myGameObject, jammer)); //enemy = ally in this case
			}
		}
	}
}
