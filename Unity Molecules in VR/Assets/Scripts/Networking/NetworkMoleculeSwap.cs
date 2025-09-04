using System;
using UnityEngine;
using Ubiq.Messaging;
using Ubiq.Rooms;

public class NetworkMoleculeSwap : MonoBehaviour
{
    public ModelSwapper modelSwapper;
    public HostManager hostManager;
    private int modelIndex;

    [Header("Ubiq Stuff")]
    public RoomClient roomClient;
    public RoomProperties roomProperties;
    public int lastKnownModel;

    private struct ModelChange
    {
        public int modelIndex;

        public ModelChange(int modelIndex)
        {
            this.modelIndex = modelIndex;
        }
    }

    private void Start()
    {
        modelSwapper = FindFirstObjectByType<ModelSwapper>();
        hostManager = FindFirstObjectByType<HostManager>();
    }

    public void Update()
    {
        // Safely parse the current state from the room property
        int.TryParse(roomClient.Room[roomProperties.isPeerVisible], out int currentModel);

        // Add a check if the host has enabled freeview
        
        
        // Only call the function if the state has actually changed
        if (currentModel != lastKnownModel)
        {
            modelSwapper.model_index = currentModel;
            lastKnownModel = currentModel;
            modelSwapper.LoadModel();
        }
    }
}
