using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TileState
{
	public int cowsEntered;
	public int cowsLeft;

	public TileState(int entered, int left)
	{
		cowsEntered = entered;
		cowsLeft = left;
	}
}
