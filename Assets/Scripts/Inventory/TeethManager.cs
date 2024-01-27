using UnityEngine;
using System.Linq;

public class TeethManager : MonoBehaviour
{
    private Character _character;
    public Tooth toothPrefab;
    public Transform SpawningPoint;

    public float ThrowForce = 12.0f;
    public float heightThrow = 10f;
    public float minThrow = 1f;
    public float maxThrow = 10f;
    public int TeethAmount = 3;

    private TeethWidget _widget;


    private void Start()
    {
        _character = GetComponent<Character>();
        _widget = FindObjectsOfType<TeethWidget>().Where(s => s.Name == _character.Name).First();
        UpdateWidget();
    }

    public void AddTooth(TeethType teeth)
    {
        TeethAmount++;
        UpdateWidget();
    }


    [ContextMenu("Drop tooth")]
    public void DropTooth()
    {
        if (TeethAmount == 0) return;

        TeethAmount = Mathf.Max(TeethAmount - 1, 0);
        Tooth newTooth = Instantiate(toothPrefab, SpawningPoint.position, Quaternion.identity);

        //Debug.Break();
        newTooth.SetAsPhysical();
        //AddImpulseForce(newTooth.Rigidbody, ThrowForce);
        newTooth.Rigidbody.AddForce((transform.up/2f + transform.forward).normalized * ThrowForce, ForceMode.VelocityChange);

        UpdateWidget();
    }

    private void UpdateWidget()
    {
        _widget.TeethAmountText.text = TeethAmount.ToString();
    }
}
