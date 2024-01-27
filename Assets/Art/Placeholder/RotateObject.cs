using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float rotationSpeed = 10f;
    public int framesBeforeRotation = 10; // N�mero de frames antes de la rotaci�n
    private Vector3 randomDirection;
    private int frameCount = 0; // Contador de frames

    void Start()
    {
        randomDirection = Random.insideUnitSphere;
    }

    void Update()
    {
        frameCount++;
        if (frameCount >= framesBeforeRotation)
        {
            transform.Rotate(randomDirection * rotationSpeed * Time.deltaTime);
            frameCount = 0;
        }
    }
}
