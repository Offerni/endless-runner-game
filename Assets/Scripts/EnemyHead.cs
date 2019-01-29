﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHead : MonoBehaviour {

	GameSession gameSession;
	[SerializeField] AudioClip deathSfx;
	[Range(0,1)][SerializeField] float deathSfxVolume = 0.5f;

    private PlayerController character;

	int killEnemyPoint = 50;

    private void Start() {
        character = FindObjectOfType<PlayerController>();
    }

    private void Update() {
        if (character.yVelocity >= 0) {
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        } else {
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    /// <summary>
    ///When the character hits the enemy's head from top to bottom, the enemy dies 
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision) {

		if (collision.collider.gameObject.layer == 8) {
			
			Destroy(transform.parent.gameObject);
			AudioSource.PlayClipAtPoint(deathSfx, Camera.main.transform.position, deathSfxVolume);
			gameSession = FindObjectOfType<GameSession>();
			gameSession.AddToScore(killEnemyPoint);
		}
	}
}
