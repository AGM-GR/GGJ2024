using System.Collections;
using UnityEngine;

public class TeethObject : MonoBehaviour
{
    public TeethType teethType;

    public Collider mCollider;
    public Collider colliderTrigger;

    public float timeDisabled = 1f;

    void Start()
    {
        colliderTrigger.enabled = false;
        StartCoroutine(EnableTrigger());
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.gameObject.GetComponent<TeethManager>().AddTeeth(teethType);
            Destroy(gameObject);
        }
    }

    public int GetRandomTeeth()
    {
        return Random.Range(0, System.Enum.GetValues(typeof(TeethType)).Length - 1);
    }



    IEnumerator EnableTrigger()
    {
        yield return new WaitForSeconds(timeDisabled);
        colliderTrigger.enabled = true;
        yield return null;
    }



}
