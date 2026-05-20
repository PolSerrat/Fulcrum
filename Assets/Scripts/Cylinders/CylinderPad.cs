using UnityEngine;

public class CylinderPad : MonoBehaviour
{
    // This boolean acts as a switch. It is public so our Manager can see it.
    public bool hasPlayer = false;

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