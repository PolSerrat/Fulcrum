using UnityEngine;

public class InitialPoint : MonoBehaviour
{
    [Header("Spawn Settings")]
    public float spawnHeight = 0.5f;

    void Start()
    {
        // Hacer invisible el cubo de Initial Point
        GetComponent<Renderer>().enabled = false;
    }

    // El GameManager obtendrá la posición de este script cuando sea necesario
    public Vector3 GetSpawnPosition()
    {
        return transform.position + Vector3.up * spawnHeight;
    }
}
