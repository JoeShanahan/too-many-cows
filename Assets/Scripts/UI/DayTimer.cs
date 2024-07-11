using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayTimer : MonoBehaviour
{
	public GameObject dotPrefab;
	public RectTransform cursor;
	public PuzzleHandler puzzle;
	public Text CursorText;
	public Text movesLeftText;
	
	public float dotWidth = 250;
	int _numMoves = 1;
	float _textAlpha = 0;

	public void SetNumberOfMoves(int moves)
	{
		if(moves < 1)
			return;

		_numMoves = moves;

		var spacing = dotWidth / moves;
		var dotSize = 16;

		if(spacing < 20)
			dotSize = 12;
		if(spacing < 14)
			dotSize = 8;
		if(spacing < 8)
			dotSize = 6;
	
		for(int i=0; i<=moves; i++)
		{
			
			var newDot = Instantiate(dotPrefab, transform);
			var rt = newDot.GetComponent<RectTransform>();
			rt.anchoredPosition = new Vector3((spacing*i)-(dotWidth/2), 0, 0);

			if(i != 0 && i != moves)
				rt.sizeDelta = Vector3.one * dotSize;
		}
	}

	void Start()
	{
	}

	void SetCursorPosition()
	{
		var currentDot = Mathf.Clamp(puzzle.movesTaken, 0, _numMoves);
		var dotPosition = new Vector3(-dotWidth/2, 28, 0);
		var spacing = dotWidth / _numMoves;
		dotPosition += Vector3.right * spacing * currentDot;
		cursor.anchoredPosition = Vector3.Lerp(cursor.anchoredPosition, dotPosition, Time.deltaTime * 8);
	}

	void SetCursorText()
	{
		var movesTaken = Mathf.Clamp(puzzle.movesTaken, 0, _numMoves);
		var movesLeft = _numMoves - movesTaken;

		CursorText.gameObject.SetActive(movesLeft > 0 && movesLeft < 10);
		CursorText.text = movesLeft.ToString();

		if(movesLeft > 1)
			movesLeftText.text = movesLeft.ToString() + " moves";
		else if(movesLeft == 1)
			movesLeftText.text = "1 move";
		else
			movesLeftText.text = "0 moves";

	}	

	void SetTextAlpha()
	{
		var movesTaken = Mathf.Clamp(puzzle.movesTaken, 0, _numMoves);
		var movesLeft = _numMoves - movesTaken;

		if(movesLeft < 1 && puzzle.FarmerInBarn())
			_textAlpha -= Time.deltaTime * 2;
		else
			_textAlpha += Time.deltaTime * 6;

		_textAlpha = Mathf.Clamp(_textAlpha, 0, 1);
		var c = movesLeftText.color;

		movesLeftText.color = new Color(c.r, c.g, c.b, _textAlpha);
	}

	// Update is called once per frame
	void Update ()
	{
		SetCursorPosition();
		SetCursorText();
		SetTextAlpha();
	}
}
