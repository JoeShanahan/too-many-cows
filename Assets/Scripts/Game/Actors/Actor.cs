using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* An Actor is an object that moves once every turn */
public class Actor : MonoBehaviour
{
	/* State Information */
	public bool isActive;
	public bool rewinding;
	protected float percentMoved;
	protected List<ActorState> stateHistory = new List<ActorState>();
	float currentScale;

	/* Positions */
	public Vector2 targetPosition;
	public Vector2 currentPosition;
	public Vector2 startPosition;
	
	/* Parent & Child */
	public Actor parentActor;
	protected Actor childActor;

	/* Visual Stuff */
	protected DepthFaker depthFaker;	// I think the actorscript should be ignorant of the depth faker

	/* Virtual Methods */
	public virtual void CalculateIfActive(){}
	public virtual void FaceDirection(){}
	protected virtual void RewindRoute(){}
	public virtual void AdvanceOne(){}

	protected virtual float zPos {
		get { return -3; }
	}

	protected virtual void SetMoveExtras(float percent)
	{
		
	}
	
	public List<Vector2> GetAllTouchedTiles()
	{
		var outList = new List<Vector2>();

		foreach(var state in stateHistory)
			outList.Add(state.position);
		outList.Add(targetPosition);

		return outList;
	}

	void Start()
	{
		depthFaker = GetComponent<DepthFaker>();
	}

	public void DoUpdate()
	{
		transform.localPosition = new Vector3(currentPosition.x, currentPosition.y, zPos);
		FaceDirection();
		CalculateIfActive();
		HandleScale();

		if(depthFaker != null)
			AddBob();	
	}

	void HandleScale()
	{
		// TODO Change this to a smooth in/out effect for more control
		if(isActive)
			currentScale = FPoint.Lerp(currentScale, 1, Time.deltaTime*12);
		else
			currentScale = FPoint.Lerp(currentScale, 0, Time.deltaTime*12);
		
		currentScale = Mathf.Clamp(currentScale, 0, 1);

		transform.localScale = Vector3.one * currentScale;
		
		if(currentScale < 0.01f && gameObject.activeSelf)
			gameObject.SetActive(false);
		
		if(currentScale > 0.01f && !gameObject.activeSelf)
			gameObject.SetActive(true);
	}

	public void setChildActor(Actor actor)
	{
		childActor = actor;
		actor.parentActor = this;
	}

	public void Rewind()
	{
		if(stateHistory.Count == 0)
			return;

		var lastState = stateHistory[stateHistory.Count-1];
		stateHistory.RemoveAt(stateHistory.Count-1);

		startPosition = currentPosition;
		targetPosition = lastState.position;
		
		SetMovePercent(0);

		if(childActor != null)
			childActor.Rewind();
		
		RewindRoute();
	}

	public void SetMovePercent(float percent)
	{
		percentMoved = Mathf.Clamp(percent, 0, 1);
		currentPosition = Vector3.Lerp(startPosition, targetPosition, percentMoved);
		SetMoveExtras(percent);
	}

	public void SetTargetPosition(Vector2 newPosition)
	{
		newPosition = new Vector2(Mathf.Round(newPosition.x), Mathf.Round(newPosition.y));

		SetMovePercent(1f);
		startPosition = currentPosition;
		targetPosition = newPosition;

		stateHistory.Add(new ActorState(this));

		if(childActor != null)
			childActor.SetTargetPosition(currentPosition);
	}

	public void CompleteMovement()
	{
		startPosition = currentPosition = targetPosition;
		percentMoved = 1;
		if(childActor != null)
			childActor.CompleteMovement();
	}

	public virtual Vector3 GetFacingDirection()
	{
		if(parentActor == null)
			return Vector3.zero;

		return (transform.position - parentActor.transform.position).normalized;
	}

	public virtual void SetStartPosition(Vector2 position)
	{
		currentPosition = startPosition = targetPosition = position;
	}

	public Vector2 PreviousPosition()
	{
		if(stateHistory.Count == 0)
			return currentPosition;
		
		return stateHistory[stateHistory.Count-1].position;
	}

	void AddBob()
	{
		depthFaker.currentHeight = Mathf.Clamp(Mathf.Abs(Mathf.Sin(percentMoved * 3.1415f * 2)) - 0.2f, 0, 1) * currentScale;
		depthFaker.bouncing = percentMoved > 0 && percentMoved < 1;
	}
}
