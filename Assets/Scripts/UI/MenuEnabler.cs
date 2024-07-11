using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TooManyCows.UI.Game
{
	public class MenuEnabler : MonoBehaviour
	{
		public HideableMenuBase[] hideableMenus;
		public TutorialScreen tutorialScreen;
		
		void Update ()
		{	
			foreach(var menu in hideableMenus)
				_HandleHideableMenu(menu);
				
			_HandleTutorialScreen();
		}

		void _HandleHideableMenu(HideableMenuBase menu)
		{
			menu.DecideIfShouldBeActive();
			var shouldBeActive = !menu.IsFullyHidden();
			
			if(menu.gameObject.activeSelf != shouldBeActive)
				menu.gameObject.SetActive(shouldBeActive);
		}

		void _HandleTutorialScreen()
		{
			tutorialScreen.DecideIfShouldBeActive();
			var shouldBeActive = !tutorialScreen.IsFullyHidden();

			if(tutorialScreen.gameObject.activeSelf != shouldBeActive)
				tutorialScreen.gameObject.SetActive(shouldBeActive);
		}
	}
}
