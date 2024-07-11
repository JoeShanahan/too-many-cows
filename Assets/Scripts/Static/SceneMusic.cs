using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMusic : MonoBehaviour
{
	static SceneMusic _instance;
	public AudioClip music;

	// Use this for initialization
	void Start ()
	{
		_instance = this;
	}
	
	public static AudioClip GetAudioClip()
	{
		if(_instance)
			return _instance.music;
		return null;
	}
}
