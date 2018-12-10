﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SceneController : MonoBehaviour {

	private int currentScene;
	private int numberOfScenes;
	private bool musicOn;
	private MusicPlayer musicPlayer;
	private bool gamePaused;
	private int collisionCount = 0;

	private void Awake() {
		
	}

	private void Start() {

		//fix the bug that was keeping the game paused after restart
		if(Mathf.Approximately(Time.timeScale, 0.0f)) {
			Time.timeScale = 1.0f;
		}
		numberOfScenes = SceneManager.sceneCountInBuildSettings;
		currentScene = SceneManager.GetActiveScene().buildIndex;
	}
	
	/// <summary>
	/// Check if the next scene index is less or equal to the total of scenes, if true, load next scene.
	/// </summary>
	public void LoadNextScene() {


		if (currentScene + 1 < numberOfScenes) {
			SceneManager.LoadScene(currentScene + 1);
		}
	}

	public void LoadScene(int sceneIndex) {
		SceneManager.LoadScene(sceneIndex);
	}

	public void RestartGame() {
		musicPlayer = FindObjectOfType<MusicPlayer>();
		musicPlayer.RestartMusic();
		currentScene = SceneManager.GetActiveScene().buildIndex;
		SceneManager.LoadScene(currentScene);

	}
	/// <summary>
	/// Compares if the timescale is equal to 0, if true: Unpause the Game and Music. And vice versa.
	/// 
	/// </summary>
	public void PauseGame() {
		musicPlayer = FindObjectOfType<MusicPlayer>();
		var backgroundMusic = musicPlayer.GetComponent<AudioSource>();

		if (Mathf.Approximately(Time.timeScale, 0.0f)) {
			Time.timeScale = 1.0f;
			backgroundMusic.Play();
		} else {
			Time.timeScale = 0.0f;
			backgroundMusic.Pause();
		}
	}

	private void GameOver() {
		LoadScene(numberOfScenes - 1);
	}

	public void ToggleMusicButton() {
		musicPlayer = FindObjectOfType<MusicPlayer>();
		musicPlayer.ToggleMusic();
	}

	public void Quit() {
		Application.Quit();
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		collisionCount++;
		if (collisionCount > 1) {
			GameOver();
		}
	}
}
