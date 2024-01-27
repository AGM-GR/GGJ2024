using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeethManager : MonoBehaviour
{

    public bool isDropingTeeth = false;
    public GameObject prefabTeeth;

    public float throwForce = 12.0f;

    public float heightThrow = 10f;

    public float minThrow = 1f;
    public float maxThrow = 10f;

    List<TeethType> teethInventory = new List<TeethType>();


    void Start()
    {
        AddTeeth(GetRandomTeeth());
        AddTeeth(GetRandomTeeth());
        AddTeeth(GetRandomTeeth());
           AddTeeth(GetRandomTeeth());
        AddTeeth(GetRandomTeeth());
        AddTeeth(GetRandomTeeth());
           AddTeeth(GetRandomTeeth());
        AddTeeth(GetRandomTeeth());
        AddTeeth(GetRandomTeeth());
           AddTeeth(GetRandomTeeth());
        AddTeeth(GetRandomTeeth());
        AddTeeth(GetRandomTeeth());
        
    }
    void Update()
    {
        if (isDropingTeeth)
        {
            isDropingTeeth = false;
            DropTeeth();
        }

    }


    public TeethType GetRandomTeeth()
    {
        return (TeethType)System.Enum.GetValues(typeof(TeethType)).GetValue(Random.Range(0, System.Enum.GetValues(typeof(TeethType)).Length - 1));
    }

    public void AddTeeth(TeethType teeth)
    {
        //puedes coger dientes iguales?
        teethInventory.Add(teeth);
    }

    public TeethType? RemoveTeeth()
    {
        if (teethInventory.Count > 0)
        {
            TeethType toDrop = teethInventory[Random.Range(0, teethInventory.Count - 1)];
            teethInventory.Remove(toDrop);
            return toDrop;
        }
        return null;
    }

    // Update is called once per frame


    public void DropTeeth()
    {
        TeethType? teeth = RemoveTeeth();
        if (teeth != null)
        {
            GameObject nTeeth = Instantiate(prefabTeeth);
            nTeeth.GetComponent<TeethObject>().teethType = teeth.Value;

            Vector3 nPos = transform.position;
            nPos.y += heightThrow;
            nTeeth.transform.position = nPos;

            ThrowObject(nTeeth.GetComponent<Rigidbody>());

        }
        else
        {
            //te dejaron sin dientes
        }
    }


    public void ThrowObject(Rigidbody rigidbody)
    {
        Vector3 direction = Vector3.forward;
        direction = Quaternion.Euler(new Vector3(0, Random.Range(0, 359), 0)) * direction * Random.Range(minThrow, maxThrow);

        // Reset forces
        rigidbody.velocity = Vector3.zero;

        // Add impulse
        rigidbody.AddForce(direction.normalized * throwForce, ForceMode.Impulse); // To exclude the mass -> ForceMode.VelocityChange
    }
}
