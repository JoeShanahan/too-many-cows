using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorManager : MonoBehaviour
{
	public Farmer farmer;
	public List<Actor> actorList = new List<Actor>();
	public List<Cow> cowList = new List<Cow>();
	public List<Tractor> tractorList = new List<Tractor>();


	public void AddActor(Actor actor)
	{
		actorList.Add(actor);

		var actorType = actor.GetType();

		if(actorType == typeof(Farmer))
			farmer = (Farmer)actor;

		else if(actorType == typeof(Cow))
			cowList.Add((Cow)actor);
		
		else if(actorType == typeof(Tractor))
			tractorList.Add((Tractor)actor);

		actor.transform.SetParent(transform);
	}

	void Update ()
	{
		for(int i=0; i<actorList.Count; i++)
			actorList[i].DoUpdate();		
	}
}
