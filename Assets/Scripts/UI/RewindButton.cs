using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewindButton : MonoBehaviour
{
	public PuzzleHandler puzzle;
	public Image glowImage;
	public float normalPosY = 90;
	public float adPosY = 150;

	RectTransform _rt;
	Vector2 _activePos;
	Vector2 _inactivePos;
	float _glowPhase = 0f;

	public bool onScreen = true;

	AdvancedButton _advBtn;
	CanvasGroup _grp;

	// Use this for initialization
	void Start ()
	{
		_rt = GetComponent<RectTransform>();

		_activePos = new Vector2(_rt.anchoredPosition.x, normalPosY);
		_inactivePos = _activePos - new Vector2(0, 300);
		_advBtn = GetComponent<AdvancedButton>();
	}

	// Update is called once per frame
	void Update ()
	{	
		if (Input.GetKeyDown(KeyCode.Z))
			_advBtn.KeyboardKeyPressed(true);
		
		if (Input.GetKeyUp(KeyCode.Z))
			_advBtn.KeyboardKeyReleased(false);
		
		onScreen = CalculateActive();
		HandlePosition();
		_HandleBtnGlow();
		_HandleHold();
	}

	void _HandleHold()
	{
		if(_advBtn.holdTime > 0.5f)
			puzzle.Rewind();
	}

	bool CalculateActive()
	{
		if(puzzle.currentState == PuzzleState.Solved)
			return false;

		if(puzzle.movesTaken == 0)
			return false;

		if(puzzle.currentState == PuzzleState.Unsolved && puzzle.FarmerInBarn())
			return false;

		return true;
	}

	void HandlePosition()
	{
		if(onScreen)
			_rt.anchoredPosition = Vector2.Lerp(_rt.anchoredPosition, _activePos, Time.deltaTime * 9);
		else
			_rt.anchoredPosition = Vector2.Lerp(_rt.anchoredPosition, _inactivePos, Time.deltaTime * 9);
	}

	void _HandleBtnGlow()
	{
		var failed = !(puzzle.currentState == PuzzleState.Unsolved || puzzle.currentState == PuzzleState.Solved);

		if(failed)
		{
			glowImage.enabled = true;
			_glowPhase += Time.deltaTime;

			var a = Mathf.PingPong(_glowPhase, 0.9f) * 200;
			glowImage.color = new Color32(255, 255, 55, (byte)a);
		}
		else
		{
			glowImage.enabled = false;
			_glowPhase = 0;
		}
	}

}
