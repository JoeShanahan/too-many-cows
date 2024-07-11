using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHitTester : MonoBehaviour
{	
	public PuzzleGrid puzzleGrid;
	public PuzzleHandler puzzleHandler;

	public bool LocationIsValid(Vector2 location, bool checkRoute=true)
	{
		if(!WithinLevelBounds(location))
			return false;

		if(!IsAdjacent(location))
			return false;

		if(IsBackwards(location))
			return false;

		if(IsStartBarn(location))
			return false;

		var tile = puzzleGrid.GetTileAt(location);
		
		if(tile == null)
			return false;
		
		if(IsTree(tile))
			return false;

		if(checkRoute && RouteBlocked(location))
			return false;

		if(FenceInTheWay(location))
			return false;

		return true;
	}

	bool WithinLevelBounds(Vector2 location)
	{
		if(location.x < 0 || location.x >= puzzleGrid.levelSize.x)
			return false;

		if(-location.y < 0 || -location.y >= puzzleGrid.levelSize.y)
			return false;

		return true;
	}

	bool IsAdjacent(Vector2 location)
	{	
		var playerPos = puzzleHandler.actorManager.farmer.currentPosition;
		
		if(location.x == playerPos.x || location.y == playerPos.y)
			return true;
		
		return false;
	}

	bool IsBackwards(Vector2 location)
	{	
		if(IsEndBarn(location))
			return false;

		if(location == puzzleHandler.actorManager.farmer.PreviousPosition())
			return true;
		return false;
	}

	bool IsTree(GameObject tile)
	{
		if(tile.name.Contains("TileTree"))
			return true;
		return false;
	}

	public void HookParent(TileHitManager hitManager)
	{
		puzzleHandler = hitManager.levelController;
		puzzleGrid = hitManager.puzzleGrid;
	}

	bool IsStartBarn(Vector2 targetPosition)
	{
		if(targetPosition == puzzleGrid.startBarnPos)
			return true;
		return false;
	}

	bool IsEndBarn(Vector2 targetPosition)
	{
		if(targetPosition == puzzleGrid.endBarnPos)
			return true;
		return false;
	}

	bool RouteBlocked(Vector2 endPosition)
	{
		var startPosition = puzzleHandler.actorManager.farmer.currentPosition;
		var routeVector = endPosition - startPosition;
		var magnitude = Mathf.RoundToInt(routeVector.magnitude);

		for(int i=1; i<magnitude; i++)
		{
			var positionToTest = startPosition + (routeVector.normalized * i);
			if(!LocationIsValid(positionToTest, checkRoute: false))
				return true;

		}

		return false;
	}

	bool FenceInTheWay(Vector2 endPosition)
	{	
		var startPosition = puzzleHandler.actorManager.farmer.currentPosition;
		var routeVector = endPosition - startPosition;
		var magnitude = Mathf.RoundToInt(routeVector.magnitude);

		for(int i=0; i<magnitude; i++)
		{
			var positionToTest = startPosition + (routeVector.normalized * i);
			if(puzzleGrid.GetFenceAt(positionToTest, routeVector.normalized) != null)
				return true;
		}

		return false;
	}
}
