using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class FinalDoor : MonoBehaviour
{
    public Collider Trigger;
    public GameObject PortalFX;

    private void Start()
    {
        
        Close();
    }

    public void Open()
    {
        PortalFX.SetActive(true);
        Trigger.enabled = true;
    }

    public void Close()
    {
        Trigger.enabled = false;
        PortalFX.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            Character character = other.GetComponent<Character>();

            if(character.GetComponent<TeethManager>().GoldTeeth>0){
                Debug.Log(character.Name + "WINS");
                PlayerPrefs.SetInt("Winner", character.CharacterIndex);
                //SceneManager
            }
           
        }
    }
}
