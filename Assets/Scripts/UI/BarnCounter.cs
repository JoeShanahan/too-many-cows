using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TooManyCows.Audio;

public class BarnCounter : MonoBehaviour
{
	public Text numberText;
	public PuzzleHandler puzzle;

	int _numCows = 0;
	int _prevNumber = 0;
	
	void Update ()
	{	
		var newNumber = _numCows - puzzle.movesTaken;

		// Updating the number redraws the canvas, so don't do it if it hasn't changed
		if(newNumber == _prevNumber)
			return;

		if(newNumber <= 0)
			numberText.text = "";
		else
			numberText.text = newNumber.ToString();		

		_prevNumber = newNumber;
	}		

	public void SetNumberOfCows(int cows)
	{
		_numCows = cows;
	}

	public void SetBarnPosition(Vector2 position)
	{
		var gridPos = puzzle.grid.transform.position;
		var gridScale = puzzle.grid.transform.localScale.x;
		transform.position = new Vector3(gridPos.x, gridPos.y, transform.position.z);
		transform.position += new Vector3(position.x, position.y, 0) * gridScale;
	}
}
