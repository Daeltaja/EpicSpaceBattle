using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;

namespace BGE.States
{

	public class GameManager : MonoBehaviour {

		public List<GameObject> allyPatrolShip = new List<GameObject>();
		public static List<GameObject> enemyForces = new List<GameObject>();
		public static List<GameObject> allyForces = new List<GameObject>();
		public List<GameObject> entities = new List<GameObject>();
		public GameObject enemyForceGO, allyForceGO;
		public Vector3 warpedPos;
		public int enemyForceCount = 10;
		public int allyForceCount = 10;

		public static float assaultDelay = 5f; //for the enemy forces assault after they warp in
		public static bool warpedDiversion = true; //change to false, turn true after short delay, this starts off proceedings!
		public static bool warpedForces;
		public static bool warpedAllies;
		public static bool jammerSearch; 

		GameObject motherShip;
		GameObject teaser;
		GameObject jammer;
		Vector3 warpPos;
		int whichAllyPatrol; //the ally patrol ship that goes to check out the diversion
		
		void Start () 
		{
			teaser = GameObject.Find("EnemyTeaser");
			motherShip = GameObject.Find ("Mothership");
			jammer = GameObject.Find ("Jammer");

			int cnt = 1;
			for(int i = 0; i < 2; i++)
			{
				allyPatrolShip.Add(GameObject.Find ("AllyPatrolShip"+cnt));
				allyPatrolShip[i].GetComponent<StateMachine>().SwitchState(new PatrolState(allyPatrolShip[i], teaser));
				cnt++;
			}
			WarpInDiversion(); //call on timer, initial diversion
			WarpForces(); //call on timer
			//WarpAllies();
		}

		void WarpInDiversion()
		{
			warpedPos = new Vector3(-80f, 0, -18f);
			teaser.transform.position = warpedPos;

			int roll = (int)UnityEngine.Random.Range (0, 2);
			Debug.Log(roll);
			if(roll == 0)
			{
				allyPatrolShip[0].GetComponent<StateMachine>().SwitchState(new AlertState(allyPatrolShip[0], teaser)); //enemy = ally in this case
				whichAllyPatrol = 1;
			}
			else
			{
				allyPatrolShip[1].GetComponent<StateMachine>().SwitchState(new AlertState(allyPatrolShip[1], teaser)); //enemy = ally in this case
				whichAllyPatrol = 0;
			}
			teaser.GetComponent<StateMachine>().SwitchState(new WarpState(teaser, allyPatrolShip[roll]));
			warpedDiversion = true;
			StartCoroutine("RadioDelay");
		}

		void WarpForces() //forces warp into existance!
		{
			float xPos = -20, yPos = 0, zPos = -40; //change the spawn in position of the enemy forces here!
			for(int i = 0; i < enemyForceCount; i++)
			{
				Vector3 randPos = new Vector3(xPos, yPos, zPos);
				GameObject enemyForce = Instantiate(enemyForceGO, randPos, Quaternion.identity) as GameObject;
				xPos += UnityEngine.Random.Range (5, 10);
				yPos += UnityEngine.Random.Range (-3, 3);
				zPos += UnityEngine.Random.Range (-3, 3);
				
				enemyForce.name = "EnemyForce";
				enemyForce.GetComponent<StateMachine>().SwitchState(new WarpState(enemyForce, motherShip));
				enemyForces.Add(enemyForce);
			}
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
				zPos += UnityEngine.Random.Range (-3, 3);
				
				allyForce.name = "AllyForce";
				allyForce.GetComponent<StateMachine>().SwitchState(new AlertState(allyForce, allyForce)); //CIRCLE FIRE STATE
				allyForces.Add(allyForce);
			}
			warpedAllies = true;
		}

		IEnumerator RadioDelay()
		{
			yield return new WaitForSeconds(assaultDelay);
			allyPatrolShip[whichAllyPatrol].GetComponent<StateMachine>().SwitchState(new RadioState(allyPatrolShip[whichAllyPatrol], jammer));
			jammerSearch = true;
		}
	}
}
