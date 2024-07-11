using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TooManyCows.DataObjects;

public class GridTrackCalculator
{
	public static Dictionary<string, Vector2> directions = new Dictionary<string, Vector2>()
    {
        { "N", Vector2.up }, { "S", Vector2.down }, { "W", Vector2.left }, { "E", Vector2.right },
	};

	public static Dictionary<string, string> transposedDirection = new Dictionary<string, string>()
    {
        { "N", "W" }, { "S", "E" }, { "W", "N" }, { "E", "S" },
	};

	public Dictionary<Vector2, TileTrackData> trackDict = new Dictionary<Vector2, TileTrackData>();
		
	public void AddTractorPath(TractorData tractor, bool isTransposed)
	{
		if(tractor.route.Length == 0)
			return;

		var currentPosition = tractor.startPosition;

		for(int i=0; i<tractor.routeString.Length; i++)
		{
			var inDir = GetRouteAtIdx(tractor.routeString, i-1);
			var outDir = GetRouteAtIdx(tractor.routeString, i);

			if (isTransposed)
			{
				inDir = transposedDirection[inDir];
				outDir = transposedDirection[outDir];
			}

			if(!trackDict.ContainsKey(currentPosition))
				trackDict[currentPosition] = new TileTrackData(currentPosition);

			trackDict[currentPosition].AddDirection(inDir + outDir);

			currentPosition += directions[outDir];
		}
	}

	public void AddSheepPath(SheepData sheep, bool isTransposed)
	{
		if(sheep.route.Length == 0)
			return;

		var currentPosition = sheep.startPosition;

		for(int i=0; i<sheep.routeString.Length; i++)
		{
			var inDir = GetRouteAtIdx(sheep.routeString, i-1);
			var outDir = GetRouteAtIdx(sheep.routeString, i);

			if (isTransposed)
			{
				inDir = transposedDirection[inDir];
				outDir = transposedDirection[outDir];
			}

			if(!trackDict.ContainsKey(currentPosition))
				trackDict[currentPosition] = new TileTrackData(currentPosition, isSheep: true);

			trackDict[currentPosition].AddDirection(inDir + outDir);

			currentPosition += directions[outDir];
		}
	}

    public TileTrackData[] TrackDataList
    {
        get
        {	
			List<TileTrackData> items = new List<TileTrackData>();
    		items.AddRange(trackDict.Values);
			return items.ToArray();
        }
    }

    string GetRouteAtIdx(string routeString, int idx)
	{
		while(idx < 0)
			idx += routeString.Length;

		return routeString[idx % routeString.Length].ToString();
	}
}
