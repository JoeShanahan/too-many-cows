using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHitManager : MonoBehaviour
{
	Camera mainCamera;
	public PuzzleHandler levelController;
	public PuzzleGrid puzzleGrid;
	public TutorialScreen tutorialScreen;

	// Use this for initialization
	void Start ()
	{
		mainCamera = Camera.main;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(levelController.isPaused)
			return;

		if(tutorialScreen.onScreen)
			return;

		if(Input.GetMouseButtonDown(0))
		{
			var worldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
			var localPosition = worldPosition - puzzleGrid.transform.position;
			localPosition /= puzzleGrid.transform.localScale.x;

			var tilePosition = new Vector2(Mathf.Round(localPosition.x), Mathf.Round(localPosition.y));
			levelController.MoveToTile(tilePosition);
		}
	}
}
