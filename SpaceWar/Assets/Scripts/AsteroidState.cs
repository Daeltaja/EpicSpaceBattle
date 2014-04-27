using UnityEngine;
using System.Collections;
namespace BGE.States
{
	public class AsteroidState : State
	{
		Vector3 initialPos;
		public override string Description()
		{
			return "Asteroid!";
		}
		
		public AsteroidState(GameObject myGameObject, Vector3 myPos) : base(myGameObject)
		{
			initialPos = myPos;
		}
		
		public override void Enter()
		{
			myGameObject.GetComponent<SteeringBehaviours>().DisableAll();
			myGameObject.GetComponent<SteeringBehaviours>().seekEnabled = true;
			myGameObject.GetComponent<SteeringBehaviours>().maxSpeed = UnityEngine.Random.Range (0.8f, 1.2f);
			myGameObject.GetComponent<SteeringBehaviours>().seekPos = myGameObject.transform.position + new Vector3(myGameObject.transform.position.x, myGameObject.transform.position.y, myGameObject.transform.position.z + 250f);
		}
		
		public override void Exit()
		{
			
		}
		
		public override void Update()
		{
			if(myGameObject.transform.position.z > initialPos.z + 230f)
			{
				myGameObject.transform.position = initialPos;
			}
		}
	}
}
