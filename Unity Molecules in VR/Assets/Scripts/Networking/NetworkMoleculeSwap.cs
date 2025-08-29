using System;
using UnityEngine;
using Ubiq.Messaging;

public class NetworkMoleculeSwap : MonoBehaviour
{
    public ModelSwapper modelSwapper;
    public HostManager hostManager;
    private int modelIndex;
    private NetworkContext context;

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
        context = NetworkScene.Register(this);
        
        modelSwapper = FindFirstObjectByType<ModelSwapper>();
        hostManager = FindObjectOfType<HostManager>();
        
        // Adds SwapModel as a Listener to when this is invoked
        modelSwapper.OnModelSwapped.AddListener(SwapModel);
    }

    // This should be called when there is a model swap and only send the message if they are the host
    private void SwapModel(int modelIndex)
    {
        Debug.Log("Swap Invoked!");
        if (hostManager.isHost)
        {
            context.SendJson(new ModelChange(modelIndex));
        }
    }

    public void ProcessMessage(ReferenceCountedSceneGraphMessage message)
    {
        var data = message.FromJson<ModelChange>();
        modelSwapper.model_index = data.modelIndex;
        modelSwapper.LoadModel();
    }
    
    
}
