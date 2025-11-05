using System;
using UnityEngine;

public class ViewManager : MonoBehaviour
{
    [Header("Current Molecule")]
    public Molecule molecule;
    public int previousIndex = 0; 
    
    [Header("References")]
    public MoleculeHandler moleculeHandler;
    public ModelSwapper modelSwapper;
    public GameObject buttonPanel;
    public GameObject buttonPrefab;
    
    private void Start()
    {
        moleculeHandler = FindFirstObjectByType<MoleculeHandler>();
        modelSwapper = FindFirstObjectByType<ModelSwapper>();
        UpdateButtons();
    }

    private void Update()
    {
        // Check if the molecule index has changed
        if (modelSwapper.moleculeIndex != previousIndex)
        {
            // If it changed, update the molecule reference
            molecule = modelSwapper.molecules[modelSwapper.moleculeIndex];
            
            // Rebuild the buttons
            UpdateButtons();
            
            // Store the new index as the "previous" one for the next frame
            previousIndex = modelSwapper.moleculeIndex;
        }
    }

    private void UpdateButtons()
    {
        // 1. Delete all existing buttons
        foreach (Transform child in buttonPanel.transform)
        {
            Destroy(child.gameObject);
        }

        // Safety check in case the molecule is null
        if (molecule == null) 
        {
            Debug.LogWarning("Current molecule is null. No buttons will be created.");
            return;
        }

        // 2. Create new buttons for the current molecule
        foreach (Quaternion rotation in molecule.specialRotations)
        {
            var button = Instantiate(buttonPrefab, buttonPanel.transform);
            var rotationProperty = button.GetComponent<RotationProperty>();
            rotationProperty.rotation = rotation;
        }
    }

    public void UpdateAngle(Quaternion rotation)
    {
        moleculeHandler.UpdateMoleculeTransform(rotation);
    }
}