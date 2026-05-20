using UnityEngine;
using UnityEngine.SceneManagement;

public class DualCylinderManager : MonoBehaviour
{
    [Header("Assign your two cylinders here")]
    public CylinderPad cylinderPad1;
    public CylinderPad cylinderPad2;

    [Header("Timer Settings")]
    [Tooltip("How many seconds both players must stand on the cylinders")]
    public float requiredTime = 2.0f;

    // This keeps track of the current time
    private float currentTimer = 0f;

    void Update()
    {
        // 1. Are BOTH players on the pads right now?
        if (cylinderPad1.hasPlayer && cylinderPad2.hasPlayer)
        {
            // Start counting up time
            currentTimer += Time.deltaTime;

            // 2. Have we reached the 2-second mark?
            if (currentTimer >= requiredTime)
            {
                // Reset the pads to prevent the code from running multiple times
                cylinderPad1.hasPlayer = false;
                cylinderPad2.hasPlayer = false;

                // Load the Tutorial scene
                SceneManager.LoadScene("Tutorial");
            }
        }
        else
        {
            // 3. If someone steps off a pad, reset the timer immediately
            currentTimer = 0f;
        }
    }
}