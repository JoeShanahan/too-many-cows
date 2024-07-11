using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHighlighter : MonoBehaviour
{
	public GameObject tilePrefab;
	public TileHitTester hitTester;
	public PuzzleHandler puzzle;
	public float maxAlpha;
	public bool active;

	MaterialPropertyBlock _propBlock;
	float _activePercent;
	Transform[] _tiles;
	Renderer[] _renderers;
	Vector3 _inactivePos = new Vector3(-50, 0, -1);
	public Vector2 _levelSize;

	void Start ()
	{
		_propBlock = new MaterialPropertyBlock();
		MoveHighlights(puzzle.actorManager.farmer.startPosition);
	}
	
	void Update ()
	{
		active = !puzzle.moving && puzzle.currentState == PuzzleState.Unsolved;

		IncrementActive();
		HandleAlpha();
	}

	public void MoveHighlights(int[] playerPosition)
	{
		MoveHighlights(new Vector2(playerPosition[0], playerPosition[1]));
	}

	public void MoveHighlights(Vector2 playerPosition)
	{	
		if(_levelSize == Vector2.zero)
		{
			Debug.LogError("Level size must be set before the highlights can be!");
			return;
		}

		var row = playerPosition.y;
		var column = playerPosition.x;

		foreach(var t in _tiles)
			t.position = _inactivePos;

		var positionsToTest = new List<Vector2>();

		for(int x=0; x<_levelSize.x; x++)
			positionsToTest.Add(new Vector2(x, row));

		for(int y=0; y<_levelSize.y; y++)
			positionsToTest.Add(new Vector2(column, -y));

		var currentIdx = 0;		
		foreach(var newPos in positionsToTest)
		{
			var idx = Mathf.Min(currentIdx, _tiles.Length-1);
			
			if(!hitTester.LocationIsValid(newPos))
				continue;
			
			if(newPos == playerPosition)
				continue;

			_tiles[idx].localPosition = new Vector3(newPos.x, newPos.y, -1);
			currentIdx++;
		}
	}

	void IncrementActive()
	{
		if(active)
			_activePercent += Time.deltaTime * 3;
		else
			_activePercent -= Time.deltaTime * 5;

		_activePercent = Mathf.Clamp(_activePercent, 0, 1);
	}

	void HandleAlpha()
	{
		if(_renderers.Length < 1)
			return;

        _renderers[0].GetPropertyBlock(_propBlock);		
        _propBlock.SetFloat("_Alpha", maxAlpha * _activePercent);

		foreach(var ren in _renderers)
			ren.SetPropertyBlock(_propBlock);
	}

	public void SetLevelSize(int[] levelSize)
	{
		_levelSize = new Vector2(levelSize[0], levelSize[1]);

		var numTiles = _levelSize.x + _levelSize.y - 2;
		var tmpTileList = new List<Transform>();
		var tmpRendList = new List<Renderer>();


		for(int i=0; i<numTiles; i++)
		{
			var newTile = Instantiate(tilePrefab);
			newTile.transform.SetParent(transform);
			newTile.transform.localPosition = _inactivePos;
			newTile.transform.localScale = Vector3.one * 0.95f;

			tmpTileList.Add(newTile.transform);
			tmpRendList.Add(newTile.GetComponent<Renderer>());
		}

		_tiles = tmpTileList.ToArray();
		_renderers = tmpRendList.ToArray();
	}
}
