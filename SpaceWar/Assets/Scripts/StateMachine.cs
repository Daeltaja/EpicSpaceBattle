using UnityEngine;
using System.Collections;
using System.Text;

namespace BGE.States
{
	public class StateMachine : MonoBehaviour
	{
		State currentState;
		
		public void Update()
		{
			if (currentState != null)
			{
				currentState.Update();
			}
		}
		
		public void SwitchState(State newState)
		{
			if (currentState != null)
			{
				currentState.Exit();
			}
			
			currentState = newState;
			if (newState != null)
			{
				currentState.Enter();
			}
		}
	}
}

