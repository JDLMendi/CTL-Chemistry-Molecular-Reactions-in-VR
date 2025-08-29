using System;
using Ubiq.Messaging;
using Ubiq.Rooms;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour
{
    public string myID;

    [Header("Host Role")]
    public string currentHostUuid;
    public bool isHost = false;
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

    private struct HostAnnouncement
    {
        public string hostUuid;

        public HostAnnouncement(string hostUuid)
        {
            this.hostUuid = hostUuid;
        }
    }

    private void Start()
    {
        context = NetworkScene.Register(this);
    }

    private void Update()
    {
        myID = roomClient.Me.uuid;
        text.text = "ID: "  + myID;
        if (isHost)
        {
            // Debug.Log("Message Sent");
            context.SendJson(new StateUpdate(handler.currentScale, handler.currentRotation, handler.animationProgress));
            
            context.SendJson(new HostAnnouncement(myID));
        }
        else
        {
            UpdatePeerVisibility();
        }
    }

    private void UpdatePeerVisibility() {}
    public void ProcessMessage(ReferenceCountedSceneGraphMessage message)
    {
        string json = message.ToString();

        if (json.Contains("\"hostUuid\""))
        {
            var announcement = message.FromJson<HostAnnouncement>();
            currentHostUuid = announcement.hostUuid;
        }

        else if (json.Contains("\"animationProgress\""))
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
    public void ToggleHost()
    {
        isHost = !isHost;
        currentHostUuid = roomClient.Me.uuid;
    }

    public void MakeHost()
    {
        isHost = true;
    }

    public void StopHost()
    {
        isHost = false;
    }
    
}
