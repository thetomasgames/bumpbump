using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{

	public GameObject heartPrefab;
	public GameObject buraco;
	private Transform buracos;
	public int x = 2;
	public int y = 1;
	public float spacing = 3f;
	public Transform hearts;
	private int lives;
	private bool gameOver = false;
	public Canvas gameOverCanvas;
	List<BuracoScript> buracosScripts;

	public AudioSource shotSource;
	public AudioSource hurtSource;

	public AudioClip hurtClip;
	public AudioClip deathClip;


	private int score = 0;
	public Text scoreText;

	void Start ()
	{
		buracosScripts = new List<BuracoScript> ();
		if (buracos == null) {
			buracos = new GameObject ().transform;
		} else {
			for (int i = 0; i < buracos.childCount; i++) {
				GameObject.Destroy (buracos.GetChild (i));
			}
		}
		for (int i = -x; i <= x; i++) {
			for (int j = -y; j <= y; j++) {
				criaBuraco (spacing * new Vector3 (i, j, 0) + new Vector3 (Random.value, Random.value, 0));
			}
		}
		for (float i = 0; i < 2 * Mathf.PI; i += 0.5f) {
//			criaBuraco (spacing * new Vector3 (Mathf.Sin (i), Mathf.Cos (i), 0));

		}
		Init ();
	}

	void Init ()
	{
		this.gameOver = false;
		gameOverCanvas.enabled = false;
		lives = 3;
		score = 0;
		updateUI ();
		buracosScripts.ForEach (b => b.Init ());
	}

	private void criaBuraco (Vector3 position)
	{
		GameObject go = GameObject.Instantiate (buraco, position, Quaternion.identity, buracos.transform);
		buracosScripts.Add (go.GetComponent<BuracoScript> ());
	}

	public void AddScore (int score)
	{
		this.score += score;
		updateUI ();
	}

	public void RemoveLive ()
	{
		if (!gameOver) {
			this.lives--;
			updateUI ();
			if (lives == 0) {
				playDeathSound ();
				initGameOver ();
			} else {
				playHurtSound ();
			}
		}
	}

	private void initGameOver ()
	{

		buracosScripts.ForEach (b => b.Reset ());
		gameOverCanvas.enabled = true;	
		this.gameOver = true;
	}

	void updateUI ()
	{
		scoreText.text = "Score " + this.score;
		int heartsOffset = hearts.childCount - lives;
		if (heartsOffset > 0) {
			for (int i = 0; i < heartsOffset; i++) {
				GameObject.Destroy (hearts.GetChild (0).gameObject);
			}
		} else if (heartsOffset < 0) {
			for (int i = 0; i < -heartsOffset; i++) {
				GameObject.Instantiate (heartPrefab, hearts);
			}
		}
	}

	public void PlayShootSound ()
	{
		shotSource.Play ();
	}

	private void playHurtSound ()
	{
		hurtSource.clip = hurtClip;
		hurtSource.Play ();
	}

	private void playDeathSound ()
	{
		hurtSource.clip = deathClip;
		hurtSource.Play ();
	}

	public void Restart ()
	{
		Init ();
	}

	public void Back ()
	{
		SceneManager.LoadScene (0);
	}


}
