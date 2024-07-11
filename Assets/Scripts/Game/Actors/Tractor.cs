using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tractor : Actor
{
	public Vector2[] route;
	int currentRouteIdx = 0;

	Quaternion faceLeft = Quaternion.Euler(new Vector3(0, 90, 0));
	Quaternion faceRight = Quaternion.Euler(new Vector3(0, -90, 0));
	Quaternion faceDown = Quaternion.Euler(new Vector3(-20, 0, 0));
	Quaternion faceUp = Quaternion.Euler(new Vector3(65, 180, 0));

	Transform colorObj;
	Transform shadowObj;

	public Transform frontWheels;
	public Transform backWheels;

	protected override float zPos
	{
		get { return -4; }
	}

	protected override void SetMoveExtras(float percent)
	{
		var multiplier = -1;
		if(rewinding)
			multiplier = 1;

		var backAngle = Quaternion.Euler(new Vector3(multiplier*180*percent, 0, 180));
		var frontAngle = Quaternion.Euler(new Vector3(multiplier*360*percent, 0, 180));
		frontWheels.localRotation = frontAngle;
		backWheels.localRotation = backAngle;
	}

	public void Awake()
	{
		colorObj = transform.GetChild(0);
		shadowObj = transform.GetChild(1);
	}

	public override void FaceDirection()
	{
		var targetVector = targetPosition - startPosition;
		var direction = Vector2.left;

		if(targetVector.magnitude < 0.1f || rewinding)
		{
			if(route.Length > 0)
			{
				var nextIdx = currentRouteIdx % route.Length;
				direction = route[nextIdx];
			}
		}
		else
		{
			targetVector.Normalize();
			
			foreach(var vec in new Vector2[]{Vector2.up, Vector2.down, Vector2.left, Vector2.right})
				if((targetVector - vec).magnitude < 0.1f)
					direction = vec;
		}

		var tgtRotation = Quaternion.Euler(0, 0, 0);

		if(direction == Vector2.up)
			tgtRotation = faceUp;
		else if(direction == Vector2.down)
			tgtRotation = faceDown;
		else if(direction == Vector2.left)
			tgtRotation = faceLeft;
		else if(direction == Vector2.right)
			tgtRotation = faceRight;

		colorObj.rotation = shadowObj.rotation = Quaternion.Lerp(colorObj.rotation, tgtRotation, Time.deltaTime*10);
		

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
		if(amt<0)
			rewinding = true;
	}

	
}
