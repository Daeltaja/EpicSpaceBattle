using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;

namespace BGE.States
{
	public class CameraManager : MonoBehaviour 
	{
		public List<GameObject> cameras = new List<GameObject>();
		public List<float> camChange = new List<float>();
		float timer;
		int count = 1;
		int camChangeIndex = 0;

		void Awake () 
		{
			for(int i = 0; i < 3; i++)
			{
				cameras[i] = GameObject.Find ("Camera"+count);
				count++;
			}
		}

		void Update () 
		{
			timer += Time.deltaTime;
			if(timer > camChange[camChangeIndex])
			{
				cameras[camChangeIndex].GetComponent<Camera>().enabled = false;
				cameras[camChangeIndex+1].GetComponent<Camera>().enabled = true;
				camChangeIndex++;
				timer = 0;
			}
		}
	}
}
