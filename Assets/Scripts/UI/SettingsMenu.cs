using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TooManyCows.Audio;

public class SettingsMenu : MonoBehaviour
{
	public MainMenu mainMenu;
	public AdvancedSwitch musicSwitch;
	public AdvancedSwitch soundSwitch;

	public void BtnPressStats()
	{
		mainMenu.menuMode = MainMenuMode.Statistics;
	}

	public void ActivateMenu(bool makeActive)
	{
		if(makeActive)
		{
			Settings.LoadSettings();
			musicSwitch.SwitchToggled = Settings.musicOn;
			soundSwitch.SwitchToggled = Settings.sfxOn;
		}
		else
		{
			mainMenu.menuMode = MainMenuMode.TopLevel;
			AudioManager.PlaySound(SoundEffect.AcceptPressed);
			Settings.SaveSettings();
		}
	}

	void SwitchPressed()
	{
		Settings.musicOn = musicSwitch.toggled;
		Settings.sfxOn = soundSwitch.toggled;
	}

	// Use this for initialization
	void Start ()
	{
		musicSwitch.onPressMethods.Add(SwitchPressed);
		soundSwitch.onPressMethods.Add(SwitchPressed);
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
