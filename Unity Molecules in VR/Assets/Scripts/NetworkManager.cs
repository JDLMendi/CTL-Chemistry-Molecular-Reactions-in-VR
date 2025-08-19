using System;
using Ubiq.Messaging;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    [Header("Host Role")]
    public bool isHost = false;
    private NetworkContext context;
    public MoleculeManagerMain manager;

    [Header("Debug Information")]
    public Vector3 scale;
    public Quaternion rotation;
    public float animationProgress;

    private struct Message
    {
        public Vector3 scale;
        public Quaternion rotation;
        public float animationProgress;

        public Message(Vector3 scale, Quaternion rotation, float animationProgress)
        {
            this.scale = scale;
            this.rotation = rotation;
            this.animationProgress = animationProgress;
        }
    }

    private void Start()
    {
        context = NetworkScene.Register(this);
    }

    private void Update()
    {
        if (isHost)
        {
            Debug.Log("Message Sent");
            context.SendJson(new Message(manager.currentScale, manager.currentRotation, manager.animationProgress));
        }
    }

    public void ProcessMessage(ReferenceCountedSceneGraphMessage message)
    {
        Debug.Log("Message Received");
        var data = message.FromJson<Message>();
        
        #if UNITY_EDITOR
        scale = data.scale;
        rotation = data.rotation;
        animationProgress = data.animationProgress;
        #endif
        
        animationProgress = data.animationProgress;
        manager.UpdateMoleculeState(data.animationProgress, data.scale, data.rotation);
    }

    public void ToggleHost()
    {
        isHost = !isHost;
    }
    
}
