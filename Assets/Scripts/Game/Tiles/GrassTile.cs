using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TooManyCows.Audio;

public class GrassTile : DynamicTile
{
	public GameObject particlePrefab;
	GameObject greenObj;
	GameObject yellowObj;

	// Use this for initialization
	void Start ()
	{
		greenObj = transform.GetChild(0).gameObject;
		yellowObj = transform.GetChild(1).gameObject;
	}
	
	public override void UpdateVisualState()
	{
		greenObj.SetActive(cowsEntered < totalCows);
		yellowObj.SetActive(cowsEntered >= totalCows);
	}

	protected override void EnterEffect()
	{
		if(greenObj.activeSelf)
		{
			var newParticles = Instantiate(particlePrefab);
			newParticles.transform.position = transform.position + Vector3.back;
			newParticles.transform.SetParent(transform);
			Destroy(newParticles, 2);
			AudioManager.PlaySoundRandomVolumeAndPitch(SoundEffect.EatGrass, 0.6f, 1.0f, 0.85f, 1.15f);
		}
	}
}
