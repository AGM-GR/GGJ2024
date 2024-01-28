using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalDoor : MonoBehaviour
{
    public GameObject Platform;
    public Collider Trigger;

    private void Start()
    {
        
        Close();
    }

    public void Open()
    {
        Trigger.enabled = true;
        Platform.SetActive(true);
    }

    public void Close()
    {
        Trigger.enabled = false;
        Platform.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Character character = other.GetComponent<Character>();
            Debug.Log(character.Name + "WINS");
            PlayerPrefs.SetInt("Winner", character.CharacterIndex);
            //SceneManager
        }
    }
}
