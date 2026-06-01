using UnityEngine;

public class FinalPoint : MonoBehaviour
{
    [Header("Game Manager Reference")]
    public GameManager gameManager;

    void OnTriggerEnter(Collider other)
    {
        // Si la bola toca el Final Point, notificar al GameManager
        if (other.GetComponent<HeavyBall>() != null)
        {
            if (gameManager != null)
            {
                gameManager.GameWon();
            }
            else
            {
                Debug.LogError("GameManager no está asignado en FinalPoint");
            }
        }
    }
}
