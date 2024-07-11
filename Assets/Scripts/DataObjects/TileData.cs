using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TooManyCows.DataObjects
{
	public class TileData
	{
		public Vector2 position;
		public TileType tileType;

		public TileData(string tileString, int xPos, int yPos)
		{
			position = new Vector2(xPos, -yPos);
			
			if(tileString == "grass")
				tileType = TileType.Grass;
			else if (tileString == "start")
				tileType = TileType.BarnStart;
			else if (tileString == "end")
				tileType = TileType.BarnEnd;
			else if (tileString == "tree")
				tileType = TileType.Tree;
			else
				Debug.LogWarning("Couldn't find tile type! (" + tileString + ")");
		}

		public TileData(int xPos, int yPos)
		{
			tileType = TileType.Blank;
			position = new Vector2(xPos, -yPos);
		}
	}
}