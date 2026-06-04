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

	[Header("Game Manager Reference")]
	[Tooltip("Optional - found automatically if left empty.")]
	public GameManager gameManager;

	private MeshRenderer ballRenderer;
	private Color originalColor;
	private Coroutine flashRoutine;

	void Start()
	{
		if (SceneManager.GetActiveScene().name == "Menu")
		{
			return;
		}

		// Find the GameManager so we can trigger the shared defeat screen on death.
		if (gameManager == null)
		{
			gameManager = FindFirstObjectByType<GameManager>();
		}

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
			// Automatically remember the ball's starting color so the flash can revert to it.
			originalColor = ballRenderer.material.color;
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		if (SceneManager.GetActiveScene().name == "Menu")
		{
			return;
		}

		// Still inside the invincibility grace period? Ignore the hit.
		if (Time.time < lastCrashTime + invincibilityTime)
		{
			return;
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

		// Record the exact time this crash happened to trigger the grace period.
		lastCrashTime = Time.time;

		Debug.Log("CRASH! Impact force was: " + force + ". Lives left: " + currentLives);

		UpdateHealthUI();

		if (currentLives <= 0)
		{
			Debug.Log("Out of lives! Game Over.");

			// Trigger the shared defeat screen (freeze + defeat text + defeat music, then Menu).
			if (gameManager != null)
			{
				gameManager.GameLost();
			}
			else
			{
				// Fallback if there is no GameManager in the scene.
				SceneManager.LoadScene("Menu");
			}

			// Stop here so we don't kick off a flash that would freeze half-finished.
			return;
		}

		if (ballRenderer != null)
		{
			// If it's already flashing from a previous hit, restart the flash.
			if (flashRoutine != null)
			{
				StopCoroutine(flashRoutine);
			}
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
		// Flash to the damage color, wait briefly, then revert to the original color.
		ballRenderer.material.color = damageFlashColor;
		yield return new WaitForSeconds(flashDuration);
		ballRenderer.material.color = originalColor;
	}
}