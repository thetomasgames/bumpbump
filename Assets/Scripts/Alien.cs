using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
	public GameObject explosao;
	private float startTime;
	private MovementState movementState = MovementState.IdleDown;
	public Transform highPos;
	public Transform downPos;
	private float verticalSpeed = .1f;

	public AudioSource audioSource;

	public void Reset ()
	{
		movementState = MovementState.IdleDown;
		this.transform.position = this.downPos.position;
		this.gameObject.SetActive (true);
	}

	public void IniciarMovimento ()
	{
		Reset ();
		audioSource.Play ();
		movementState = MovementState.Showing;
		startTime = Time.time;
	}

	void Update ()
	{
		if (movementState != MovementState.IdleDown && Time.time - startTime > 3.0f) {
			this.Hide ();
		}

	}

	public void Hide ()
	{
		movementState = MovementState.Hiding;
	}

	void FixedUpdate ()
	{
		switch (movementState) {
		case MovementState.Showing:
			if (this.transform.position.y < highPos.position.y) {
				this.transform.Translate (new Vector3 (0, verticalSpeed, 0));
			} else {
				this.transform.position = highPos.position;
				movementState = MovementState.IdleUp;
			}
			break;
		case MovementState.Hiding:
			if (this.transform.position.y > downPos.position.y) {
				this.transform.Translate (new Vector3 (0, -verticalSpeed, 0));
			} else {
				FixHidePos ();
				getParentScript ().alienHide ();
			}
			break;
		}
	}

	public void FixHidePos ()
	{
		this.transform.position = downPos.position;
		movementState = MovementState.IdleDown;
	}

	void OnMouseDown ()
	{
		Vector3 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		pos.z = 0;
		GameObject goExplosao = GameObject.Instantiate (explosao, pos, Quaternion.identity);
		GameObject.Destroy (goExplosao, 2f);
		getParentScript ().alienDestroyed (Time.time - startTime);
		this.gameObject.SetActive (false);
	}

	private BuracoScript getParentScript ()
	{
		return transform.parent.GetComponent<BuracoScript> ();
	}

	public enum MovementState
	{
		Showing,
		Hiding,
		IdleDown,
		IdleUp}

	;

}
