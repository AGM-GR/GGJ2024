using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAndRespawn : MonoBehaviour
{
    [SerializeField] float _Lifetime = 3f; 

    IEnumerator Start()
    {
        yield return new WaitForSeconds(_Lifetime);
        Destroy(gameObject);
    }
}
