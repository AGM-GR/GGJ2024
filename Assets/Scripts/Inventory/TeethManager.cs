using UnityEngine;
using System.Linq;
using UniRx;

public class TeethManager : MonoBehaviour
{
    private Character _character;
    public Tooth toothPrefab;
    public Transform SpawningPoint;

    public float ThrowForce = 12.0f;
    public float heightThrow = 10f;
    public float minThrow = 1f;
    public float maxThrow = 10f;

    public ReactiveProperty<int> TeethAmount = new(3);

    private PlayerAreaWidget _widget;


    private void Start()
    {
        _character = GetComponent<Character>();
        _widget = FindObjectsOfType<PlayerAreaWidget>().Where(s => s.characterData.Name == _character.Name).First();
        _widget.Init(TeethAmount);
    }

    public void AddTooth(TeethType teeth)
    {
        TeethAmount.Value++;
    }


    [ContextMenu("Drop tooth")]
    public void DropTooth()
    {
        if (TeethAmount.Value == 0) return;

        TeethAmount.Value = Mathf.Max(TeethAmount.Value - 1, 0);
        Tooth newTooth = Instantiate(toothPrefab, SpawningPoint.position, Quaternion.identity);

        //Debug.Break();
        newTooth.SetAsPhysical();

        Vector3 direction = Vector3.right * Random.Range(-1, 2) + Vector3.forward * Random.Range(-1, 2) + Vector3.up;
        newTooth.Rigidbody.AddForce(direction.normalized * ThrowForce, ForceMode.VelocityChange);
    }
}
