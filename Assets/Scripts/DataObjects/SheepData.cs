using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TooManyCows.DataObjects
{
	public class SheepData
	{
		public int numSheep;
		public Vector2 startPosition;
		public Vector2[] route;
		public string routeString;

		public SheepData(int length, int xpos, int ypos, string directions, bool isTransposed)
		{
			numSheep = length;
			startPosition = new Vector2(xpos, -ypos);
			var routeList = new List<Vector2>();
			routeString = directions;

			foreach(var ch in directions)
			{
				switch(ch)
				{
					case 'N':
						routeList.Add(isTransposed ? Vector2.left : Vector2.up);
						break;
					case 'E':
						routeList.Add(isTransposed ? Vector2.down : Vector2.right);				
						break;
					case 'S':
						routeList.Add(isTransposed ? Vector2.right : Vector2.down);
						break;
					case 'W':
						routeList.Add(isTransposed ? Vector2.up : Vector2.left);
						break;
					default:
						Debug.LogWarning("Unknown direction for sheep! (" + ch + ")");
						break;
				}
			}

			route = routeList.ToArray();		
		}
	}
}