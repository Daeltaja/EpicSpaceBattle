using UnityEngine;
using System.Collections;

public class WarpScale : MonoBehaviour {

	public float delay;
	public float scale;

	void Start () 
	{
		Invoke ("ScaleMe", delay);
	}

	void ScaleMe()
	{
		audio.Play();
		iTween.ScaleTo(gameObject, iTween.Hash("x", scale, "y", scale, "z", scale, "time", .5f, "easetype", iTween.EaseType.easeInOutSine));
	}
}
