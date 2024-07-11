using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuBtn : MonoBehaviour
{
	public PuzzleHandler puzzle;

	Vector2 _activePos;
	Vector2 _inactivePos;
	RectTransform _rect;
	Button _btn;

	bool _isActive;

	// Use this for initialization
	void Start ()
	{
		_rect = GetComponent<RectTransform>();
		_activePos = _rect.anchoredPosition;
		_inactivePos = _activePos + new Vector2(150, 0);
		_btn = GetComponent<Button>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.M) && _isActive)
			_btn.onClick.Invoke();

		_isActive = CalculateActive();
		SetPosition();
	}

	bool CalculateActive()
	{
		if(puzzle.isPaused)
			return false;

		if(puzzle.currentState == PuzzleState.Unsolved)
			return true;

		if(puzzle.currentState == PuzzleState.UnknownCollision)
			return true;

		return false;
	}

	void SetPosition()
	{
		if(_isActive)
			_rect.anchoredPosition = Vector2.Lerp(_rect.anchoredPosition, _activePos, Time.deltaTime * 9);
		else
			_rect.anchoredPosition = Vector2.Lerp(_rect.anchoredPosition, _inactivePos,  Time.deltaTime * 4);
	}
}
