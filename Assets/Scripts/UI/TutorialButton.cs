using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialButton : MonoBehaviour
{
	public float normalPosY = 100;
	public float adPosY = 160;
	public bool onScreen = true;
	public TutorialScreen tutorialScreen;
	public PuzzleHandler puzzle;

	RectTransform _rt;
	Vector2 _activePos;
	Vector2 _inactivePos;


	// Use this for initialization
	void Start ()
	{
		_rt = GetComponent<RectTransform>();
		_activePos = new Vector2(_rt.anchoredPosition.x, normalPosY);
		_inactivePos = _activePos - new Vector2(0, 300);
	}
	
	// Update is called once per frame
	void Update ()
	{	
		onScreen = CalculateActive();
		HandlePosition();

	}

	bool CalculateActive()
	{
		if(!tutorialScreen.levelHasTutorial)
			return false;

		if(tutorialScreen.onScreen)
			return false;

		if(puzzle.FarmerInBarn())
			if(puzzle.currentState == PuzzleState.Unsolved || puzzle.currentState == PuzzleState.Solved)
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
}
