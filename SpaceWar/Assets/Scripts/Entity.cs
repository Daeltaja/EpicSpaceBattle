using UnityEngine;
using System.Collections.Generic;
using BGE.Geom;

namespace BGE.States
{
	public class Entity : MonoBehaviour 
	{
		GameManager gm;
		public GameObject explosionSmall, explosionBig;
		public int health = 5;
		float asteroidRotateSpeed;

		void Start()
		{
			gm = GameObject.Find ("GameManager").GetComponent<GameManager>();
			if(this.gameObject.name == "Asteroid")
			{
				asteroidRotateSpeed = Random.Range (14f, 30f);
			}
		}

		void TakeDamage(int dmg)
		{
			health -= dmg;
			if(health <= 0)
			{
				Instantiate(explosionBig, transform.position, transform.rotation);
				Destroy(gameObject);
			}
		}

		void Update()
		{
			if(this.gameObject.name == "Asteroid")
			{
				transform.Rotate(transform.right * asteroidRotateSpeed * Time.deltaTime);
			}



			for(int i = 0; i < gm.entities.Count; i++) //loop through the entity array list
			{
				if(gm.entities[i].name == "LaserAlly" != null)
				{
					if(gm.entities[i].name == "LaserAlly")
					{
						{
							if(this.name.Contains("Enemy"))
							{
								GameObject laser = gm.entities[i];
								if((transform.position - laser.transform.position).magnitude < 2f)
								{
									Vector3 diePos = new Vector3(laser.transform.position.x, laser.transform.position.y, laser.transform.position.z)+laser.transform.forward / transform.localScale.z / 3;
                                                                                                                                           
									GameObject explodeSmall = Instantiate(explosionSmall, diePos, transform.rotation)as GameObject;
									laser.transform.position = transform.position;
									laser.transform.forward = transform.forward;
									Destroy(laser.gameObject);
									gm.entities.Remove(laser);
									TakeDamage(1);
								}
							}
						}
					}
				}
				if(gm.entities[i].name == "LaserEnemy" != null)
				{
					if(gm.entities[i].name == "LaserEnemy")
					{
						{
							if(this.name.Contains("Ally"))
							{
								GameObject laser = gm.entities[i];
								if((transform.position - laser.transform.position).magnitude < 2f)
								{
									Vector3 diePos = new Vector3(laser.transform.position.x, laser.transform.position.y, laser.transform.position.z)+laser.transform.forward / transform.localScale.z / 4;
									GameObject explodeSmall = Instantiate(explosionSmall, diePos, transform.rotation)as GameObject;
									laser.transform.position = transform.position;
									laser.transform.forward = transform.forward;
									gm.entities.Remove(laser);
									TakeDamage(1);
								}
							}
						}
					}
				}
			}
		}
	}
}
