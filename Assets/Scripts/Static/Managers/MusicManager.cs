using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TooManyCows.Audio
{
	public class MusicManager : MonoBehaviour
	{
		public float mainMenuVol;
		
		public static bool speedingUp = false;
		public static bool menuUp = false;

		float _fadeMultiplier = 1f;

		AudioSource _audioSrc;

		// Use this for initialization
		void Start ()
		{
			_audioSrc = GetComponent<AudioSource>();
		}
		
		// Update is called once per frame
		void Update ()
		{
			HandleMusicSelection();
			HandlePitch();
			HandleVolume();	
		}

		void HandleMusicSelection()
		{
			if(_audioSrc.clip == SceneMusic.GetAudioClip())
				return;

			if(_audioSrc.clip != null)
			{
				_fadeMultiplier -= Time.deltaTime * 2;
				_fadeMultiplier = Mathf.Clamp(_fadeMultiplier, 0, 1);

				if(_fadeMultiplier == 0)
					_audioSrc.clip = null;
			}

			if(_audioSrc.clip == null)
			{
				_audioSrc.clip = SceneMusic.GetAudioClip();
				_fadeMultiplier = 1f;
				_audioSrc.Play();
			}
		}

		void HandlePitch()
		{
			var outPitch = 1f;

			if(speedingUp)
				outPitch *= 2f;

			var diff = outPitch - _audioSrc.pitch;

			if(diff < 0)
				outPitch = _audioSrc.pitch - Mathf.Min(-diff, Time.deltaTime*3);
			if(diff > 0)
				outPitch = _audioSrc.pitch + Mathf.Min(diff, Time.deltaTime*3);

			_audioSrc.pitch = outPitch;
		}

		void HandleVolume()
		{
			var outVol = 1f;
					
			if(menuUp)
				outVol *= mainMenuVol;

			outVol *= _fadeMultiplier;
		
			outVol = FPoint.Lerp(_audioSrc.volume, outVol, Time.deltaTime * 8);

			if(Settings.musicOn == false)
				outVol *= 0f;

			_audioSrc.volume = outVol;
		}
	}
}