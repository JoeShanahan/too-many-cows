using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cow : Actor
{
	public FollowEyes eyes;

	Vector2 startBarnPosition;
	Vector2 endBarnPosition;

	public override void FaceDirection()
	{
		eyes.lookDirection =  GetFacingDirection();
	}

	public override void CalculateIfActive()
	{	
		if(FPoint.isEqual(targetPosition, startBarnPosition))
		{	
			isActive = false;			
		}
		else if(FPoint.isEqual(targetPosition, endBarnPosition))
		{
			isActive = false;
		}
		else
		{
			isActive = true;
		}
	}

	public override void SetStartPosition(Vector2 position)
	{
		base.SetStartPosition(position);
		startBarnPosition = position;
	}

	public void SetEndPosition(Vector2 position)
	{
		endBarnPosition = position;
	}
}
