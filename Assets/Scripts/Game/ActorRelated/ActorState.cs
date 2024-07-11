using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ActorState
{
	public Vector2 position;
	public bool isActive;

	public ActorState(Actor actor)
	{
		position = actor.currentPosition;
		isActive = actor.isActive;
	}
}
