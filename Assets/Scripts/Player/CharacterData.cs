using UnityEngine;


public enum CharacterType
{
    Rulas,
    Jenny,
    Richarl,
    Vane
}


[CreateAssetMenu(fileName = "CharacterData", menuName = "ScriptableObjects/CharacterData")]
public class CharacterData : ScriptableObject
{
    public CharacterType Name;
    public Color Color;
}

