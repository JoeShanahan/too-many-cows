using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TooManyCows.Audio;

namespace TooManyCows.UI.Game
{
	public class WinScreen : HideableMenuBase
	{
		public SceneLoader loader;
		public Text[] uiText;

		protected override void KeyboardInput()
		{
			if(_activePercent < 1)
				return;

			if(Input.GetAxis("Submit") > 0.1f)
				BtnPressAccept();
		}

		public void BtnPressAccept()
		{
			// AudioManager.PlaySound(SoundEffect.BtnPress);
			loader.LoadScene("MapScene");
		}
		
		public override void DecideIfShouldBeActive()
		{
			if(active)
				return;

			if(puzzle.currentState == PuzzleState.Solved)
			{
				SaveData.SetCurrentLevelComplete();
				SetText(Strings.VictoryTitle, Strings.VictoryDesc);
				AudioManager.PlaySound(SoundEffect.WinJingle);
			}
		}

		void SetText(string titleText, string descText)
		{
			SetMenuActive(true);
			uiText[0].text = titleText;
			uiText[1].text = descText;
		}
	}
}