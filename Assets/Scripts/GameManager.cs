using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    public InitialPoint initialPoint;
    public HeavyBall ball;
    public AudioManager audioManager;

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
    }

    public void GameWon()
    {
        if (gameOver) return;

        gameOver = true;
        gameWon = true;

        Debug.Log("¡¡¡GANASTE!!!");

        Time.timeScale = 0f;

        if (audioManager != null)
        {
            audioManager.PlayVictoryMusic();
        }

        StartCoroutine(ResetAfterDelay());
    }

    public void GameLost()
    {
        if (gameOver) return;

        gameOver = true;

        Debug.Log("PERDISTE");

        Time.timeScale = 0f;

        if (audioManager != null)
        {
            audioManager.PlayDefeatMusic();
        }

        StartCoroutine(ResetAfterDelay());
    }

    private IEnumerator ResetAfterDelay()
    {
        // IMPORTANTE: usar unscaled time porque timeScale = 0
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
        {
            audioManager.ResumeMusic();
        }

        Debug.Log("Juego reiniciado");
    }

    public bool IsGameOver() => gameOver;
    public bool IsGameWon() => gameWon;
}