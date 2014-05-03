using UnityEngine;
using System.Collections;

public class CameraRotation : MonoBehaviour {

	bool rotate;
	public float timer;
	public float speed;

	void Start()
	{
		rotate = false;
		Invoke ("Delay", timer);
	}

	// Update is called once per frame
	void Update () 
	{
		if(rotate)
		{
			transform.Rotate(-transform.up * speed * Time.deltaTime, Space.Self);
		}
	}

	void Delay()
	{
		rotate = true;
	}
}
