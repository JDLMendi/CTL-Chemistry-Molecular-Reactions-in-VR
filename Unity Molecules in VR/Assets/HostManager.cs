using System;
using UnityEngine;

public class HostManager : MonoBehaviour
{
    public bool isHost = false;
    public GameObject[] moleculeModels;

    private void Start()
    {
        var swapper = FindFirstObjectByType<ModelSwapper>();
        moleculeModels = swapper.molecule_models;
    }
    
}
