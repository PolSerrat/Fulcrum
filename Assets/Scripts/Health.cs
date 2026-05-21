using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Health : MonoBehaviour
{
    [Header("Life Settings")]
    public int currentLives = 3;

    [Header("Crash Settings")]
    public float crashThreshold = 5.0f;

    [Header("Invincibility Settings")]
    [Tooltip("How many seconds of immunity after spawning or taking damage.")]
    public float invincibilityTime = 0.5f;

    // Tracks the exact moment in time the ball last took damage (or spawned)
    private float lastCrashTime;

    [Header("UI Settings")]
    public TextMeshProUGUI healthText;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            return;
        }

        // 1. Set the "last crash" to the exact moment the scene starts.
        // This automatically gives the ball 0.5 seconds of invincibility at spawn!
        lastCrashTime = Time.time;

        GameObject textObj = GameObject.Find("HealthText");

        if (textObj != null)
        {
            healthText = textObj.GetComponent<TextMeshProUGUI>();
            UpdateHealthUI();
        }
        else
        {
            Debug.LogError("I couldn't find the text! Make sure it is named exactly 'HealthText'.");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            return;
        }

        // 2. Check if we are still inside the invincibility grace period.
        // If the current game time is less than the last crash time + 0.5s, ignore the hit.
        if (Time.time < lastCrashTime + invincibilityTime)
        {
            return; // Stop running the code right here
        }

        float impactForce = collision.relativeVelocity.magnitude;

        if (impactForce > crashThreshold)
        {
            LoseLife(impactForce);
        }
    }

    void LoseLife(float force)
    {
        currentLives--;

        // 3. Record the exact time this crash happened to trigger the grace period
        lastCrashTime = Time.time;

        Debug.Log("CRASH! Impact force was: " + force + ". Lives left: " + currentLives);

        UpdateHealthUI();

        if (currentLives <= 0)
        {
            Debug.Log("Out of lives! Game Over.");
            SceneManager.LoadScene("Menu");
        }
    }

    void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "Lives: " + currentLives;
        }
    }
}