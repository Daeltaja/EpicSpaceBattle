using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BGE
{
	public class Path 
	{
		public List<Vector3> waypoints = new List<Vector3>(); //list to hold 
		public int currWaypoint = 0; //current waypoint that will be iterated through
		bool looped = true;

		public void CreatePath(int points, float radius)
		{
			float thetaInc = Mathf.PI / points * 2; 
			float lastX = 0; //holds last xPos
			float lastZ = radius; //z pos is always equal to radius (10f)
			
			for (int i = 0 ; i <= points ; i++) //loop for the points creation
			{
				float currentX; 
				float currentZ;
				float theta = (float) i * thetaInc; 
				
				currentX = Mathf.Sin(theta) * (radius); //calculating X position on edge of circle
				currentZ = Mathf.Cos(theta) * (radius); //calculating X position on edge of circle
				
				lastX = currentX; //update last to current
				lastZ = currentZ; //update last to current
				
				Vector3 newPos = new Vector3(lastX+0f, 3, lastZ-8f); //new position storing lastX and lastZ
				waypoints.Add(newPos); //stores the new Pos in waypoint list for line drawing
			}
		}

		public void DrawPath()
		{
			for(int i = 0; i < waypoints.Count-1; i++)
			{
				Debug.DrawLine (waypoints[i], waypoints[i+1], Color.magenta );
			}
		}

		public Vector3 NextWaypoint()
		{
			return waypoints[currWaypoint];
		}

		public void AdvanceToNext()
		{
			if (looped)
			{
				currWaypoint = (currWaypoint + 1) % waypoints.Count();
			}
			else
			{
				if (currWaypoint != waypoints.Count() - 1)
				{
					currWaypoint = currWaypoint + 1;
				}
			}
		}
	}
}
