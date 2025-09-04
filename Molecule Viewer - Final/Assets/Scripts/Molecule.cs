using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Molecule", menuName = "Scriptable Objects/Molecule")]
public class Molecule : ScriptableObject
{
    public Sprite image;
    public string moleculeName;
    public GameObject prefab;
}
