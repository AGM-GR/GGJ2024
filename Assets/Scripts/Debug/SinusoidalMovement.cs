using UnityEngine;

public class SinusoidalMovement : MonoBehaviour
{
    [Range(0f, 5f)]
    [SerializeField] float _DistanceX;

    [Range(0f, 5f)]
    [SerializeField] float _DistanceY;

    [Range(0f, 5f)]
    [SerializeField] float _DistanceZ;

    float _OriginalPosX;
    float _OriginalPosY;
    float _OriginalPosZ;

    private void Start()
    {
        _OriginalPosX = transform.position.x;
        _OriginalPosY = transform.position.y;
        _OriginalPosZ = transform.position.z;
    }

    void Update()
    {
        float offset = Mathf.Sin(Time.time);
        transform.position = new Vector3(_OriginalPosX + offset * _DistanceX, _OriginalPosY + offset * _DistanceY, _OriginalPosZ + offset * _DistanceZ);
    }
}
