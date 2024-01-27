using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeethManager : MonoBehaviour{

    List<TeethType> teethInventory = new List<TeethType>();

    public float getRandomTeeth(){
        return Random.Range(0,System.Enum.GetValues(typeof(TeethType)).Length-1);
    }

    public void addTeeth(TeethType teeth){
        //puedes coger dientes iguales?
        teethInventory.Add(teeth);
    }

    public TeethType? removeTeeth(){
        if(teethInventory.Count>0){
            TeethType toDrop = teethInventory[Random.Range(0,teethInventory.Count-1)];
            teethInventory.Remove(toDrop);
            return toDrop;
        }
        return null;
    }



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
