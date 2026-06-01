using UnityEngine;

public class CylinderPad : MonoBehaviour
{
    [Header("Pad State")]
    public bool hasPlayer = false;

    [Header("Visual Feedback")]
    [Tooltip("The glowing color it reaches right before teleporting")]
    [ColorUsage(true, true)]
    public Color chargeColor = Color.cyan;

    private MeshRenderer padRenderer;

    void Start()
    {
        padRenderer = GetComponent<MeshRenderer>();
    }

    // The Manager will call this function to update the glow smoothly
    public void UpdateGlow(float chargePercentage)
    {
        if (padRenderer != null)
        {
            // 1. Tell the material we want to use the Emission system
            padRenderer.material.EnableKeyword("_EMISSION");

            // 2. Blend from Black (Off) to the target Charge Color
            Color currentEmission = Color.Lerp(Color.black, chargeColor, chargePercentage);

            // 3. Apply the new color strictly to the Emission channel, leaving the base texture alone!
            padRenderer.material.SetColor("_EmissionColor", currentEmission);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // If the object rolling onto the pad has the HeavyBall script
        if (other.GetComponent<PlayerMovement>() != null)
        {
            hasPlayer = true; // Turn the switch ON
        }
    }

    // This is called automatically when an object leaves the trigger area
    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerMovement>() != null)
        {
            hasPlayer = false; // Turn the switch OFF
        }
    }

}