using UnityEngine;
using System.Linq;
using UniRx;
using System.Collections.Generic;

public class TeethManager : MonoBehaviour
{
    private Character _character;
    public List<Tooth> toothPrefabs;
    public Transform SpawningPoint;

    public float ThrowForce = 12.0f;
    public float heightThrow = 10f;
    public float minThrow = 1f;
    public float maxThrow = 10f;

    public ReactiveCollection<TeethType> teeths = new();

    public int TeethAmount => teeths.Count;
    private int normalTeeth;
    private int goldTeeth;

    private PlayerAreaWidget _widget;


    public void Initialize()
    {
        _character = GetComponent<Character>();
        _widget = FindObjectsOfType<PlayerAreaWidget>().Where(s => s.characterData.Name == _character.Name).First();
        _widget.Init(teeths);
        AddTooth(TeethType.Normal);
    }

    public bool CanAdd(TeethType teethType)
    {
        if (teeths.Where(t => t == TeethType.Normal).Count() >= 4 && teethType == TeethType.Normal) return false;
        return true;
    }

    // Solo puedes coger el que te hace falta, si ya no tienes huecos para blancos, no los coges

    public void AddTooth(TeethType teeth)
    {
        teeths.Add(teeth);
        if (teeth == TeethType.Normal) normalTeeth++;
        else goldTeeth++;

        if (TeethAmount == 4)
        {
            FindObjectOfType<GoldToothManager>().TrySpawnGoldTooth();
        }

        if (normalTeeth == 4 && goldTeeth == 1)
        {
            FindObjectOfType<FinalDoorsManager>().OpenRandomDoor();
        }
    }


    [ContextMenu("Drop tooth")]
    public void DropTooth()
    {
        if (TeethAmount == 0) return;

        TeethType tooth = RemoveRandomFromList();
        InstantiateTooth(tooth);
        FindObjectOfType<FinalDoorsManager>().CloseOpenedDoor();
    }

    private TeethType RemoveRandomFromList()
    {
        int index = Random.Range(0, teeths.Count);
        TeethType tooth = teeths[index];
        teeths.RemoveAt(index);

        if (tooth == TeethType.Normal) normalTeeth--;
        else goldTeeth--;

        return tooth;
    }

    private void InstantiateTooth(TeethType tooth)
    {
        var prefab = toothPrefabs.Where(p => p.TeethType == tooth).First();
        Tooth newTooth = Instantiate(prefab, SpawningPoint.position, Quaternion.identity);

        //Debug.Break();
        newTooth.SetAsPhysical();

        Vector3 direction = Vector3.right * Random.Range(-1, 2) + Vector3.forward * Random.Range(-1, 2) + Vector3.up;
        newTooth.Rigidbody.AddForce(direction.normalized * ThrowForce, ForceMode.VelocityChange);
    }
}
