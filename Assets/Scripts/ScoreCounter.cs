using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour {

	public static int score = 0;

	public Text scoreDisplay;

	// Use this for initialization
	void Start () {
		if (scoreDisplay != null)
			StartCoroutine (UpdateScore());
	}

	IEnumerator UpdateScore () {
		while (true) {
			scoreDisplay.text = "score: " + score;

			yield return new WaitForSeconds (0.05f);
		}
	}

	void OnCollision() {
		score += 1;
	}
}
