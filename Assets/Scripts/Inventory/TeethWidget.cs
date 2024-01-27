using UnityEngine;
using TMPro;

public class TeethWidget : MonoBehaviour
{
    public CharacterData CharacterData;
    public TextMeshProUGUI TeethAmountText;

    public CharacterType Name => CharacterData.Name;

    private void Start()
    {
        TeethAmountText.color = CharacterData.Color;
    }
}
