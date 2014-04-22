using UnityEngine;
using System.Collections;
namespace BGE.States
{
	public class TeaseState:State
	{
	    GameObject teasee;

	    public override string Description()
	    {
	        return "Tease State";
	    }

	    public TeaseState(GameObject entity, GameObject teasee):base(entity)
	    {
	        this.teasee = teasee;
	    }

	    public override void Enter()
	    {
	        myGameObject.GetComponent<SteeringBehaviours>().DisableAll();
	        myGameObject.GetComponent<SteeringBehaviours>().pursuitEnabled = true;
	        myGameObject.GetComponent<SteeringBehaviours>().pursuitTarget = teasee;
	    }

	    public override void Update()
	    {
	        float distance = 5.0f;
	        if (Vector3.Distance(myGameObject.transform.position, teasee.transform.position) < distance)
	        {
	            myGameObject.GetComponent<StateMachine>().SwitchState(new EvadeState(myGameObject, teasee));
	        }
	    }

	    public override void Exit()
	    {
	        myGameObject.GetComponent<SteeringBehaviours>().DisableAll();            
	    }
	}
}

