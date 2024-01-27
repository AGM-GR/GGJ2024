using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class TeethObject : MonoBehaviour{
    public TeethType teethType;

    public Collider mCollider;
    public Collider colliderTrigger;

    public float timeDisabled =  1f;


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

    void Start(){
        colliderTrigger.enabled = false;
        StartCoroutine(EnableTrigger());

    }

    IEnumerator EnableTrigger() {
        
        yield return new WaitForSeconds(timeDisabled);
        colliderTrigger.enabled = true;
        yield return null;
    }



}
