using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TooManyCows.DataObjects;

// This script holds the LevelData object, and all the levels
public class LevelLoader : MonoBehaviour
{
	public TextAsset[] levelList;
	public LevelData levelData;

	// Use this for initialization
	void Start ()
	{
		var rawLevelText = levelList[SaveData.GetCurrentLevel()].text;
		levelData = LevelData.CreateFromJSON(rawLevelText);
	}
}
