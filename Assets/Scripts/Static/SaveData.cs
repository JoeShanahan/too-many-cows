using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
	public static void SetCurrentLevel(int levelIdx)
	{
		PlayerPrefs.SetInt("currentLevel", levelIdx);
		PlayerPrefs.Save();
	}

	public static int GetCurrentLevel()
	{
		return PlayerPrefs.GetInt("currentLevel");
	}

	public static List<bool> GetLevelStates()
	{
		var stateString = PlayerPrefs.GetString("levelStates");
		if(stateString == null)
			stateString = "";

		var outList = new List<bool>();
		
		foreach(var c in stateString)
		{
			if(c == '1')
				outList.Add(true);
			else
				outList.Add(false);
		}

		return outList;
	}

	public static void SetLevelStates(List<bool> levelStates)
	{
		var stateString = "";
		foreach(var lvlState in levelStates)
		{
			if(lvlState)
				stateString += "1";
			else
				stateString += "0";
		}
		
		PlayerPrefs.SetString("levelStates", stateString);
		PlayerPrefs.Save();
	}

	public static void SetCurrentLevelComplete()
	{
		var idx = Mathf.Clamp(GetCurrentLevel(), 0, 1024);
		var states = GetLevelStates();

		while(states.Count <= idx)
			states.Add(false);
		
		states[idx] = true;
		SetLevelStates(states);
	}

	public static bool MusicSetting
	{
		get
		{
			var musicSettings = PlayerPrefs.GetInt("Settings_Music", 1);
			if(musicSettings == 1)
				return true;
			return false;
		}
		set
		{
			if(value == true)
				PlayerPrefs.SetInt("Settings_Music", 1);
			else
				PlayerPrefs.SetInt("Settings_Music", 0);
			PlayerPrefs.Save();
		}
	}

	public static bool SoundSetting
	{
		get
		{
			var sfxSettings = PlayerPrefs.GetInt("Settings_Sfx", 1);
			if(sfxSettings == 1)
				return true;
			return false;
		}
		set
		{
			if(value == true)
				PlayerPrefs.SetInt("Settings_Sfx", 1);
			else
				PlayerPrefs.SetInt("Settings_Sfx", 0);
			PlayerPrefs.Save();
		}
	}

	public static string TrackingDataString
	{
		get
		{
			return PlayerPrefs.GetString("ProgressTracking");
		}

		set
		{
			PlayerPrefs.SetString("ProgressTracking", value);
			PlayerPrefs.Save();
		}
	}

	public static bool CongratulationsShown
	{
		get
		{
			return PlayerPrefs.GetString("Congratulations") == "y";
		}

		set
		{
			if(value)
				PlayerPrefs.SetString("Congratulations", "y");
			else
				PlayerPrefs.SetString("Congratulations", "n");
			
			PlayerPrefs.Save();
		}
	}

	public static bool[] TutorialStates
	{
		get
		{
			var outBools = new bool[5];
			if(!PlayerPrefs.HasKey("TutorialSeen"))
				return outBools;
			
			var seenString = PlayerPrefs.GetString("TutorialSeen");
			var numItems = Mathf.Min(5, seenString.Length);

			for(int i=0; i<numItems; i++)
				outBools[i] = seenString[i] == 'y';

			return outBools;
		}
		set
		{
			var serialised = "";
			for(int i=0; i<value.Length; i++)
			{
				if(value[i])
					serialised += "y";
				else
					serialised += "n";
			}

			PlayerPrefs.SetString("TutorialSeen", serialised);
			PlayerPrefs.Save();
		}
	}

	public static void WipeAll()
	{
		SetLevelStates(new List<bool>());
		TrackingDataString = "";
		CongratulationsShown = false;
		TutorialStates = new bool[5];
	}
}
