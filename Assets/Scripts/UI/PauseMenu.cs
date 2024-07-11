using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TooManyCows.UI.Game
{
	public class PauseMenu : HideableMenuBase
	{
		public override void DecideIfShouldBeActive()
		{
			SetMenuActive(puzzle.isPaused);
		}
	}
}