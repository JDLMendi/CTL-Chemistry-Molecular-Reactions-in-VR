using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ModelSwapper : MonoBehaviour
{
    public Molecule[] molecules;
    public int moleculeIndex;
    public string currentMoleculeName;
    
    [Header("References")]
    public Image modelImage;
    public Text moleculeName;
    public Text moleculeLabel;

    [Header("Events")] public UnityEvent<int> onModelLoaded;

    private void Start()
    {
        onModelLoaded = new UnityEvent<int>();
    }

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

    public void UpdateModel(int moleculeIndex)
    {
        this.moleculeIndex = moleculeIndex;
        currentMoleculeName = molecules[moleculeIndex].moleculeName;
        LoadModel();
    }

    public void LoadModel()
    {
        Debug.Log("Loading Model in Index: " +  moleculeIndex);
        currentMoleculeName = molecules[moleculeIndex].moleculeName;
        onModelLoaded.Invoke(moleculeIndex);
    }
}
