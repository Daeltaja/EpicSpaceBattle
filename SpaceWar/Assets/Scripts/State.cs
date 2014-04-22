using System;
using UnityEngine;

namespace BGE.States
{
	public abstract class State 
	{
		protected GameObject myGameObject;
		
		public State(GameObject myGameObject)
		{
			this.myGameObject = myGameObject;
		}
		
		public abstract string Description();
		
		public abstract void Enter();
		public abstract void Exit();
		
		public abstract void Update();
	}
}