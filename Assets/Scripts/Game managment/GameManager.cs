using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	[Header("References")]
	public InitialPoint initialPoint;
	public HeavyBall ball;
	public AudioManager audioManager;

	[Header("UI")]                             
	public GameOverUI gameOverUI;               
	public GameTimer gameTimer;                 

	private bool gameOver = false;
	private bool gameWon = false;
	private Quaternion initialBallRotation;

	void Start()
	{
		if (ball == null || initialPoint == null)
		{
			Debug.LogError("GameManager: Ball o InitialPoint no están asignados");
			return;
		}

		initialBallRotation = ball.transform.rotation;

		if (gameOverUI == null)
			gameOverUI = FindFirstObjectByType<GameOverUI>();
		if (gameTimer == null)
			gameTimer = FindFirstObjectByType<GameTimer>();
	}

	public void loseLife()
	{
		if (gameOver) return;
		Debug.Log("¡Has perdido una vida!");
		if (audioManager != null)
			audioManager.PlayLoseLifeSound();
	}

	public void GameWon()
	{
		if (gameOver) return;

		gameOver = true;
		gameWon = true;
		Debug.Log("¡¡¡GANASTE!!!");
		Time.timeScale = 0f;

		if (gameTimer != null)                        
			gameTimer.StopTimer();                     

		if (audioManager != null)
			audioManager.PlayVictoryMusic();

		if (gameOverUI != null)                         
		{                                              
			float timeLeft = gameTimer != null          
				? gameTimer.TimeRemaining : 0f;        
			gameOverUI.ShowVictoryScreen(timeLeft);    
		}                                             
		else                                           
			StartCoroutine(ResetAfterDelay());          
	}

	public void GameLost(string reason = "lives")       
	{
		if (gameOver) return;

		gameOver = true;
		Debug.Log("PERDISTE — Razón: " + reason);
		Time.timeScale = 0f;

		if (audioManager != null)
			audioManager.PlayDefeatMusic();

		if (gameOverUI != null)                       
			gameOverUI.ShowDefeatScreen(reason);        
		else                                          
			StartCoroutine(ResetAfterDelay());          
	}

	// Keep old no-argument version so FinalPoint still works
	public void GameLost()
	{
		GameLost("lives");
	}

	private IEnumerator ResetAfterDelay()
	{
		yield return new WaitForSecondsRealtime(10f);
		ResetGame();
	}

	public void ResetGame()
	{
		gameOver = false;
		gameWon = false;
		Time.timeScale = 1f;

		ball.transform.position = initialPoint.GetSpawnPosition();
		ball.transform.rotation = initialBallRotation;

		Rigidbody rb = ball.GetComponent<Rigidbody>();
		if (rb != null)
		{
			rb.linearVelocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
		}

		if (audioManager != null)
			audioManager.ResumeMusic();

		SceneManager.LoadScene("Menu");
	}

	public bool IsGameOver() => gameOver;
	public bool IsGameWon() => gameWon;
}