using UnityEngine;

// FoV tutorial: https://youtu.be/TfhPBAe9Tt8?t=612

// TODO: Take into account the sides of the target instead of the center point (transform).

[RequireComponent(typeof(Light))]
public class ThrowFoV : MonoBehaviour
{
    [SerializeField] float _ViewDistance = 10f;
    [SerializeField] string _TagetTag = "Player";

    Light _SpotLight;
    float _ViewAngle;

    Transform[] _Targets;

    private void Awake()
    {
        _SpotLight = GetComponent<Light>();

        _SpotLight.type = LightType.Spot;
    }

    void Start()
    {
        GameObject[] go = GameObject.FindGameObjectsWithTag(_TagetTag);

        if (go.Length == 0) 
        {
            Debug.LogError($"There are no objects tagged with <b>{_TagetTag}</b> in the scene");
        }

        _Targets = new Transform[go.Length];

        for (int i = 0; i < go.Length; i++) 
        { 
            _Targets[i] = go[i].transform;
        }

        // Use the Spotlight as vision cone
        _ViewAngle = _SpotLight.spotAngle;
    }


    void Update()
    {
        if (GetTargetInFoV() != null)
        {
            _SpotLight.color = Color.red;
        }
        else
        {
            _SpotLight.color = Color.yellow;
        }
    }

    public Transform GetTargetInFoV()
    {
        foreach (Transform target in _Targets) 
        {
            // Check the distance with the player
            if (Vector3.Distance(transform.position, target.position) < _ViewDistance)
            {
                // Get the vector direction to the player and normalize it
                Vector3 directionToTarget = target.position - transform.position;
                directionToTarget = directionToTarget.normalized;

                /* 
                 * Get the angle between the vector that points the player from our field of view center 
                 * and the vector that points from our field of view center to the local foward 
                 */

                // Project vectors onto the XZ plane by setting the Y component to 0
                Vector3 directionForwardXZ = new Vector3(transform.forward.x, 0, transform.forward.z);
                Vector3 directionTargetXZ = new Vector3(directionToTarget.x, 0, directionToTarget.z);

                float angleBetween = Vector3.Angle(directionForwardXZ, directionTargetXZ);

                // Check if the player is within the FoV angle
                if (angleBetween < _ViewAngle / 2f)
                {
                    return target; // We see the player    
                }
            }
        }

        return null; // We can't see the player
    }


    private void OnDrawGizmos()
    {
        //Draw red line for visualize the View Distance
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * _ViewDistance);
    }
}
