using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicTile : MonoBehaviour
{
	public List<TileState> stateHistory = new List<TileState>();
	public int cowsEntered;
	public int cowsLeft;
	protected int totalCows;

	public Vector2 Position()
	{
		return new Vector2(transform.localPosition.x, transform.localPosition.y);
	}

	public void UpdateStateHistory()
	{
		stateHistory.Add(new TileState(cowsEntered, cowsLeft));
	}

	public void Rewind()
	{
		if(stateHistory.Count == 0)
			return;

		var lastState = stateHistory[stateHistory.Count-1];
		stateHistory.RemoveAt(stateHistory.Count-1);

		cowsEntered = lastState.cowsEntered;
		cowsLeft = lastState.cowsLeft;

		UpdateVisualState();
	}

	public void CowEnter()
	{
		EnterEffect();
		cowsEntered += 1;
		UpdateVisualState();		
	}

	public void CowLeave()
	{
		cowsLeft += 1;
		UpdateVisualState();
	}

	public virtual void UpdateVisualState(){}

	protected virtual void EnterEffect() {}

	public void SetTotalCows(int count)
	{
		totalCows = count;
	}
}
