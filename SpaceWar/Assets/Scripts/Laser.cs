using UnityEngine;
using System.Collections.Generic;
using BGE.Geom;

namespace BGE.States
{
	class Laser : MonoBehaviour
	{
		GameManager gm;
		float speed = 45.0f;

		void Start()
		{
			gm = GameObject.Find("GameManager").GetComponent<GameManager>();
			gm.entities.Add(this.gameObject);
			Invoke("Destroy", 2f);
		}

		void Update()
		{
			transform.position += transform.forward * speed * Time.deltaTime;
		}

		void Destroy()
		{
			gm.entities.Remove(this.gameObject);
			Destroy(gameObject);
		}
	}
}
