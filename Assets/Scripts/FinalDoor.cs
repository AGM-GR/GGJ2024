using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class FinalDoor : MonoBehaviour
{
    public Collider Trigger;
    public GameObject PortalFX;

    public GameObject FinalSet;

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
                FinalSet.SetActive(true);

                Debug.Log(character.Name + " WINS");
                PlayerPrefs.SetInt("Winner", character.CharacterIndex);
                //SceneManager
              
                if(character.CharacterIndex == 2){
                    GameObject.Find("Richarl_RIG_end").SetActive(true);
                    GameObject.Find("Richarl_RIG_end_1").SetActive(false);

                    GameObject.Find("LaJenny_RIG_end").SetActive(false);
                    GameObject.Find("ElRulas_RIG_end").SetActive(false);
                    GameObject.Find("LaVane_RIG_end").SetActive(false);
                }
                else if(character.CharacterIndex == 1){
                    GameObject.Find("LaJenny_RIG_end").SetActive(true);
                    GameObject.Find("LaJenny_RIG_end_1").SetActive(false);

                    GameObject.Find("Richarl_RIG_end").SetActive(false);
                    GameObject.Find("ElRulas_RIG_end").SetActive(false);
                    GameObject.Find("LaVane_RIG_end").SetActive(false);
                }
                else if(character.CharacterIndex == 0){
                    GameObject.Find("ElRulas_RIG_end").SetActive(true);
                    GameObject.Find("ElRulas_RIG_end_1").SetActive(false);

                    GameObject.Find("Richarl_RIG_end").SetActive(false);
                    GameObject.Find("LaJenny_RIG_end").SetActive(false);
                    GameObject.Find("LaVane_RIG_end").SetActive(false);
                }
                else if(character.CharacterIndex == 3){
                    GameObject.Find("LaVane_RIG_end").SetActive(true);
                    GameObject.Find("LaVane_RIG_end_1").SetActive(false);

                    GameObject.Find("Richarl_RIG_end").SetActive(false);
                    GameObject.Find("LaJenny_RIG_end").SetActive(false);
                    GameObject.Find("ElRulas_RIG_end").SetActive(false);
                }

                

            }
           
        }
    }
}
