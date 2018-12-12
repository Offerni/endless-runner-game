﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour {
	[SerializeField] int jumpForceY = 13;
	[SerializeField] int doubleJumpForceY = 10;
	[SerializeField] int forceStartY = 16;
	[SerializeField] int forceStartX = 6;
	[SerializeField] int jumpCount = 0; // making sure that the char isn't have any remaining jumps on start

	public bool grounded = false;
	private Rigidbody2D myRigidbody;
	private BoxCollider2D myCollider;
	private AudioSource audioSource;
	private AudioSource audioSourceJump;
	private float audioVolume = 0.50f;
	private int xPush = -20;
	private int yPush = 15;
	private int speedPush = 35;


	/// <summary>
	/// listen the current eventsystem position, set it's position to mouse current position,
	/// populate the current position with raycasts and return true if is populated
	/// in other words, check if other UI element is touched
	/// </summary>
	/// <returns></returns>
	private bool IsPointerOverUIObject() {
		PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
		eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		List<RaycastResult> results = new List<RaycastResult>();		
		EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
		return results.Count > 0;
	}

	private void Start() {
		myRigidbody = GetComponent<Rigidbody2D>();
		myCollider = GetComponent<BoxCollider2D>();
		myRigidbody.velocity = new Vector2(forceStartX, forceStartY);
		audioSource = GetComponent<AudioSource>();
		audioSourceJump = audioSource;
		audioSourceJump.volume = audioVolume;
	}
	private void Update() {
		if (jumpCount > 1) {
			Jump(jumpForceY);
		} else if (jumpCount > 0 && jumpCount <=1) {
			Jump(doubleJumpForceY);
		}
	}

	/// <summary>
	/// This function check if the game isn't paused and the character is on the ground, if true: Jump.
	/// Doublejump on second click with a lower yPush (Finally!!)
	/// </summary>
	public void Jump(int forceY) {
		if (Input.GetMouseButtonDown(0) && !IsPointerOverUIObject() && !Mathf.Approximately(Time.timeScale, 0.0f)) {
			myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, forceY);
			audioSourceJump.Play();
			jumpCount--;
		} 
	}

	/// <summary>
	/// if impact on layer 9(ground), add rigidbody axis constraints;
	/// if impact on layer 10(car), reset the jump as well to able double jump;
	/// if impact on layer 12(impact area), remove all constraints and push the player to gameover trigger;
	/// </summary>
	/// <param name="collision"></param>
	private void OnCollisionEnter2D(Collision2D collision) {

		if (collision.gameObject.layer == 9) {
			jumpCount = 2;
			myRigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
			grounded = true;

		} if (collision.gameObject.layer == 10) {
			jumpCount = 2;
			grounded = true;

		} if (collision.gameObject.layer == 12) {
			myRigidbody.constraints = RigidbodyConstraints2D.None;
			myRigidbody.velocity = new Vector2(xPush, yPush) * speedPush * Time.deltaTime;
		}
	}

	/// <summary>
	/// Check if player left the ground and do other stuff ('Sorry, I'm tired')
	/// </summary>
	/// <param name="collision"></param>
	private void OnCollisionExit2D(Collision2D collision) {
		if (collision.gameObject.layer == 9 || collision.gameObject.layer == 10) {
			grounded = false;
		}
	}
}
