using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TooManyCows.DataObjects;

public class LevelSpawner : MonoBehaviour
{
	public PuzzleHandler puzzleHandler;
	public PuzzleGrid puzzleGrid;
	public LevelLoader loader;

	[Header("UI Stuff")]
	public DayTimer dayTimer;
	public BarnCounter barnCounter;
	public TileHighlighter tileHighlighter;

	[Header("Actor Prefabs")]
	public GameObject farmerPrefab;
	public GameObject cowPrefab;
	public GameObject tractorPrefab;
	public GameObject sheepPrefab;


	// Use this for initialization
	void Start ()
	{	
		
		puzzleGrid.SetLevelSize(loader.levelData.levelSize);		
		puzzleGrid.SpawnTiles(loader.levelData.TileData);
		puzzleGrid.InitDynamicTiles(loader.levelData.numCows);
		puzzleGrid.SpawnFences(loader.levelData.FenceData);
		SpawnTractors(loader.levelData.TractorData, loader.levelData.isTransposed);
		SpawnSheep(loader.levelData.SheepData, loader.levelData.isTransposed);
		SpawnCowTrain();

		dayTimer.SetNumberOfMoves(loader.levelData.moveCount);
		barnCounter.SetNumberOfCows(loader.levelData.numCows);
		barnCounter.SetBarnPosition(puzzleGrid.startBarnPos);
		tileHighlighter.SetLevelSize(loader.levelData.levelSize);
		puzzleGrid.moveLimit = loader.levelData.moveCount;	

	}
	
	void SpawnSheep(SheepData[] sheepList, bool isTransposed)
	{
		var trackCalculator = new GridTrackCalculator();

		foreach(var sheep in sheepList)
		{
			var sheepObj = Instantiate(sheepPrefab, transform);
			var sheepActor = sheepObj.GetComponent<Sheep>();

			sheepActor.SetStartPosition(sheep.startPosition);
			sheepActor.route = sheep.route;

			puzzleHandler.actorManager.AddActor(sheepActor);

			var prevActor = sheepActor;

			for(int i=1; i<sheep.numSheep; i++)
			{	
				var _sheepObj = Instantiate(sheepPrefab, transform);
				var _sheepActor = _sheepObj.GetComponent<Sheep>();

				_sheepActor.SetStartPosition(sheep.startPosition);
				prevActor.setChildActor(_sheepActor);
				prevActor = _sheepActor;

				puzzleHandler.actorManager.AddActor(_sheepActor);			
			}

			sheepActor.DoALoop();
			sheepActor.Activate();
			trackCalculator.AddSheepPath(sheep, isTransposed);
		}
		
		puzzleGrid.SpawnTracks(trackCalculator.TrackDataList);	
	}

	void SpawnTractors(TractorData[] tractorList, bool isTransposed)
	{
		var trackCalculator = new GridTrackCalculator();		

		foreach(var tractor in tractorList)
		{
			var tractorObj = Instantiate(tractorPrefab, transform);
			var tractorActor = tractorObj.GetComponent<Tractor>();

			tractorActor.SetStartPosition(tractor.startPosition);
			tractorActor.route = tractor.route;

			puzzleHandler.actorManager.AddActor(tractorActor);
			trackCalculator.AddTractorPath(tractor, isTransposed);			
		}

		puzzleGrid.SpawnTracks(trackCalculator.TrackDataList);
	}

	void SpawnCowTrain()
	{
		var startPos = new Vector2(loader.levelData.startPos[0], -loader.levelData.startPos[1]);

		var farmerObj = Instantiate(farmerPrefab, transform);
		var farmerActor = farmerObj.GetComponent<Farmer>();
		
		farmerActor.SetStartPosition(startPos);
		farmerActor.SetEndPosition(puzzleGrid.endBarnPos);
		puzzleHandler.actorManager.farmer = farmerActor;
		puzzleHandler.actorManager.AddActor(farmerActor);

		var prevActor = farmerObj.GetComponent<Actor>();;

		for(int i=0; i<loader.levelData.numCows; i++)
		{
			var cowObj = Instantiate(cowPrefab, transform);
			var cowActor = cowObj.GetComponent<Cow>();

			cowActor.SetStartPosition(puzzleGrid.startBarnPos);
			cowActor.SetEndPosition(puzzleGrid.endBarnPos);
			prevActor.setChildActor(cowActor);
			prevActor = cowActor;

			puzzleHandler.actorManager.AddActor(cowActor);			
		}
	}
}
