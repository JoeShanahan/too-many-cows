using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TooManyCows.Tutorials;
using TooManyCows.Audio;

public class PuzzleHandler : MonoBehaviour
{
	public PuzzleState currentState;
	public ActorManager actorManager;
	public CollisionChecker collisionChecker;
	public PuzzleGrid grid;
	public TileHitTester hitTester;
	public TileHighlighter tileHighlighter;
	public bool isPaused = false;
	public TutorialScreen tutorialMenu;

	[Header("Speed Values")]
	public float moveSpeed;
	public float rewindSpeed;
	public float boostSpeed;

	[Header("Move Values")]
	public bool moving;
	public float movePercent;
	public int movesTaken;
	
	bool boosting;
	bool rewinding;
	bool collided;

	bool _midMoveComplete = false;

	Vector2 targetPosition;
	
	void Update ()
	{
		IncrementMovePercent();
		CheckForMidMove();
		CheckForCollisions();		
		CheckForMoveComplete();
		UpdateActors();
		KeyboardControls();
		SetMusicState();
		_HandleEscapeKey();
	}

	void _HandleEscapeKey()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
			TogglePause(true);
	}

	void SetMusicState()
	{
		MusicManager.speedingUp = boosting;
	}

	void CheckForMidMove()
	{
		if(_midMoveComplete)
			return;

		if(movePercent < 0.5f)
			return;

		if(currentState == PuzzleState.Solved)
			return;

		_midMoveComplete = true;
		AudioManager.PlaySound(SoundEffect.Move, 0.3f);
	}

	void IncrementMovePercent()
	{	
		if(collided)
			return;

		if(isPaused)
			return;

		var canBoost = FarmerInBarn() && currentState == PuzzleState.Unsolved;

		bool boostPressed = Input.GetMouseButtonDown(0) || Input.GetKey(KeyCode.X);
		boosting = boostPressed && canBoost;

		if(moving && rewinding)
			movePercent += Time.deltaTime * rewindSpeed;
		else if(moving && boosting)
			movePercent += Time.deltaTime * boostSpeed;
		else if(moving)
			movePercent += Time.deltaTime * moveSpeed;

		movePercent = Mathf.Clamp(movePercent, 0, 1);
	}

	void CheckForMoveComplete()
	{
		if(collided)
			return;

		if(movePercent == 1)
		{	
			foreach(var actor in actorManager.actorList)
			{
				actor.SetMovePercent(movePercent);
				actor.CalculateIfActive();
			}

			CheckForCollisions();
			if(collided)
				return;

			if(!rewinding)
			{	
				if(currentState != PuzzleState.Solved)
					AudioManager.PlaySound(SoundEffect.Move, 0.3f);			
				grid.ToggleCowsOnTiles(actorManager.cowList, true);	// This MUST go before the actors CompleteMovement
			}
			var wasRewinding = rewinding;

			actorManager.farmer.CompleteMovement();
			moving = rewinding = false;
			movePercent = 0;
			foreach(var actor in actorManager.actorList)
			{
				if(actor.GetType() == typeof(Tractor))
					actor.CompleteMovement();
				if(actor.GetType() == typeof(Sheep))
					actor.CompleteMovement();
			}

			CheckForCollisions();

			if(collided)
				return;

			if(wasRewinding && targetPosition == grid.endBarnPos)
			{
				Rewind();
				return;
			}

			// Stop the farmer overshooting
			if(FarmerInBarn() && targetPosition != grid.endBarnPos)
				targetPosition = grid.endBarnPos;

			if(ShouldMoveAgain())
			{
				MoveToTile(targetPosition);
				IncrementMovePercent();
			}
			else
			{
				CheckForLose();
				if(currentState == PuzzleState.Unsolved)
					tileHighlighter.MoveHighlights(actorManager.farmer.targetPosition);
			}
			CheckForWin();
		}
	}

	void CheckForWin()
	{
		if(currentState == PuzzleState.Solved)
			return;

		var aCowNotFinished = false;

		foreach(var cow in actorManager.cowList)
		{
			if(!FPoint.isEqual(cow.currentPosition, grid.endBarnPos))
			{
				aCowNotFinished = true;
				break;
			}
		}

		var allCowsFinished = !aCowNotFinished;

		if(allCowsFinished)
		{
			currentState = PuzzleState.Solved;
			PlayerTracker.CompleteLevel();
		}
	}

	void CheckForLose()
	{
		if(FarmerInBarn())
		{
			if(!grid.AllGrassEaten())
				currentState = PuzzleState.MissedSomeGrass;
		}
		else if(movesTaken >= grid.moveLimit && TutorialManager.timeLimit)
		{
			currentState = PuzzleState.OutOfMoves;
		}
	}

	bool ShouldMoveAgain()
	{	
		if(FarmerInBarn())
		{
			if(grid.AllGrassEaten())
				return true;
			return false;
		}

		if(movesTaken >= grid.moveLimit && TutorialManager.timeLimit)
			return false;

		if(actorManager.farmer.currentPosition != targetPosition)
			return true;
			
		return false;
	}

	void CheckForCollisions()
	{
		if(!moving)
			return;

		if(rewinding)
			return;

		var collidedActors = collisionChecker.CheckAllCollisions();
		if(collidedActors != null)
		{	
			moving = false;
			collided = true;
			currentState = collisionChecker.GetCollisionState(collidedActors);
		}
	}

	void UpdateActors()
	{	
		foreach(var actor in actorManager.actorList)
			actor.rewinding = rewinding;

		if(!moving)
			return;

		foreach(var actor in actorManager.actorList)
			actor.SetMovePercent(movePercent);
	}

	public void Rewind()
	{
		if(moving)
			return;

		if(movesTaken < 1)
			return;

		if(!TutorialManager.canRewind)
			return;

		AudioManager.PlaySound(SoundEffect.Rewind);

		collided = false;
		movePercent = 0;
		
		moving = rewinding = true;
		actorManager.farmer.Rewind();
		targetPosition = actorManager.farmer.targetPosition;
		currentState = PuzzleState.Unsolved;

		if(!FPoint.isEqual(actorManager.farmer.targetPosition, grid.endBarnPos))
		{
			movesTaken --;
			PlayerTracker.RewindOne();		
		}

		foreach(var tile in grid.dynamicTiles)
			tile.Rewind();
		
		foreach(var actor in actorManager.actorList)
		{
			if(actor.GetType() == typeof(Tractor))
				actor.Rewind();
			
			if(actor.GetType() == typeof(Sheep))
			{
				if(((Sheep)actor).IsLeader())
					actor.Rewind();
			}
		}
	}

	public void KeyboardControls()
	{
		if(isPaused)
			return;

		if(currentState != PuzzleState.Unsolved)
			return;

		if(moving || collided)
			return;

		if (tutorialMenu.onScreen)
			return;

		if(Input.GetAxis("Horizontal") > 0.1f)
			MoveToTile(actorManager.farmer.targetPosition + Vector2.right);
		else if(Input.GetAxis("Horizontal") < -0.1f)
			MoveToTile(actorManager.farmer.targetPosition + Vector2.left);
		else if(Input.GetAxis("Vertical") > 0.1f)
			MoveToTile(actorManager.farmer.targetPosition + Vector2.up);
		else if(Input.GetAxis("Vertical") < -0.1f)
			MoveToTile(actorManager.farmer.targetPosition + Vector2.down);
		else if(Input.GetAxis("Back") > 0.1f)
			Rewind();
	}

	public void MoveToTile(Vector2 pos)
	{
		if(moving || collided)
			return;

		if(currentState != PuzzleState.Unsolved && currentState != PuzzleState.Solved)
			return;

		if(pos == actorManager.farmer.currentPosition && pos != grid.endBarnPos)
			return;

		if(!hitTester.LocationIsValid(pos))
			return;

		targetPosition = pos;

		var direction = pos - actorManager.farmer.currentPosition;
		if(direction.magnitude > 0.1f)
		{
			movesTaken ++;
			PlayerTracker.MoveOne();
			direction.Normalize();
		}

		foreach(var tile in grid.dynamicTiles)
			tile.UpdateStateHistory();

		_midMoveComplete = false;
		actorManager.farmer.SetTargetPosition(actorManager.farmer.currentPosition + direction);
		grid.ToggleCowsOnTiles(actorManager.cowList, false);	// This MUST go after the actors SetTargetPosition		
		moving = true;

		foreach(var actor in actorManager.actorList)
			actor.AdvanceOne();
	}

	public bool FarmerInBarn()
	{
		return actorManager.farmer.currentPosition == grid.endBarnPos;
	}

	public void TogglePause(bool yesno)
	{
		isPaused = yesno;
		if(yesno)
			AudioManager.PlaySound(SoundEffect.MenuOpen);
		else
			AudioManager.PlaySound(SoundEffect.MenuClose);
	}
}
