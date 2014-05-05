using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
//using UnityEngine;

namespace BGE.States
{
	public class CameraManager : MonoBehaviour 
	{
		public List<GameObject> cameras = new List<GameObject>();
		public List<float> camChange = new List<float>();
		public float timer;
		int camChangeIndex = 0;
		bool playAudio = true;

		void Update () 
		{
			timer += Time.deltaTime;
			if(timer > camChange[camChangeIndex])
			{
				cameras[camChangeIndex].transform.GetChild(0).gameObject.GetComponent<Camera>().enabled = false;
				cameras[camChangeIndex].transform.GetChild(0).gameObject.GetComponent<AudioListener>().enabled = false;
				cameras[camChangeIndex+1].transform.GetChild(0).gameObject.GetComponent<Camera>().enabled = true;
				cameras[camChangeIndex+1].transform.GetChild(0).gameObject.GetComponent<AudioListener>().enabled = true;
				camChangeIndex++;
				if(cameras[camChangeIndex].transform.GetChild(0).gameObject.GetComponent<AudioSource>() != null)
				{
					cameras[camChangeIndex].transform.GetChild(0).gameObject.audio.Play ();
				}
				timer = 0;
			}
		}
	}
}
