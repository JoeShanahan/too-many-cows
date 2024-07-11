using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TooManyCows.Audio
{
	public class AudioManager : MonoBehaviour
	{
		public MusicManager musicPlayer;
		public SfxManager sfxPlayer;

		static AudioManager _instance;

		List<SoundEffect> soundsPlayedThisFrame = new List<SoundEffect>();

		void Awake ()
		{
			if(_instance == null)
				_SetSelfSingleton();

			else if(_instance != this)
				Destroy(this.gameObject);
		}
		
		void _SetSelfSingleton()
		{
			_instance = this;
			DontDestroyOnLoad(this);
		}

		public static void PlaySound(SoundEffect effect, float volume=1)
		{
			if(_instance == null)
				return;

			_instance.sfxPlayer.PlaySound(effect, volume);
		}

		public static void PlaySoundRandomVolumeAndPitch(SoundEffect effect, float volMin, float volMax, float pitchMin, float pitchMax)
		{
			if(_instance == null)
				return;

			if(!Settings.sfxOn)
				return;

			if (_instance.soundsPlayedThisFrame.Contains(effect))
				return;

			_instance.soundsPlayedThisFrame.Add(effect);

			var volume = Random.Range(volMin, volMax);
			var pitch = Random.Range(pitchMin, pitchMax);

			_instance.sfxPlayer.PlaySound(effect, volume, pitch);
		}

		// Update is called once per frame
		void LateUpdate ()
		{
			if (soundsPlayedThisFrame.Count > 0)
				soundsPlayedThisFrame.Clear();
		}
	}
}