using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHand : MonoBehaviour
{
	public PuzzleHandler puzzleHandler;
	public PuzzleGrid grid;
	public SpriteRenderer sprite;
	int _currentLevel;
	
	int _tgtScale = 1;
	Vector2 _tgtBasePosition;
	Vector2 _basePosition;

	float fluctuation = 0.05f;
	float handSize = 0.18f;
	float _opacity = 0f;

	int lastKnownMoveCount = -1;

	bool[] 	  tut1_pointingLeft = new bool[] { true, true };
	Vector2[]  tut1_tgtPosition = new Vector2[] { new Vector2(3, 0), new Vector2(3, -3) };

	bool[]   tut2_pointingLeft = new bool[] { false, true, true, false };
	Vector2[] tut2_tgtPosition = new Vector2[] { new Vector2(1, -2), new Vector2(4, -2), new Vector2(4, 0), new Vector2(3, -1) };

	// Use this for initialization
	void Start ()
	{
		_currentLevel = SaveData.GetCurrentLevel();
	}

	void _CalculateOpacity()
	{
		var shouldBeVisible = true;

		if(puzzleHandler.moving)
			shouldBeVisible = false;

		if(_tgtBasePosition != _basePosition)
			shouldBeVisible = false;

		if(shouldBeVisible)
			_opacity += Time.deltaTime * 2;
		else
			_opacity -= Time.deltaTime * 4;

		_opacity = Mathf.Clamp(_opacity, 0, 1);

		sprite.color = new Color(1,1,1,_opacity);

		if(_opacity == 0 && _tgtBasePosition != _basePosition)
		{
			_basePosition = _tgtBasePosition;
			transform.localScale = new Vector3(handSize, handSize * _tgtScale, handSize);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		_MakeHandWiggle();
		_CalculateOpacity();

		if(_currentLevel == 1)
			_UpdateHandPosition(tut1_pointingLeft, tut1_tgtPosition);
		else if(_currentLevel == 2)
			_UpdateHandPosition(tut2_pointingLeft, tut2_tgtPosition);			
		else
			Destroy(this.gameObject);

	}

	void _MakeHandWiggle()
	{
		var xpos = _basePosition.x + (Mathf.Sin(Time.time * 3.5f) * fluctuation);
		transform.localPosition = new Vector3(xpos, _basePosition.y, -7);
	}

	void _UpdateHandPosition(bool[] pointingLeft, Vector2[] targetPosition)
	{
		if(puzzleHandler.movesTaken == lastKnownMoveCount)
			return;

		lastKnownMoveCount = puzzleHandler.movesTaken;
		var touchedTiles = puzzleHandler.actorManager.farmer.GetAllTouchedTiles();

		for(int i=0; i<targetPosition.Length; i++)
		{
			if(touchedTiles.Contains(targetPosition[i]))
				continue;
			
			_SetHandPosition(targetPosition[i], pointingLeft[i]);
			break;
		}
	}

	void _SetHandPosition(Vector2 position, bool pointingLeft)
	{
		var tile = grid.GetTileAt(position);
		position = tile.transform.localPosition;

		_tgtBasePosition = new Vector2(position.x, position.y);

		if(pointingLeft)
		{
			_tgtScale = -1;
			_tgtBasePosition += new Vector2(0.85f, 0);
		}
		else
		{
			_tgtScale = 1;
			_tgtBasePosition -= new Vector2(0.85f, 0);			
		}

	}
}
