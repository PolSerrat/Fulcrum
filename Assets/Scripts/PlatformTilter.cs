using UnityEngine;

public class PlatformTilter : MonoBehaviour
{
    [Header("Player References")]
    public Transform player1;
    public Transform player2;

    [Header("Tilt Settings")]
    [Tooltip("How much the platform tilts per unit of distance from the center.")]
    public float tiltSensitivity = 5f;

    [Tooltip("The maximum angle the platform can tilt in any direction.")]
    public float maxTiltAngle = 20f;

    [Tooltip("How smoothly the platform tilts (higher is faster).")]
    public float tiltSpeed = 3f;

    // The starting position of the platform's pivot
    private Vector3 pivotPosition;

    void Start()
    {
        // Remember where the platform is in the world
        pivotPosition = transform.position;
    }

    void Update()
    {
        if (player1 == null || player2 == null) return;

        // 1. Find the midpoint between the two players
        Vector3 midPoint = (player1.position + player2.position) / 2f;

        // 2. Find the offset from the platform's center
        // We only care about X and Z for the tilt (assuming Y is up)
        float offsetX = midPoint.x - pivotPosition.x;
        float offsetZ = midPoint.z - pivotPosition.z;

        // 3. Calculate the target angles based on the offset
        // X offset affects the Z axis (Roll)
        // Z offset affects the X axis (Pitch)
        float targetPitch = Mathf.Clamp(offsetZ * tiltSensitivity, -maxTiltAngle, maxTiltAngle);
        float targetRoll = Mathf.Clamp(-offsetX * tiltSensitivity, -maxTiltAngle, maxTiltAngle);

        // Create the target rotation
        Quaternion targetRotation = Quaternion.Euler(targetPitch, 0f, targetRoll);

        // 4. Smoothly apply the rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * tiltSpeed);
    }
}