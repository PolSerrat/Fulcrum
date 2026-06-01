using UnityEngine;
using TMPro; // Needed for the warning text
//a 
public class AFKManager : MonoBehaviour
{
    [Header("Tracking Setup")]
    public Transform player1;
    public Transform player2;
    public Health ballHealth; // Link to the ball's health script

    [Header("VR Jitter Filter")]
    [Tooltip("How far (in Unity meters) a player must move from their anchor to reset the AFK timer. 0.1 = 10cm.")]
    public float movementThreshold = 0.15f;

    [Header("Timers (Seconds)")]
    public float timeUntilWarning = 10f;
    public float gracePeriodAfterWarning = 3f;
    public float damageInterval = 1f;

    [Header("UI References")]
    [Tooltip("Drag the Warning Text UI object here")]
    public GameObject warningUI;

    // State tracking
    private Vector3 p1Anchor;
    private Vector3 p2Anchor;
    private float afkTimer = 0f;
    private float nextDamageTimer = 0f;

    void Start()
    {
        // Set the initial anchors where the players spawn
        if (player1 != null) p1Anchor = player1.position;
        if (player2 != null) p2Anchor = player2.position;

        // Ensure warning is hidden when the game starts
        if (warningUI != null) warningUI.SetActive(false);
    }

    void Update()
    {
        if (player1 == null || player2 == null || ballHealth == null) return;

        // 1. Check distance from the established anchors
        float dist1 = Vector3.Distance(player1.position, p1Anchor);
        float dist2 = Vector3.Distance(player2.position, p2Anchor);

        // 2. Did ANYONE move further than the threshold?
        if (dist1 > movementThreshold || dist2 > movementThreshold)
        {
            ResetAFK();
        }
        else
        {
            // Nobody moved enough, so increase the idle timer
            afkTimer += Time.deltaTime;

            // 3. Warning Phase (Timer has hit 10s, but hasn't reached 13s yet)
            if (afkTimer >= timeUntilWarning && afkTimer < (timeUntilWarning + gracePeriodAfterWarning))
            {
                if (warningUI != null) warningUI.SetActive(true);
            }
            // 4. Damage Phase (Timer has passed 13s)
            else if (afkTimer >= (timeUntilWarning + gracePeriodAfterWarning))
            {
                // Ensure warning stays on screen
                if (warningUI != null) warningUI.SetActive(true);

                // Countdown the 1-second damage tick
                nextDamageTimer -= Time.deltaTime;
                if (nextDamageTimer <= 0f)
                {
                    ApplyAFKDamage();
                    nextDamageTimer = damageInterval; // Reset tick timer so it hits again in 1 second
                }
            }
        }
    }

    void ResetAFK()
    {
        // Drop new anchors at their current positions
        p1Anchor = player1.position;
        p2Anchor = player2.position;

        // Reset all timers
        afkTimer = 0f;
        nextDamageTimer = damageInterval;

        // Hide warning
        if (warningUI != null) warningUI.SetActive(false);
    }

    void ApplyAFKDamage()
    {
        // Pass 0 force so it doesn't log a massive crash in your console, 
        // it just silently drops the health and updates the UI!
        ballHealth.LoseLife(0f);
    }
}