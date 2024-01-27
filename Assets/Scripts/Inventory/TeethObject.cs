using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class TeethObject : MonoBehaviour{
    public TeethType teethType;

    private void OnTriggerEnter (Collider other){
        if(other.gameObject.layer==LayerMask.NameToLayer("Player")){
            other.gameObject.GetComponent<TeethManager>().addTeeth(teethType);
        }
    }

    public int getRandomTeeth(){
        return Random.Range(0,System.Enum.GetValues(typeof(TeethType)).Length-1);
    }

}
