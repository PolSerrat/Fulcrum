using UnityEngine;
using UnityEngine.SceneManagement;

public class DualCylinderManager : MonoBehaviour
{
    [Header("Assign your two cylinders here")]
    public CylinderPad cylinderPad1;
    public CylinderPad cylinderPad2;

    [Header("Timer Settings")]
    public float requiredTime = 2.0f;

    [Header("Scene Transition")]
    [Tooltip("Leave this blank if the scene isn't built yet!")]
    public string nextSceneName;

    private float currentTimer = 0f;

    void Update()
    {
        if (cylinderPad1.hasPlayer && cylinderPad2.hasPlayer)
        {
            currentTimer += Time.deltaTime;

            if (currentTimer >= requiredTime)
            {
                // Reset the pads so the timer doesn't trigger multiple times
                cylinderPad1.hasPlayer = false;
                cylinderPad2.hasPlayer = false;

                // SAFETY CHECK: Did we leave the scene name blank for the future?
                if (string.IsNullOrEmpty(nextSceneName))
                {
                    Debug.LogWarning("Level locked! You haven't typed a scene name in the Inspector yet.");
                    currentTimer = 0f; // Reset timer so it doesn't spam the console
                    return; // Stop the code right here so it doesn't try to load
                }

                // If the name isn't blank, load the scene normally!
                SceneManager.LoadScene(nextSceneName);
            }
        }
        else
        {
            // Reset timer if someone steps off
            currentTimer = 0f;
        }
    }
}