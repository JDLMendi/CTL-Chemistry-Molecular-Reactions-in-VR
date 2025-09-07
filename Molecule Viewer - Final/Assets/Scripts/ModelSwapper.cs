using System;
using UnityEngine;
using UnityEngine.UI;

public class ModelSwapper : MonoBehaviour
{
    public Molecule[] molecules;
    public int moleculeIndex;
    
    [Header("References")]
    public Image modelImage;
    public Text moleculeName;

    private void Update()
    {
        modelImage.sprite = molecules[moleculeIndex].image;
        moleculeName.text = molecules[moleculeIndex].moleculeName;
    }

    public void GoForward()
    {
        if (molecules.Length == 0) return;
        moleculeIndex = (moleculeIndex + 1) % molecules.Length;

    }

    public void GoBack()
    {
        if (molecules.Length == 0) return;
        moleculeIndex = (moleculeIndex - 1 + molecules.Length) % molecules.Length;
    }

    public void LoadModel()
    {
        Debug.Log("Loading Model in Index: " +  moleculeIndex);
        var prefab = molecules[moleculeIndex].prefab;
        
        
    }
}
