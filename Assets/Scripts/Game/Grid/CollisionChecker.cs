using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
	public PuzzleHandler puzzleController;
	public float collisionDistance = 0.2f;

	public Actor[] CheckAllCollisions()
	{
		var numActors = puzzleController.actorManager.actorList.Count;

		for(int i=0; i<numActors; i++)
		{	
			var actor1 = puzzleController.actorManager.actorList[i];

			if(!actor1.isActive)
				continue;

			for(int j=i+1; j<numActors; j++)
			{
				var actor2 = puzzleController.actorManager.actorList[j];
				
				if(!actor2.isActive)
					continue;

				var distance = (actor1.currentPosition - actor2.currentPosition).magnitude;
				
				if(distance <= collisionDistance)
				{
					return new Actor[] {actor1, actor2};
				}
			}
		}
		return null;
	}

	public PuzzleState GetCollisionState(Actor[] actors)
	{
		if(actors.Length != 2)
		{
			Debug.LogWarning("Only two actors are supposed to collide, not " + actors.Length.ToString());
			return PuzzleState.UnknownCollision;
		}
		
		var a1 = actors[0].GetType();
		var a2 = actors[1].GetType();

		var aCow = 	    a1 == typeof(Cow)     || a2 == typeof(Cow);
		var aFarmer =   a1 == typeof(Farmer)  || a2 == typeof(Farmer);
		var aTractor =  a1 == typeof(Tractor) || a2 == typeof(Tractor);
		var aSheep = 	a1 == typeof(Sheep)   || a2 == typeof(Sheep);

		if(aCow && aTractor)
			return PuzzleState.CowHitTractor;
		if(aFarmer && aTractor)
			return PuzzleState.PlayerHitTractor;
		if(aCow && aFarmer)
			return PuzzleState.PlayerHitCow;
		if(aCow && aSheep)
			return PuzzleState.CowHitSheep;
		if(aFarmer && aSheep)
			return PuzzleState.PlayerHitSheep;
		
		Debug.LogWarning("Unhandled collision between: " + actors[0].name + " & " + actors[1].name);
		return PuzzleState.UnknownCollision;
	}
}
