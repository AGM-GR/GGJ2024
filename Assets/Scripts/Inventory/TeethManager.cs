using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TeethManager : MonoBehaviour
{
    private Character _character;
    public Tooth toothPrefab;

    public float ThrowForce = 12.0f;
    public float heightThrow = 10f;
    public float minThrow = 1f;
    public float maxThrow = 10f;

    //List<TeethType> teethInventory = new List<TeethType>();

    public int TeethAmount = 3;

    private TeethWidget _widget;


    private void Start()
    {
        _character = GetComponent<Character>();
        _widget = FindObjectsOfType<TeethWidget>().Where(s => s.Name == _character.Name).First();
        UpdateWidget();
    }

    //public TeethType GetRandomTeeth()
    //{
    //    return (TeethType)System.Enum.GetValues(typeof(TeethType)).GetValue(Random.Range(0, System.Enum.GetValues(typeof(TeethType)).Length - 1));
    //}

    public void AddTooth(TeethType teeth)
    {
        //teethInventory.Add(teeth);
        TeethAmount++;
        UpdateWidget();
    }

    //public TeethType? RemoveTeeth()
    //{
    //    if (teethInventory.Count > 0)
    //    {
    //        TeethType toDrop = teethInventory[Random.Range(0, teethInventory.Count - 1)];
    //        teethInventory.Remove(toDrop);
    //        return toDrop;
    //    }
    //    return null;
    //}

    [ContextMenu("Drop tooth")]
    public void DropTooth()
    {
        if (TeethAmount == 0) return;

        TeethAmount = Mathf.Max(TeethAmount - 1, 0);
        Tooth newTooth = Instantiate(toothPrefab);
        Vector3 nPos = transform.position;
        nPos.y += heightThrow;
        newTooth.transform.position = nPos;

        //Debug.Break();
        newTooth.SetAsPhysical();
        ThrowObject(newTooth.Rigidbody);
        UpdateWidget();
    }

    private void UpdateWidget()
    {
        _widget.TeethAmountText.text = TeethAmount.ToString();
    }

    public void ThrowObject(Rigidbody rigidbody)
    {

        Vector3 direction = Vector3.up;
        //direction = Quaternion.Euler(new Vector3(0, Random.Range(0, 359), 0)) * direction * Random.Range(minThrow, maxThrow);

        // Reset forces
        rigidbody.velocity = Vector3.zero;

        // Add impulse
        rigidbody.AddForce(direction.normalized * ThrowForce, ForceMode.Impulse); // To exclude the mass -> ForceMode.VelocityChange
    }
}
