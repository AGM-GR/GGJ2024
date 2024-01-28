using System.Collections;
using UnityEngine;

public class Tooth : MonoBehaviour
{
    public TeethType TeethType = TeethType.Normal;

    public Collider colliderTrigger;
    public Rigidbody Rigidbody;

    public float timeDisabled = 1f;

    public TeethSpawner Spawner;

    void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.gameObject.GetComponent<TeethManager>().AddTooth(TeethType);
            gameObject.SetActive(false);
            if (Spawner != null)
            {
                Spawner.ItemDisabled();
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
        else if (other.CompareTag("Ground"))
        {
            SetAsStatic();
        }
    }

    public int GetRandomTeeth()
    {
        return Random.Range(0, System.Enum.GetValues(typeof(TeethType)).Length - 1);
    }


    public void SetAsPhysical()
    {
        Rigidbody = GetComponent<Rigidbody>();
        //Rigidbody.useGravity = true;

        StartCoroutine(TriggerCooldown());
    }

    public void SetAsStatic()
    {
        var groundedPosition = transform.position;
        groundedPosition.y = 0;
        transform.position = groundedPosition;

        Rigidbody.angularVelocity = Vector3.zero;
        Rigidbody.velocity = Vector3.zero;
        Rigidbody.useGravity = false;
    }



    IEnumerator TriggerCooldown()
    {
        colliderTrigger.enabled = false;
        yield return new WaitForSeconds(timeDisabled);
        colliderTrigger.enabled = true;
        yield return null;
    }



}
