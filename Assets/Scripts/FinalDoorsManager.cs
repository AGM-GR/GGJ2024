using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FinalDoorsManager : MonoBehaviour
{
    private List<FinalDoor> doors = new();
    private FinalDoor _openDoor;

    private void Start()
    {
        doors = FindObjectsOfType<FinalDoor>().ToList();
    }

    public void OpenRandomDoor()
    {
        _openDoor = doors.GetRandomElement();
        _openDoor.Open();
    }

    public void CloseOpenedDoor()
    {
        if(_openDoor != null)
        {
            _openDoor.Close();
        }
    }

}
