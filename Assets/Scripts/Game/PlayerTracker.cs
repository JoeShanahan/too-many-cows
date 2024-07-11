using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTracker : MonoBehaviour
{
	static PlayerTracker _instance;

	public int levelIdx;
	public float timeSpent = 0;
	public int movesMade = 0;
	public int rewindsMade = 0;
	public bool levelSolved = false;
	public int restartCount = 0;

	Dictionary<int, int[]> _trackingDict;

	// Use this for initialization
	void Start ()
	{
		_instance = this;
		_LoadTrackingData();
		_FindThisLevel();
	}
	
	void _LoadTrackingData()
	{
		levelIdx = SaveData.GetCurrentLevel();
		var trackingData = TrackingData.CreateFromJSON(SaveData.TrackingDataString);
		_trackingDict = trackingData.GetLevelStats();
	}

	void _FindThisLevel()
	{
		if(!_trackingDict.ContainsKey(levelIdx))
			return;

		var intList = _trackingDict[levelIdx];

		movesMade = intList[1];
		rewindsMade = intList[2];
		timeSpent = intList[3];
		levelSolved = intList[4] == 1;
		restartCount = intList[5];
	}

	// Update is called once per frame
	void Update ()
	{
		if(levelSolved)
			return;
			
		timeSpent += Time.deltaTime;
	}

	public static void SaveCurrentLevel()
	{
		if(_instance == null)
			return;

		_instance._SaveCurrentLevel();
	}

	public static void MoveOne()
	{
		if(_instance == null || _instance.levelSolved)
			return;

		_instance.movesMade += 1;
	}

	public static void CompleteLevel()
	{
		if(_instance == null)
			return;
		
		_instance.levelSolved = true;
	}

	public static void RewindOne()
	{
		if(_instance == null || _instance.levelSolved)
			return;

		_instance.rewindsMade += 1;
	}

	public static void LevelRestartedOrQuit()
	{
		if(_instance == null || _instance.levelSolved)
			return;

		_instance.restartCount++;
	}

	void _SaveCurrentLevel()
	{
		var intList = new int[6];

		/* levelIdx,moveCount,rewindCount,totalSeconds,solved,restarts */
		intList[0] = levelIdx;
		intList[1] = movesMade;
		intList[2] = rewindsMade;
		intList[3] = (int)timeSpent;
		intList[5] = restartCount;
		
		if(levelSolved)
			intList[4] = 1;
		else
			intList[4] = 0;

		_trackingDict[levelIdx] = intList;
		var trackingData = new TrackingData();
		trackingData.DictionaryToStrings(_trackingDict);

		SaveData.TrackingDataString = trackingData.ObjToJson();
	}


}
