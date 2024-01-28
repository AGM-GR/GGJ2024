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

    private PlayerAreaWidget _widget;


    private void Start()
    {
        _character = GetComponent<Character>();
        _widget = FindObjectsOfType<PlayerAreaWidget>().Where(s => s.characterData.Name == _character.Name).First();
        _widget.Init(teeths);
    }

    public void AddTooth(TeethType teeth)
    {
        teeths.Add(teeth);

        if (TeethAmount == 4)
        {
            FindObjectOfType<GoldToothManager>().TrySpawnGoldTooth();
        }        
    }


    [ContextMenu("Drop tooth")]
    public void DropTooth()
    {
        if (TeethAmount == 0) return;

        int index = Random.Range(0, teeths.Count);
        TeethType tooth = teeths[index];           
        teeths.RemoveAt(index);

        var prefab = toothPrefabs.Where(p => p.TeethType == tooth).First();
        Tooth newTooth = Instantiate(prefab, SpawningPoint.position, Quaternion.identity);

        //Debug.Break();
        newTooth.SetAsPhysical();

        Vector3 direction = Vector3.right * Random.Range(-1, 2) + Vector3.forward * Random.Range(-1, 2) + Vector3.up;
        newTooth.Rigidbody.AddForce(direction.normalized * ThrowForce, ForceMode.VelocityChange);
    }
}
