using UnityEngine;

// This line ensures Unity automatically adds a Rigidbody if you forgot!
[RequireComponent(typeof(Rigidbody))]
public class PlatformTilter : MonoBehaviour
{
    [Header("Player References")]
    public Transform player1;
    public Transform player2;

    [Header("Tilt Settings")]
    public float tiltSensitivity = 5f;
    public float maxTiltAngle = 10f;
    public float tiltSpeed = 1.5f;

    private Vector3 pivotPosition;
    private Rigidbody rb;
    private Quaternion targetRotation;

    void Start()
    {
        pivotPosition = transform.position;

        // Grab the Rigidbody and ensure it is set up correctly
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    // Use Update for reading inputs and calculating math
    void Update()
    {
        if (player1 == null || player2 == null) return;

        Vector3 midPoint = (player1.position + player2.position) / 2f;

        float offsetX = midPoint.x - pivotPosition.x;
        float offsetZ = midPoint.z - pivotPosition.z;

        float targetPitch = Mathf.Clamp(offsetZ * tiltSensitivity, -maxTiltAngle, maxTiltAngle);
        float targetRoll = Mathf.Clamp(-offsetX * tiltSensitivity, -maxTiltAngle, maxTiltAngle);

        // Store the target rotation we want to reach
        targetRotation = Quaternion.Euler(targetPitch, 0f, targetRoll);
    }

    // Use FixedUpdate for applying physical movement
    void FixedUpdate()
    {
        // Calculate the next step toward our target rotation
        // Notice we use Time.fixedDeltaTime here instead of Time.deltaTime
        Quaternion nextRotation = Quaternion.Lerp(rb.rotation, targetRotation, Time.fixedDeltaTime * tiltSpeed);

        // Tell the physics engine to move the rotation, which pushes the ball properly!
        rb.MoveRotation(nextRotation);
    }
}