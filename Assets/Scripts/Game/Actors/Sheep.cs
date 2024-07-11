using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : Actor
{
	public Vector2[] route;
	int currentRouteIdx = 0;

	public FollowEyes eyes;

	public override void FaceDirection()
	{	
		if(IsLeader())
			eyes.lookDirection = -LeaderFaceDirection();
		else
			eyes.lookDirection =  GetFacingDirection();
	}

	Vector2 LeaderFaceDirection()
	{	
		var targetVector = targetPosition - startPosition;
		if(targetVector.magnitude < 0.1f)
		{ 
			if(route.Length == 0)
				return Vector2.zero;

			return route[currentRouteIdx];
		}

		return targetVector.normalized;
	}

	public bool IsLeader()
	{
		return parentActor == null;
	}

	public void Start()
	{
		depthFaker = GetComponent<DepthFaker>();
	}

	public override void SetStartPosition(Vector2 position)
	{
		base.SetStartPosition(position);
	}

	protected override void RewindRoute()
	{
		IncrementRoute(-1);
	}

	public override void AdvanceOne()
	{
		if(route.Length == 0 || route == null)
			return;

		var newPosition = currentPosition + route[currentRouteIdx];
		IncrementRoute();
		SetTargetPosition(newPosition);
	}

	void IncrementRoute(int amt=1)
	{
		currentRouteIdx += amt;
		if(currentRouteIdx >= route.Length)
			currentRouteIdx = 0;
		if(currentRouteIdx < 0)
			currentRouteIdx = route.Length - 1;
	}

	public void DoALoop()
	{
		for(int i=0; i<route.Length; i++)
		{
			AdvanceOne();
			CompleteMovement();
		}

	}

	public void Activate()
	{
		isActive = true;
		stateHistory.Clear();
		if(childActor != null)
			((Sheep)childActor).Activate();
	}

	
}
