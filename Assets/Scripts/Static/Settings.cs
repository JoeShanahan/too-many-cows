using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
	public static bool musicOn = true;
	public static bool sfxOn = true;

	static bool _beenInitialised = false;
	
	void Start()
	{
		if(Settings._beenInitialised)
			return;

		Settings.LoadSettings();
		Settings._beenInitialised = true;
	}

	public static void LoadSettings()
	{
		musicOn = SaveData.MusicSetting;
		sfxOn = SaveData.SoundSetting;
	}

	public static void SaveSettings()
	{
		SaveData.MusicSetting = musicOn;
		SaveData.SoundSetting = sfxOn;
	}
}
