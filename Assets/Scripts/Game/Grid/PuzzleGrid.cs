using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TooManyCows.DataObjects;

public class PuzzleGrid : MonoBehaviour
{
	Dictionary<TileType, GameObject> tileDict;

	public GameObject[] tilePrefabs;
	public GameObject fencePrefab;
	public List<GameObject> tileObjList = new List<GameObject>();
	public List<GameObject> fenceObjList = new List<GameObject>();
	public List<DynamicTile> dynamicTiles = new List<DynamicTile>();

	public Vector2 levelSize;
	public Vector2 startBarnPos;
	public Vector2 endBarnPos;

	public int moveLimit;
	
	void InitTileDict()
	{
		tileDict = new Dictionary<TileType, GameObject>();

		foreach(var gameObj in tilePrefabs)
		{
			if(gameObj.name == "TileGrass")
				tileDict[TileType.Grass] = gameObj;
			if(gameObj.name == "TileBarn")
				tileDict[TileType.BarnStart] = gameObj;
			if(gameObj.name == "TileBarn")
				tileDict[TileType.BarnEnd] = gameObj;
			if(gameObj.name == "TileTree")
				tileDict[TileType.Tree] = gameObj;
			if(gameObj.name == "TileBlank")
				tileDict[TileType.Blank] = gameObj;
		}
	}

	public bool AllGrassEaten()
	{
		foreach(var tile in dynamicTiles)
		{
			var grassTile = tile.GetComponent<GrassTile>();
			if(grassTile != null && grassTile.cowsEntered == 0)
				return false;
		}

		return true;
	}

	GameObject GetTilePrefab(string tilename)
	{
		foreach(var gameObj in tilePrefabs)
		{
			if(gameObj.name == tilename)
				return gameObj;
		}

		return null;
	}

	public void SpawnTiles(TileData[] tileList)
	{
		if(tileDict == null)
			InitTileDict();

		foreach(var tileData in tileList)
		{
			var toSpawn = tileDict[tileData.tileType];
			var newObj = Instantiate(toSpawn, transform);
			newObj.transform.localPosition = tileData.position;
			newObj.transform.localScale = new Vector3(0.95f, 0.95f, 0.95f);
			tileObjList.Add(newObj);

			if(tileData.tileType == TileType.BarnStart)
				startBarnPos = tileData.position;

			if(tileData.tileType == TileType.BarnEnd)
				endBarnPos = tileData.position;
		}
	}

	public void SpawnTracks(TileTrackData[] tileList)
	{
		if(tileDict == null)
			InitTileDict();

		foreach(var trackData in tileList)
		{	
			foreach(var dir in trackData.directionList)
			{	
				var spawnInfo = TileTrackData.trackPrefabInfo[dir];
				
				if(trackData.sheepTracks)
					spawnInfo = TileTrackData.feetPrefabInfo[dir];
				
				var toSpawn = GetTilePrefab(spawnInfo.tilename);
				var newObj = Instantiate(toSpawn, transform);
				newObj.transform.localPosition = trackData.position;
				newObj.transform.rotation = Quaternion.Euler(0, 0, 90 * spawnInfo.rotation);
				newObj.transform.localScale = new Vector3(0.95f * spawnInfo.flipped, 0.95f, 0.95f);
			}
		}
	}

	public void InitDynamicTiles(int numCows)
	{
		foreach(var obj in tileObjList)
		{
			var dynamicScript = obj.GetComponent<DynamicTile>();
			if(dynamicScript == null)
				continue;
			
			dynamicScript.SetTotalCows(numCows);
			dynamicTiles.Add(dynamicScript);
		}
	}

	public void SpawnFences(FenceData[] fenceList)
	{
		foreach(var fenceData in fenceList)
		{
			var newObj = Instantiate(fencePrefab, transform);
			newObj.transform.localPosition = fenceData.position;
			newObj.transform.localPosition += new Vector3(0, 0, -0.75f);
			newObj.transform.localScale = Vector3.one;
			newObj.transform.rotation = Quaternion.Euler(0, 0, fenceData.rotation);
			fenceObjList.Add(newObj);
		}
	}

	public void SetLevelSize(int[] inpLevelSize)
	{	
		levelSize = new Vector2(Mathf.Max(inpLevelSize[0], 1),
								Mathf.Max(inpLevelSize[1], 1));

		var xSize = Mathf.Clamp(14.1f / levelSize.x, 0, 1.5f);
		var ySize = Mathf.Clamp(7.5f / levelSize.y, 0, 1.5f);
		
		var scale = Mathf.Min(xSize, ySize);
		transform.localScale = new Vector3(scale, scale, 1);

		var xSpan = scale * levelSize.x;
		var ySpan = scale * levelSize.y;

		transform.position = new Vector3((0.5f*scale)-xSpan/2, (ySpan/2)-.75f, 0);
	}

	public GameObject GetTileAt(Vector2 position)
	{
		foreach(var tileOb in tileObjList)
		{
			if(!FPoint.isEqual(tileOb.transform.localPosition.x, position.x))
				continue;

			if(!FPoint.isEqual(tileOb.transform.localPosition.y, position.y))
				continue;

			return tileOb;
		}

		return null;
	}

	public GameObject GetFenceAt(Vector2 position, Vector2 direction)
	{	
		var finalPosition = position + (direction * 0.5f);

		foreach(var fenceOb in fenceObjList)
		{
			if(!FPoint.isEqual(fenceOb.transform.localPosition.x, finalPosition.x))
				continue;

			if(!FPoint.isEqual(fenceOb.transform.localPosition.y, finalPosition.y))
				continue;

			return fenceOb;
		}

		return null;
	}

	public void ToggleCowsOnTiles(List<Cow> cowList, bool entering)
	{
		foreach(var actor in cowList)
		{
			foreach(var tile in dynamicTiles)
			{	
				if(actor.startPosition == actor.targetPosition)
					continue;

				var distToTarget = (tile.Position() - actor.targetPosition).magnitude;
				var distToStart = (tile.Position() - actor.startPosition).magnitude;
				

				if(entering)
				{
					if(distToTarget < 0.1f)
						tile.CowEnter();					
				}
				else
				{	
					if(distToStart < 0.1f)
						tile.CowLeave();					
				}
			}
		}
	}
}
