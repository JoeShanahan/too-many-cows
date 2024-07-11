using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TooManyCows.Audio;
using UnityEngine.EventSystems;

public class TutorialScreen : MonoBehaviour
{
	public bool onScreen = false;
	public bool levelHasTutorial = false;
	int _currentLevel;
	float _bgAlpha = 0f;
	float _targetBgAlpha = 99f;

	public MenuLerper panel;
	public RectTransform panelRect;		//ht 580-720
	public Image blackBg;
	public Image screenshotImage;
	public CanvasGroup canvasGrp;
	public GameObject btnObj;

	public Text titleTextObj;
	public Text descTextObj;

	public Sprite[] screenshots;
	
	string[] titleText = {
		TutorialStrings.MovingTitle,
		TutorialStrings.CollideTitle,
		TutorialStrings.RewindTitle,
		TutorialStrings.EmptyTileTitle,
		TutorialStrings.TimeLimitTitle
	};

	string[] descText = {
		TutorialStrings.MovingText,
		TutorialStrings.CollideText,
		TutorialStrings.RewindText,
		TutorialStrings.EmptyTileText,
		TutorialStrings.TimeLimitText
	};

	public bool IsFullyHidden()
	{
		return !onScreen && _bgAlpha == 0;
	}

	// Use this for initialization
	void Start ()
	{
		SetPopupEnabled(false);
		_currentLevel = SaveData.GetCurrentLevel();

		if(_currentLevel < 1 || _currentLevel > 5)
			return;

		levelHasTutorial = true;

		titleTextObj.text = titleText[_currentLevel-1];
		descTextObj.text = descText[_currentLevel-1];
		screenshotImage.sprite = screenshots[_currentLevel-1];

		_FixForShortImage();
	}

	void _FixForShortImage()
	{
		if(_currentLevel != 5)
			return;

		panelRect.sizeDelta = new Vector2(panelRect.sizeDelta.x, 580);
	}

	// Update is called once per frame
	void Update ()
	{
		_HandlePosition();
		_HandleBgAlpha();
	}

	public void BtnPressOpenTutorial()
	{
		AudioManager.PlaySound(SoundEffect.TutorialOpen);
		SetPopupEnabled(true);
	}

	public void BtnPressCloseTutorial()
	{
		AudioManager.PlaySound(SoundEffect.TutorialClose);		
		SetPopupEnabled(false);
	}

	public void SetPopupEnabled(bool enabled)
	{
		if(!levelHasTutorial)
			enabled = false;

		onScreen = enabled;
		canvasGrp.interactable = canvasGrp.blocksRaycasts = enabled;

		if(enabled)
		{
			EventSystem.current.SetSelectedGameObject(btnObj);
			_SetTutorialSeen();
		}
	}

	void _SetTutorialSeen()
	{
		var tutorialStates = SaveData.TutorialStates;
		if(tutorialStates[_currentLevel-1] == true)
			return;

		tutorialStates[_currentLevel-1] = true;
		SaveData.TutorialStates = tutorialStates;
	}

	void _HandlePosition()
	{
		if(onScreen)
			panel.positionMultiplier = 0;
		else
			panel.positionMultiplier = 1;
	}

	void _HandleBgAlpha()
	{
		if(onScreen && _bgAlpha < _targetBgAlpha)
			_bgAlpha += _targetBgAlpha * Time.deltaTime * 3;
		
		if(!onScreen && _bgAlpha > 0)
			_bgAlpha -= _targetBgAlpha * Time.deltaTime * 3;

		_bgAlpha = Mathf.Clamp(_bgAlpha, 0, _targetBgAlpha);

		if(blackBg.color.a != _bgAlpha)
			blackBg.color = new Color32(0, 0, 0, (byte)_bgAlpha);
	}

	public void DecideIfShouldBeActive()
	{
		
	}
}
