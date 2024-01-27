using System.Collections;
using UnityEngine;

public class Tooth : MonoBehaviour
{
    public TeethType teethType;

    public Collider colliderTrigger;
    public Rigidbody Rigidbody;

    public float timeDisabled = 1f;

    public TeethSpawner Spawner;

    void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        colliderTrigger.enabled = false;
        StartCoroutine(EnableTrigger());
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.gameObject.GetComponent<TeethManager>().AddTooth(teethType);
            gameObject.SetActive(false);
            Spawner?.ItemDisabled();
        }
    }

    public int GetRandomTeeth()
    {
        return Random.Range(0, System.Enum.GetValues(typeof(TeethType)).Length - 1);
    }


    public void SetAsPhysical()
    {
        Rigidbody = GetComponent<Rigidbody>();

        Rigidbody.useGravity = true;
        Rigidbody.isKinematic = false;
    }



    IEnumerator EnableTrigger()
    {
        yield return new WaitForSeconds(timeDisabled);
        colliderTrigger.enabled = true;
        yield return null;
    }



}
