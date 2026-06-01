using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

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

    [Header("Visual Effects")]
    [Tooltip("The color the ball turns when it takes damage")]
    public Color damageFlashColor = Color.white;
    [Tooltip("How long the flash lasts in seconds")]
    public float flashDuration = 0.15f;

    private MeshRenderer ballRenderer;
    private Color originalColor;
    private Coroutine flashRoutine;

	[Header("Game Manager")]            
	public GameManager gameManager;

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

        ballRenderer = GetComponent<MeshRenderer>();

        if (ballRenderer != null)
        {
            // This is the magic part: It automatically saves the starting color!
            // If the ball is Blue in the Normal level, it remembers Blue.
            originalColor = ballRenderer.material.color;
        }

		if (gameManager == null) {
			gameManager = FindFirstObjectByType<GameManager>();
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

    public void LoseLife(float force)
    {
        currentLives--;

        // 3. Record the exact time this crash happened to trigger the grace period
        lastCrashTime = Time.time;

        Debug.Log("CRASH! Impact force was: " + force + ". Lives left: " + currentLives);

        UpdateHealthUI();

        if (currentLives <= 0)
        {
            Debug.Log("Out of lives! Game Over.");
			Debug.Log("Out of lives! Game Over.");

			if (gameManager != null)                           
				gameManager.GameLost("lives");              
			else                                             
				SceneManager.LoadScene("Menu");                // fallback

			return;
		}

        if (ballRenderer != null)
        {
            // If it's already flashing from a previous hit, stop it so we can restart the flash
            if (flashRoutine != null)
            {
                StopCoroutine(flashRoutine);
            }
            // Start the new flash timer
            flashRoutine = StartCoroutine(DamageFlash());
        }
    }

    void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "Lives: " + currentLives;
        }
    }

    private IEnumerator DamageFlash()
    {
        // 1. Instantly change to the bright flash color
        ballRenderer.material.color = damageFlashColor;

        // 2. Wait for a tiny fraction of a second
        yield return new WaitForSeconds(flashDuration);

        // 3. Revert exactly back to the saved color (Purple, Green, Blue, or Red)
        ballRenderer.material.color = originalColor;
    }
}