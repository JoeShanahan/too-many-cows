using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TooManyCows.Audio;

public class AdvancedSwitch : MonoBehaviour
{
	public bool toggled = true;
	public RectTransform onRect;
	public RectTransform offRect;

	public Image onImage;
	public Image offImage;

	[Header("Text")]
	public Text onText;
	public Text offText;

	[Header("Image Color")]
	public Color32 normalColor;
	public Color32 downColor;

	[Header("Text Color")]
	public Color32 onTextColor;
	public Color32 offTextColor;
	public Color32 inactiveTextColor;

	[Header("Height Values")]
	public float upAmount;
	public float downAmount;

	public List<Action> onPressMethods = new List<Action>();

	// Use this for initialization
	void Start ()
	{
		_UpdateState();
	}

	public void Toggle()
	{
		if (toggled)
			OffPressed();
		else
			OnPressed();
	}

	public bool SwitchToggled
	{
		get 
		{
			return toggled;
		}
		set
		{
			toggled = value;
			_UpdateState();
		}
	}

	public void OnPressed()
	{
		if(toggled == true)
			return;

		toggled = true;
		_UpdateState();
		foreach(var act in onPressMethods)
			act.Invoke();
		AudioManager.PlaySound(SoundEffect.SwitchToggle);		
		
	}

	public void OffPressed()
	{
		if(toggled == false)
			return;
		
		toggled = false;
		_UpdateState();
		foreach(var act in onPressMethods)
			act.Invoke();
		AudioManager.PlaySound(SoundEffect.SwitchToggle);		
				
	}

	void _UpdateState()
	{	
		if(toggled)
		{
			onRect.anchoredPosition = new Vector2(onRect.anchoredPosition.x, downAmount);
			offRect.anchoredPosition = new Vector2(offRect.anchoredPosition.x, upAmount);

			onImage.color = downColor;
			offImage.color = normalColor;

			onText.color = onTextColor;
			offText.color = inactiveTextColor;
		}
		else
		{
			onRect.anchoredPosition = new Vector2(onRect.anchoredPosition.x, upAmount);
			offRect.anchoredPosition = new Vector2(offRect.anchoredPosition.x, downAmount);

			onImage.color = normalColor;
			offImage.color = downColor;

			onText.color = inactiveTextColor;
			offText.color = offTextColor;
		}
	}
}
