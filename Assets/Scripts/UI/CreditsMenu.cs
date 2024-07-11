using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TooManyCows.UI
{
	public class CreditsMenu : MonoBehaviour
	{
		public CanvasGroup firstText;
		public CanvasGroup secondText;
		public float timer = 0.0f;
		MenuLerper menuLerper;

		// Use this for initialization
		void Start ()
		{
			menuLerper = GetComponent<MenuLerper>();	
		}
		
		// Update is called once per frame
		void Update ()
		{	
			if(menuLerper.positionMultiplier != 0)
				return;

			timer += Time.deltaTime;

			if(timer >= 10)
				timer -= 10;

			if(timer < 4)
			{
				firstText.alpha = 1;
				secondText.alpha = 0;
			}
			else if(timer < 4.5f)
			{
				var a = (timer - 4) * 2;
				firstText.alpha = 1 - a;
				secondText.alpha = 0;
			}
			else if(timer < 5)
			{
				var a = timer - 4.5f;
				firstText.alpha = 0;
				secondText.alpha = a*2;
			}
			else if(timer < 9)
			{
				firstText.alpha = 0;
				secondText.alpha = 1;
			}
			else if(timer < 9.5f)
			{
				var a = (timer - 9) * 2;
				firstText.alpha = 0;
				secondText.alpha = 1 - a;
			}
			else if(timer < 10)
			{
				var a = (timer - 9.5f) * 2;
				firstText.alpha = a;
				secondText.alpha = 0;
			}
		}
	}
}