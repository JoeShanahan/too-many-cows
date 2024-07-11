using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TooManyCows.Audio
{
	public enum SoundEffect
	{
		TitleConfirm, MenuOpen, MenuClose, Move, WinJingle, LevelPressed, 
		EnterBarn, BtnPress, AcceptPressed, LoseJingle, SwitchToggle,
		TransitionOut, TransitionIn, EatGrass, Rewind, TutorialOpen, TutorialClose
	};

	public class SfxManager : MonoBehaviour
	{
		[Header("Title Scene")]
		public AudioClip titleConfirm;

		[Header("Game Scene")]
		public AudioClip playerMove;
		public AudioClip rewind;
		public AudioClip winJingle;
		public AudioClip loseJingle;
		public AudioClip enterBarn;
		public AudioClip eatGrass;

		[Header("Map Scene")]
		public AudioClip levelPressed;

		[Header("Menus")]
		public AudioClip menuOpen;
		public AudioClip menuClose;
		public AudioClip btnPress;
		public AudioClip switchToggle;
		public AudioClip accept;

		[Header("Transitioning")]
		public AudioClip shrinkNoise;
		public AudioClip growNoise;

		[Header("Tutorials")]
		public AudioClip tutorialOpen;
		public AudioClip tutorialClose;

		Dictionary<SoundEffect, AudioClip> _sfxDict;
		AudioSource _audioSrc;
		bool _initDone = false;

		// Use this for initialization
		void Start ()
		{
			if(_initDone)
				return;

			_sfxDict = new Dictionary<SoundEffect, AudioClip>()
			{
				{ SoundEffect.TitleConfirm, titleConfirm },

				{ SoundEffect.Move, playerMove },
				{ SoundEffect.Rewind, rewind },
				{ SoundEffect.WinJingle, winJingle },
				{ SoundEffect.LoseJingle, loseJingle },
				{ SoundEffect.EnterBarn, enterBarn },
				{ SoundEffect.EatGrass, eatGrass },

				{ SoundEffect.LevelPressed, levelPressed },

				{ SoundEffect.MenuOpen, menuOpen },
				{ SoundEffect.MenuClose, menuClose },
				{ SoundEffect.BtnPress, btnPress },
				{ SoundEffect.SwitchToggle, switchToggle },
				{ SoundEffect.AcceptPressed, accept },

				{ SoundEffect.TransitionIn, growNoise },
				{ SoundEffect.TransitionOut, shrinkNoise },

				{ SoundEffect.TutorialOpen, tutorialOpen },
				{ SoundEffect.TutorialClose, tutorialClose }
			};

			_audioSrc = GetComponent<AudioSource>();
			_initDone = true;
		}

		public void PlaySound(SoundEffect effect, float volume=1, float pitch=1)
		{	
			if(Settings.sfxOn == false)
				return;

			Start();

			if(pitch == 1)
				_audioSrc.PlayOneShot(_sfxDict[effect], volume);
			else
				_PlaySoundWithPitch(effect, volume, pitch);
			
		}

		void _PlaySoundWithPitch(SoundEffect effect, float volume=1, float pitch=1)
		{
			var obj = new GameObject();
			obj.transform.position = transform.position;

			var audio = obj.AddComponent<AudioSource>();
			audio.pitch = pitch;
			audio.PlayOneShot(_sfxDict[effect], volume);
			Destroy(obj, 5);
		}
	}
}