using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

namespace BGE.States
{
	public class GameManager : MonoBehaviour {

		public List<GameObject> allyPatrolShip = new List<GameObject>();
		public static List<GameObject> enemyForces = new List<GameObject>();
		public static List<GameObject> allyForces = new List<GameObject>();
		public List<GameObject> entities = new List<GameObject>();
		public GameObject enemyForceGO, allyForceGO, asteroidGO;
		public Vector3 warpedPos;
		public int enemyForceCount = 10;
		public int allyForceCount = 10;
		public int asteroidCount = 10;
		public float warpDiversionDelay;
		public float warpForcesDelay;
		public float warpAlliesDelay;
		public float checkOutDiversionDelay;
		int asteroidCounter = 6;

		public static float assaultDelay = 15f; //for the enemy forces assault after they warp in
		public static bool warpedDiversion = true; //change to false, turn true after short delay, this starts off proceedings!
		public static bool warpedForces;
		public static bool warpedAllies;
		public static bool jammerSearch; 

		GameObject motherShip, teaser, jammer, asteroidSpawner, teaserWarp, cameraManager;
		Vector3 warpPos;
		CameraManager camManager;
		
		void Start () 
		{
			cameraManager = GameObject.Find ("GameManager");
			camManager = cameraManager.GetComponent<CameraManager>();
			teaser = GameObject.Find("EnemyTeaser");
			motherShip = GameObject.Find ("Mothership");
			jammer = GameObject.Find ("Jammer");
			asteroidSpawner = GameObject.Find ("AsteroidSpawner");
			teaserWarp = GameObject.Find ("TeaserWarp");
			int cnt = 1;
			for(int i = 0; i < 2; i++)
			{
				allyPatrolShip.Add(GameObject.Find ("AllyPatrolShip"+cnt));
				allyPatrolShip[i].GetComponent<StateMachine>().SwitchState(new PatrolState(allyPatrolShip[i]));
				cnt++;
			}
			Invoke ("WarpInDiversion", warpDiversionDelay);
			Invoke ("WarpForces", warpForcesDelay);
			Invoke ("WarpAllies", warpAlliesDelay);
			InvokeRepeating("WarpAsteroids", 0f, 4f);
		}

		void Update()
		{
			if(asteroidCounter == 0)
			{
				CancelInvoke("WarpAsteroids");
			}
		}

		void WarpInDiversion()
		{
			warpedPos = new Vector3(teaserWarp.transform.position.x, teaserWarp.transform.position.y, teaserWarp.transform.position.z);
			teaser.transform.position = warpedPos;
			Invoke ("DelayCheck", checkOutDiversionDelay);
		}

		void DelayCheck()
		{
			warpedDiversion = true;
			allyPatrolShip[0].GetComponent<StateMachine>().SwitchState(new AlertState(allyPatrolShip[0], teaser)); //enemy = ally in this case
			teaser.GetComponent<StateMachine>().SwitchState(new WarpState(teaser, allyPatrolShip[0]));
		}

		void WarpForces() //forces warp into existance!
		{
			float xPos = -20, yPos = 3, zPos = -40; //change the spawn in position of the enemy forces here!
			for(int i = 0; i < enemyForceCount; i++)
			{
				Vector3 randPos = new Vector3(xPos, yPos, zPos);
				GameObject enemyForce = Instantiate(enemyForceGO, randPos, Quaternion.identity) as GameObject;
				xPos += UnityEngine.Random.Range (5, 10);
				yPos += UnityEngine.Random.Range (-.5f, .5f);
				zPos += UnityEngine.Random.Range (-2, 2);
				
				enemyForce.name = "EnemyForce";
				enemyForce.GetComponent<StateMachine>().SwitchState(new WarpState(enemyForce, motherShip));
				enemyForces.Add(enemyForce);
			}
			StartCoroutine("RadioDelay");
			warpedForces = true;
		}

		void WarpAllies() //forces warp into existance!
		{
			float xPos = -20, yPos = 0, zPos = -80; //change the spawn in position of the enemy forces here!
			for(int i = 0; i < allyForceCount; i++)
			{
				Vector3 randPos = new Vector3(xPos, yPos, zPos);
				GameObject allyForce = Instantiate(allyForceGO, randPos, Quaternion.identity) as GameObject;
				xPos += UnityEngine.Random.Range (5, 10);
				yPos += UnityEngine.Random.Range (-3, 3);
				zPos += UnityEngine.Random.Range (-1, 1);
				
				allyForce.name = "AllyForce";
				allyForce.GetComponent<StateMachine>().SwitchState(new AlertState(allyForce, allyForce)); //CIRCLE FIRE STATE
				allyForces.Add(allyForce);
			}
			warpedAllies = true;
		}

		IEnumerator RadioDelay()
		{
			yield return new WaitForSeconds(assaultDelay-11f);
			allyPatrolShip[1].GetComponent<StateMachine>().SwitchState(new RadioState(allyPatrolShip[1], jammer));
			jammerSearch = true;
		}

		public void WarpAsteroids()
		{
			float xPos = 0, yPos = 0, zPos = 0;
			for(int i = 0; i < asteroidCount; i++)
			{
				Vector3 spawner = new Vector3(asteroidSpawner.transform.position.x + xPos, asteroidSpawner.transform.position.y + yPos, asteroidSpawner.transform.position.z + zPos);
				GameObject asteroid = Instantiate(asteroidGO, spawner, asteroidSpawner.transform.rotation) as GameObject;
				Vector3 myPos = new Vector3(asteroid.transform.position.x, asteroid.transform.position.y, asteroid.transform.position.z);
				xPos += UnityEngine.Random.Range (6f, 10f);
				yPos += UnityEngine.Random.Range (-8, 8);
				zPos += (int)UnityEngine.Random.Range (-10, 10);
				float randomSize = UnityEngine.Random.Range(1.5f, 7.5f);
				asteroid.transform.localScale = new Vector3 (randomSize, randomSize, randomSize);
				asteroid.name = "Asteroid";
				asteroid.GetComponent<StateMachine>().SwitchState(new AsteroidState(asteroid, myPos));
				entities.Add(asteroid);
			}
			asteroidCounter --;
		}
	}
}
