using System;
using Ubiq.Messaging;
using Ubiq.Rooms;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour
{
    public string myID;

    [Header("Host Role")]
    public HostManager hostManager;
    
    private NetworkContext context;
    public RoomClient roomClient;
    public MoleculeHandler handler;
    public Text text;

    [Header("Debug Information")]
    public Vector3 scale;
    public Quaternion rotation;
    public float animationProgress;

    private struct StateUpdate
    {
        public Vector3 scale;
        public Quaternion rotation;
        public float animationProgress;

        public StateUpdate(Vector3 scale, Quaternion rotation, float animationProgress)
        {
            this.scale = scale;
            this.rotation = rotation;
            this.animationProgress = animationProgress;
        }
    }

    private void Start()
    {
        hostManager = FindFirstObjectByType<HostManager>();
        context = NetworkScene.Register(this);
    }

    private void Update()
    {
        myID = roomClient.Me.uuid;
        text.text = "ID: "  + myID;
        if (hostManager.isHost)
        {
            context.SendJson(new StateUpdate(handler.currentScale, handler.currentRotation, handler.animationProgress));
        }
        else
        {
            UpdatePeerVisibility();
        }
    }

    private void UpdatePeerVisibility() {}
    public void ProcessMessage(ReferenceCountedSceneGraphMessage message)
    {
        var state = message.FromJson<StateUpdate>();
    
        #if UNITY_EDITOR
        scale = state.scale;
        rotation = state.rotation;
        animationProgress = state.animationProgress;
        #endif
    
        handler.UpdateMoleculeState(state.animationProgress, state.scale, state.rotation);
        
    }
}
