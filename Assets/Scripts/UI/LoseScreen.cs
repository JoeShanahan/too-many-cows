using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TooManyCows.Audio;

namespace TooManyCows.UI.Game
{
	public class LoseScreen : HideableMenuBase
	{	
		public SceneLoader loader;
		public Text[] uiText;

		protected override void KeyboardInput()
		{
			if(_activePercent < 1)
				return;
				
			if(Input.GetAxis("Submit") > 0.1f)
				loader.LoadScene("GameScene");
		}

		public override void DecideIfShouldBeActive()
		{
			if(puzzle.currentState == PuzzleState.Unsolved)
				SetMenuActive(false);

			if(active)
				return;

			if(puzzle.currentState == PuzzleState.PlayerHitCow)
				SetText(Strings.PlayerHitCowTitle, Strings.PlayerHitCow);
			
			else if(puzzle.currentState == PuzzleState.PlayerHitTractor)
				SetText(Strings.PlayerHitTractorTitle, Strings.PlayerHitTractor);
			
			else if(puzzle.currentState == PuzzleState.CowHitTractor)
				SetText(Strings.TractorHitCowTitle, Strings.TractorHitCow);
			
			else if(puzzle.currentState == PuzzleState.CowHitSheep)
				SetText(Strings.CowHitSheepTitle, Strings.CowHitSheep);

			else if(puzzle.currentState == PuzzleState.PlayerHitSheep)
				SetText(Strings.PlayerHitSheepTitle, Strings.PlayerHitSheep);

			else if(puzzle.currentState == PuzzleState.OutOfMoves)
				SetText(Strings.OutOfMovesTitle, Strings.OutOfMoves);

			else if(puzzle.currentState == PuzzleState.MissedSomeGrass)
				SetText(Strings.MissedGrassTitle, Strings.MissedGrass);
		}

		void SetText(string titleText, string descText)
		{
			AudioManager.PlaySound(SoundEffect.LoseJingle);
			SetMenuActive(true);
			uiText[0].text = titleText;
			uiText[1].text = descText;
		}
	}
}