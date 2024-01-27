using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class TeethObject : MonoBehaviour{
    public TeethType teethType;

    private void OnTriggerEnter (Collider other){
        Debug.Log(other.name);
        if(other.gameObject.layer==LayerMask.NameToLayer("Player")){
            other.gameObject.GetComponent<TeethManager>().addTeeth(teethType);
            Destroy(gameObject);
        }
    }

    public int getRandomTeeth(){
        return Random.Range(0,System.Enum.GetValues(typeof(TeethType)).Length-1);
    }

}
