using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BuracoScript : MonoBehaviour
{

	public Alien alien;

	private Manager manager;
	private int minTime = 3;
	private int maxTime = 10;
	private float startAlienShowChance = .1f;
	private float maxAlienShowChance = .6f;

	private bool stopped = false;
	private double alienShowChance;
	System.Random random;

	void Awake ()
	{
		random = new System.Random (gameObject.GetInstanceID ());
		this.manager = GameObject.FindGameObjectWithTag ("Manager").GetComponent<Manager> ();
	}

	public void Init ()
	{
		stopped = false;
		InitAlienInRandomTime ();
		alienShowChance = startAlienShowChance;
	}

	public void Reset ()
	{
		stopped = true;
		alien.Reset ();
		StopAllCoroutines ();
	}

	public void alienDestroyed (float timeSpent)
	{
		this.manager.PlayShootSound ();
		if (timeSpent < 5) {
			this.manager.AddScore ((int)(10 / Mathf.Max (timeSpent, 0.2f)));
		}
		InitAlienInRandomTime ();
	}

	public void alienHide ()
	{
		this.manager.RemoveLive ();
		InitAlienInRandomTime ();
	}


	private void InitAlienInRandomTime ()
	{
		StopAllCoroutines ();
		if (stopped == false) {
			if (alienShowChance < maxAlienShowChance) {
				alienShowChance += 0.05f;
			}
			StartCoroutine (initAlienInSeconds (random.Next (minTime, maxTime)));
		}
	}


	IEnumerator initAlienInSeconds (float seconds)
	{
		yield return new WaitForSeconds (seconds);
		if (random.NextDouble () < alienShowChance) {
			this.alien.IniciarMovimento ();	
		} else {
			InitAlienInRandomTime ();
		}
		yield return null;
	}

}
