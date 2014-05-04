using UnityEngine;
using System.Collections;

public class CameraRotation : MonoBehaviour {

	bool rotate, move;
	public bool rotation, movement;
	public float timer;
	public float speed;

	void Start()
	{
		rotate = false;
		if(rotation)
		{
			Invoke ("DelayRotate", timer);
		}
		if(movement)
		{
			Invoke ("DelayMove", timer);
		}
	}

	// Update is called once per frame
	void Update () 
	{
		if(rotate)
		{
			transform.Rotate(-transform.up * speed * Time.deltaTime, Space.World);
		}
		if(move)
		{
			transform.Translate(transform.right * speed * Time.deltaTime, Space.World);
		}
	}

	void DelayRotate()
	{
		rotate = true;
	}

	void DelayMove()
	{
		move = true;
	}

}
