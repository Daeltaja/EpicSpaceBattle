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
			this.name = "LaserAlly";
			gm = GameObject.Find("GameManager").GetComponent<GameManager>();
			gm.entities.Add(this.gameObject);
		}

		void Update()
		{
			transform.position += transform.forward * speed * Time.deltaTime;
		}
	}
}
