using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TrackSpawnInfo
{
	public string tilename;
	public int rotation;
	public int flipped;

	public TrackSpawnInfo(string t, int r, int f)
	{
		tilename = "Tile" + t;
		rotation = r;
		flipped = f;
	}
}

public class TileTrackData
{
	public Vector2 position;
	public bool sheepTracks;
	
	public List<string> directionList = new List<string>();
	public static Dictionary<string, string> opposite = new Dictionary<string, string>()
    {
        { "NN", "SS" }, { "SS", "NN" }, { "EE", "WW" }, { "WW", "EE" },		// Straight
		{ "NS", "SN" }, { "SN", "NS" }, { "EW", "WE" }, { "WE", "EW" },		// U-turns
		{ "NE", "WS" }, { "ES", "NW" }, { "SW", "EN" }, { "WN", "SE" },		// Clockwise turns
		{ "WS", "NE" }, { "NW", "ES" }, { "EN", "SW" }, { "SE", "WN" },		// Anticlockwise turns
    };

	public static Dictionary<string, TrackSpawnInfo> trackPrefabInfo = new Dictionary<string, TrackSpawnInfo>()
	{
		{ "NN", new TrackSpawnInfo("Tracks", 0, 1) },
		{ "EE", new TrackSpawnInfo("Tracks", 1, 1) },
		{ "SS", new TrackSpawnInfo("Tracks", 2, 1) },
		{ "WW", new TrackSpawnInfo("Tracks", 3, 1) }, 
		
		{ "NE", new TrackSpawnInfo("TracksR", 0, -1)},
		{ "ES", new TrackSpawnInfo("TracksR", 3, -1)}, 
		{ "SW", new TrackSpawnInfo("TracksR", 2, -1)}, 
		{ "WN", new TrackSpawnInfo("TracksR", 1, -1)},
		
		{ "WS", new TrackSpawnInfo("TracksR", 1, 1)}, 
		{ "NW", new TrackSpawnInfo("TracksR", 0, 1)}, 
		{ "EN", new TrackSpawnInfo("TracksR", 3, 1)}, 
		{ "SE", new TrackSpawnInfo("TracksR", 2, 1)},  
		
		{ "SN", new TrackSpawnInfo("TracksU", 0, 1)}, 
		{ "NS", new TrackSpawnInfo("TracksU", 2, 1)}, 
		{ "EW", new TrackSpawnInfo("TracksU", 1, 1)}, 
		{ "WE", new TrackSpawnInfo("TracksU", 3, 1)}, 
	};

	public static Dictionary<string, TrackSpawnInfo> feetPrefabInfo = new Dictionary<string, TrackSpawnInfo>()
	{
		{ "NN", new TrackSpawnInfo("Feet", 0, 1) },
		{ "EE", new TrackSpawnInfo("Feet", 3, 1) },
		{ "SS", new TrackSpawnInfo("Feet", 2, 1) },
		{ "WW", new TrackSpawnInfo("Feet", 1, 1) }, 
		
		{ "NE", new TrackSpawnInfo("FeetR", 0, -1)},
		{ "ES", new TrackSpawnInfo("FeetR", 3, -1)}, 
		{ "SW", new TrackSpawnInfo("FeetR", 2, -1)}, 
		{ "WN", new TrackSpawnInfo("FeetR", 1, -1)},
		
		{ "WS", new TrackSpawnInfo("FeetR", 1, 1)}, 
		{ "NW", new TrackSpawnInfo("FeetR", 0, 1)}, 
		{ "EN", new TrackSpawnInfo("FeetR", 3, 1)}, 
		{ "SE", new TrackSpawnInfo("FeetR", 2, 1)},  
	};


	public TileTrackData(Vector2 pos, bool isSheep=false)
	{
		position = pos;
		sheepTracks = isSheep;
	}

	public void AddDirection(string direction)
	{
		// If it's not a valid direction, ditch it
		if(!opposite.ContainsKey(direction))
			return;

		// If it's already in the list, or its opposite is, ditch it
		if(directionList.Contains(direction))
			return;
		if(directionList.Contains(opposite[direction]))
			return;

		// U-turns are exclusive, so nothing can be added after a U-turn
		if(directionList.Contains("NS") || directionList.Contains("SN"))
			return;

		if(directionList.Contains("EW") || directionList.Contains("WE"))
			return;
		
		// U-turns are exclusive, so can't be added if the list isn't empty
		// Also Sheep can't do U-turns
		if(sheepTracks || directionList.Count > 0)
		{
			if(direction == "NS" || direction == "SN" || direction == "EW" || direction == "WE")
				return;
		}

		directionList.Add(direction);
	}
}
