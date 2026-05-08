using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    public InitialPoint initialPoint;
    public HeavyBall ball;
    public AudioManager audioManager;

    [Header("Game State")]
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
        ResetGame();
    }

    public void ResetGame()
    {
        gameOver = false;
        gameWon = false;
        Time.timeScale = 1f; // Reanudar el juego si estaba pausado

        // Colocar la bola en el Initial Point
        ball.transform.position = initialPoint.GetSpawnPosition();
        ball.transform.rotation = initialBallRotation;

        // Resetear velocidad
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // Reanudar música
        if (audioManager != null)
        {
            audioManager.ResumeMusic();
        }

        Debug.Log("═══════════════════════════════════");
        Debug.Log("      JUEGO INICIADO");
        Debug.Log("═══════════════════════════════════");
    }

    public void GameWon()
    {
        if (gameOver) return;

        gameOver = true;
        gameWon = true;

        Debug.Log("╔════════════════════╗");
        Debug.Log("║   ¡¡¡GANASTE!!!   ║");
        Debug.Log("╚════════════════════╝");

        // Tocar música de victoria
        if (audioManager != null)
        {
            audioManager.PlayVictoryMusic();
        }

        // Pausar el juego
        Time.timeScale = 0f;
    }

    public bool IsGameOver() => gameOver;
    public bool IsGameWon() => gameWon;
}
