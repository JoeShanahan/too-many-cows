using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TooManyCows.Audio;

public enum TitleState {AnimatingIn, WaitingForInput, Confirming, Loading};

public class TitleScreen : MonoBehaviour
{
	public TitleState currentState;

	[Space(16)]
	public CanvasGroup sunGrp;
	public RectTransform topText;
	public RectTransform bottomText;
	public Transform cowModel;
	public FlashingText pressScreenText;
	public SceneLoader sceneLoader;

	public float _animTime = 0f;
	public float _confirmTime = 0f;
	Vector2 _topTextStartPos;
	Vector2 _topTextEndPos;

	Vector2 _bottomTextStartPos;
	Vector2 _bottomTextEndPos;

	// Use this for initialization
	void Start ()
	{
		_topTextEndPos = topText.anchoredPosition;
		_bottomTextEndPos = bottomText.anchoredPosition;

		topText.anchoredPosition += Vector2.left * 1200f;
		bottomText.anchoredPosition += Vector2.right * 1200f;

		_topTextStartPos = topText.anchoredPosition;
		_bottomTextStartPos = bottomText.anchoredPosition;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(currentState == TitleState.AnimatingIn)
			_AnimateIn();

		else if(currentState == TitleState.WaitingForInput)
			_WaitForInput();

		else if(currentState == TitleState.Confirming)
			_HandleConfirm();

	}

	void _HandleConfirm()
	{
		_confirmTime += Time.deltaTime;
		if(_confirmTime > 0.8f)
		{
			currentState = TitleState.Loading;
			sceneLoader.LoadScene("MapScene");
		}
	}

	void _WaitForInput()
	{
		if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
		{
			currentState = TitleState.Confirming;
			pressScreenText.Activate();
			AudioManager.PlaySound(SoundEffect.TitleConfirm);
		}
	}

	void _AnimateIn()
	{
		_animTime += Time.deltaTime;
		_animTime = Mathf.Clamp(_animTime, 0, 2.7f);

		_AnimateCow(0f, 0.8f);
		_AnimateTopText(0.5f, 0.8f);
		_AnimateBottomText(0.75f, 0.65f);
		_AnimateSun(1.1f, 1.5f);

		if(_animTime > 2)
			pressScreenText.isVisible = true;

		if(_animTime == 2.7f)	
			currentState = TitleState.WaitingForInput;
		
	}

	void _AnimateCow(float delay, float length)
	{
		var t = Mathf.Clamp(_animTime - delay, 0, length) / length;
		cowModel.localScale = Vector3.one * SmoothFunctions.Overgrow(t);
	}

	void _AnimateTopText(float delay, float length)
	{
		var t = Mathf.Clamp(_animTime - delay, 0, length) / length;
		var diff = _topTextEndPos - _topTextStartPos;

		topText.anchoredPosition = _topTextStartPos + (diff * SmoothFunctions.EaseOut(t));
	}

	void _AnimateBottomText(float delay, float length)
	{
		var t = Mathf.Clamp(_animTime - delay, 0, length) / length;
		var diff = _bottomTextEndPos - _bottomTextStartPos;

		bottomText.anchoredPosition = _bottomTextStartPos + (diff * SmoothFunctions.EaseOut(t));		
	}

	void _AnimateSun(float delay, float length)
	{
		var t = Mathf.Clamp(_animTime - delay, 0, length) / length;
		sunGrp.alpha = t;
	}


}
