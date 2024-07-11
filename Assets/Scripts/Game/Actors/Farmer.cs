using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farmer : Actor
{
    public FollowEyes eyes;

	Vector2 endBarnPosition = new Vector2(-1, -1);

	public override void FaceDirection()
	{
		eyes.lookDirection =  GetFacingDirection();
	}

	public override Vector3 GetFacingDirection()
	{
		var lookVector = (currentPosition - targetPosition);
		if(lookVector.magnitude > 0.1f)
			return lookVector.normalized;
			
		return Vector3.zero;
	}

	public override void CalculateIfActive()
	{
		if(FPoint.isEqual(targetPosition, endBarnPosition))
			isActive = false;
		else
			isActive = true;
	}

	public void SetEndPosition(Vector2 position)
	{
		endBarnPosition = position;
	}
}
