using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float rotationSpeed = 10f;
    private Vector3 randomDirection;

    void Start()
    {
        randomDirection = Random.insideUnitSphere;
    }

    void Update()
    {
        transform.Rotate(randomDirection * rotationSpeed * Time.deltaTime);
    }
}
