using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TooManyCows.UI;
using TooManyCows.Audio;

public enum MainMenuMode {None, TopLevel, Settings, Statistics, Credits, Congratulations};

public class MainMenu : MonoBehaviour
{	
	public bool active;
	public MainMenuMode menuMode = MainMenuMode.TopLevel;
	public SceneLoader loader;
	public Image bgImage;

	[Header("Lerper Objects")]
	public MenuLerper topLevelLerper;
	public MenuLerper settingsLerper;
	public MenuLerper statsLerper;
	public MenuLerper creditsLerper;
	public MenuLerper congratulationsLerper;

	[Header("Menu Objects")]
	public SettingsMenu settingsMenu;
	public CreditsMenu creditsScreen;

	[Space(8)]
	public RectTransform[] screenContent;
	public Vector2[] activePositions;
	public Vector2[] inactivePositions;

	float _activePercent = 0f;
	float _targetBlackAlpha = 0.75f;
	int _wipePressCount = 0;
	CanvasGroup _canvasGrp;
	
	// Use this for initialization
	void Start ()
	{
		_canvasGrp = GetComponent<CanvasGroup>();
		_InitActivePositions();
	}

	void _InitActivePositions()
	{
		activePositions[0] -= new Vector2(0, 45);
		activePositions[1] = inactivePositions[1];
		activePositions[2] += new Vector2(0, 70);
		activePositions[3] += new Vector2(0, 70);
	}

	public void SetIsActive(bool yesno)
	{
		active = yesno;

		if(yesno)
		{
			AudioManager.PlaySound(SoundEffect.MenuOpen);
			menuMode = MainMenuMode.TopLevel;
		}
		else
		{
			AudioManager.PlaySound(SoundEffect.MenuClose);
			menuMode = MainMenuMode.None;
		}
	}
	
	void _HandleEscapeKey()
	{
		if(Input.GetKeyDown(KeyCode.M) && !active)
			SetIsActive(true);
	}

	// Update is called once per frame
	void Update ()
	{
		HandleActiveTimer();
		HandleElementsPosition();
		HandleCanvasGroup();
		_HandleMenuPositions();
		_HandleMusic();
		_HandleBlackBg();
		_HandleEscapeKey();
	}

	void _HandleMusic()
	{
		MusicManager.menuUp = active;
	}

	void _HandleBlackBg()
	{	
		var alpha = FPoint.Lerp(bgImage.color.a, _targetBlackAlpha, Time.deltaTime*4);
		bgImage.color = new Color(0, 0, 0, alpha);
	}

	void HandleActiveTimer()
	{
		if(active)
			_activePercent += Time.deltaTime * 3;
		else
			_activePercent -= Time.deltaTime * 3;

		_activePercent = Mathf.Clamp(_activePercent, 0, 1);
	}

	void HandleCanvasGroup()
	{
		_canvasGrp.blocksRaycasts = active;
		_canvasGrp.interactable = active;
		_canvasGrp.alpha = Mathf.Clamp(_activePercent, 0, 1);
	}

	void HandleElementsPosition()
	{
		for(int i=0; i<screenContent.Length; i++)
		{
			var cont = screenContent[i];

			if(menuMode != MainMenuMode.None && menuMode != MainMenuMode.Congratulations)
				cont.anchoredPosition = Vector2.Lerp(cont.anchoredPosition, activePositions[i], Time.deltaTime * 9);
			else
				cont.anchoredPosition = Vector2.Lerp(cont.anchoredPosition, inactivePositions[i],  Time.deltaTime * 9);
		}
	}

	public void BtnPressWipeAll()
	{
		AudioManager.PlaySound(SoundEffect.BtnPress);		

		_wipePressCount ++;

		if(_wipePressCount < 8)
			return;

		SaveData.WipeAll();
		loader.LoadScene("MapScene");
	}

	public void BtnPressSettings()
	{
		menuMode = MainMenuMode.Settings;
		settingsMenu.ActivateMenu(true);
		AudioManager.PlaySound(SoundEffect.BtnPress);
	}

	public void BtnPressQuit()
	{
		AudioManager.PlaySound(SoundEffect.BtnPress);		
		Application.Quit();
	}

	public void ViewCredits(bool yesno)
	{
		if(yesno)
		{
			menuMode = MainMenuMode.Credits;
			creditsScreen.timer = 0;
			AudioManager.PlaySound(SoundEffect.BtnPress);
		}
		else
		{
			menuMode = MainMenuMode.Settings;
			AudioManager.PlaySound(SoundEffect.BtnPress);			
		}
	}

	void _HandleMenuPositions()
	{	
		if(menuMode == MainMenuMode.TopLevel)
			_targetBlackAlpha = 0.6f;		
		else if(menuMode == MainMenuMode.Settings)
			_targetBlackAlpha = 0.6f;					
		else if(menuMode == MainMenuMode.Statistics)
			_targetBlackAlpha = 0.8f;								
		else if(menuMode == MainMenuMode.Credits)
			_targetBlackAlpha = 0.8f;											
		else if(menuMode == MainMenuMode.Congratulations)
			_targetBlackAlpha = 0.7f;		

		topLevelLerper.SetMenuMode(menuMode);
		settingsLerper.SetMenuMode(menuMode);
		creditsLerper.SetMenuMode(menuMode);
		statsLerper.SetMenuMode(menuMode);
		congratulationsLerper.SetMenuMode(menuMode);
	}

	public void EnableCongratulations(bool enabled)
	{
		active = enabled;
		if(enabled)
		{
			menuMode = MainMenuMode.Congratulations;
			AudioManager.PlaySound(SoundEffect.AcceptPressed);
		}
		else
		{
			menuMode = MainMenuMode.None;
			AudioManager.PlaySound(SoundEffect.TutorialClose);
		}
	}
}
