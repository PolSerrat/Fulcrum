using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	[Header("References")]
	public AudioManager audioManager;

	[Header("End-of-game UI")]
	[Tooltip("Object shown for the whole end screen when the player WINS")]
	public GameObject winText;

	[Tooltip("Object shown for the whole end screen when the player LOSES")]
	public GameObject defeatText;

	[Header("End screen settings")]
	[Tooltip("How many seconds the win/defeat screen stays frozen on screen before the Menu loads.")]
	public float endScreenDuration = 10f;

	[Tooltip("Name of the scene to load after the end screen.")]
	public string menuSceneName = "Menu";

	private bool gameOver = false;
	private bool gameWon = false;

	void Start()
	{
		// Make sure both messages start hidden, no matter how they were left in the editor.
		if (winText != null) winText.SetActive(false);
		if (defeatText != null) defeatText.SetActive(false);
	}

	public void loseLife()
	{
		if (gameOver) return;

		Debug.Log("¡Has perdido una vida!");

		if (audioManager != null)
		{
			audioManager.PlayLoseLifeSound();
		}
	}

	public void GameWon()
	{
		if (gameOver) return;

		gameOver = true;
		gameWon = true;

		Debug.Log("¡¡¡GANASTE!!!");

		// Show the WIN message, then freeze the game.
		if (winText != null) winText.SetActive(true);
		Time.timeScale = 0f;

		if (audioManager != null)
		{
			audioManager.PlayVictoryMusic();
		}

		StartCoroutine(EndScreenThenMenu());
	}

	public void GameLost()
	{
		if (gameOver) return;

		gameOver = true;

		Debug.Log("PERDISTE");

		// Show the DEFEAT message, then freeze the game.
		if (defeatText != null) defeatText.SetActive(true);
		Time.timeScale = 0f;

		if (audioManager != null)
		{
			audioManager.PlayDefeatMusic();
		}

		StartCoroutine(EndScreenThenMenu());
	}

	// Shared by win AND lose: hold the frozen message on screen, then go to the Menu.
	private IEnumerator EndScreenThenMenu()
	{
		// Realtime wait: it keeps counting even though Time.timeScale is 0 (game frozen).
		yield return new WaitForSecondsRealtime(endScreenDuration);

		Time.timeScale = 1f;

		SceneManager.LoadScene(menuSceneName);
	}

	public bool IsGameOver() => gameOver;
	public bool IsGameWon() => gameWon;
}