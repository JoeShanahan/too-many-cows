using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressToSpeedUp : MonoBehaviour
{
	public PuzzleHandler puzzle;

	float _alpha = 0;
	float _holdAlpha = 0;
	CanvasGroup _grp;

	// Use this for initialization
	void Start ()
	{
		_grp = GetComponent<CanvasGroup>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		IncrementAlpha();
		IncrementHold();

		var alphaA = 0.1f + ((Mathf.Sin(Time.time * 3) + 1) / 4.0f);
		alphaA = Mathf.Max(alphaA, _holdAlpha);


		_grp.alpha = alphaA * _alpha;
	}

	void IncrementHold()
	{
		var active = puzzle.FarmerInBarn() && puzzle.currentState == PuzzleState.Unsolved;
		if(!active)
			return;

		if(Input.GetMouseButton(0))
			_holdAlpha += Time.deltaTime * 5;
		else
			_holdAlpha -= Time.deltaTime * 0.8f;
		_holdAlpha = Mathf.Clamp(_holdAlpha, 0, 1);
	}

	void IncrementAlpha()
	{
		var active = puzzle.FarmerInBarn() && puzzle.currentState == PuzzleState.Unsolved;
		
		if(active)
			_alpha += Time.deltaTime * 2;
		else
			_alpha -= Time.deltaTime * 2;
		_alpha = Mathf.Clamp(_alpha, 0, 1);
	}
}
