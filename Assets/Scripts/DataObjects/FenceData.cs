using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TooManyCows.DataObjects
{
	public class FenceData
	{
		public Vector2 position;
		public int rotation;

		public FenceData(int x1, int y1, int x2, int y2)
		{	
			position = new Vector2(x1 + x2, -(y1 + y2));
			position *= 0.5f;

			if(y1 != y2)
			{
				rotation = 90;
			}
		}
	}
}