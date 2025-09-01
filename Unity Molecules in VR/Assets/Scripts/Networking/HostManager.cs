using System;
using System.Collections.Generic;
using System.Linq;
using Ubiq.Messaging;
using Ubiq.Rooms;
using Ubiq.Samples;
using UnityEngine;
using UnityEngine.Events;

public class HostManager : MonoBehaviour
{
    [Header("Host")]
    public bool isHost = false;
    public string hostID;
    public string currentHostID;
    
    [Header("Peer Handler")]
    public PeerHandler peerHandler;
    
    [Header("Peer Audio")]
    public bool isPeerAudible = true;
    public UnityEvent<bool> onPeerAudible;
    
    [Header("Peer Visbility")]
    public bool isPeerVisible = false;
    public UnityEvent<bool> onPeerVisible;
    
    [Header("References")]
    public GameObject textObject;
    public GameObject modelSwapperPanel;
    public GameObject toolbar;
    
    // Private Variables
    private RoomClient roomClient;
    private NetworkContext context;

    private struct HostCommnad
    {
        public string command;

        public HostCommnad(string command)
        {
            this.command = command;
        }
    }
    
    private void OnEnable()
    {
        context = NetworkScene.Register(this);
        
        // Initialising Events
        onPeerVisible = new UnityEvent<bool>();
        onPeerAudible = new UnityEvent<bool>();
        
        roomClient = FindAnyObjectByType<RoomClient>();
        peerHandler = FindAnyObjectByType<PeerHandler>();
        
        roomClient.OnJoinedRoom.AddListener(OnRoomUpdated_RoomUpdate);
        roomClient.OnPeerAdded.AddListener(OnRoomUpdated_RoomUpdate);
    }

    private void Update()
    {
        #if UNITY_EDITOR
        currentHostID = roomClient.Room[hostID];
        #endif
    }

    private void Start()
    {
        ToggleVisbility();
        ToggleAudio();
    }

    private void OnRoomUpdated_RoomUpdate(IPeer peer)
    {
        var hostIDProperty = roomClient.Room[hostID];
        if (hostIDProperty == roomClient.Me.uuid)
        {
            EnableHostWindows(); 
        }
        else
        {
            DisableHostWindows();
        }
    }
    
    private void OnRoomUpdated_RoomUpdate(IRoom room)
    {
        // We establish a property in the room which identifies who is the host, in our case the host is the first person in the room
        var hostIDProperty = roomClient.Room[hostID];
        if (string.IsNullOrEmpty(hostIDProperty))
        {
             roomClient.Room[hostID] = roomClient.Me.uuid;
        }
        
    }
    
    private void DisableHostWindows()
    {
        isHost = false;
        textObject.SetActive(false);
        toolbar.SetActive(false);
        modelSwapperPanel.SetActive(false);
        
        // Bug when loop is previously enabled
        toolbar.GetComponent<ToolbarManager>().RestartToolBar();
    }
    
    public void EnableHostWindows()
    {
        isHost = true;
        textObject.SetActive(true);
        toolbar.SetActive(true);
        modelSwapperPanel.SetActive(true);
    }

    public void ToggleVisbility()
    {
        SendCommnad("ToggleVisibility");
        if (isPeerVisible)
        {
            onPeerVisible.Invoke(false);
            isPeerVisible = false;
        } else if (!isPeerVisible)
        {
            onPeerVisible.Invoke(true);
            isPeerVisible = true;
        }
    }

    public void ToggleAudio()
    {
        SendCommnad("ToggleAudio");
        if (isPeerAudible)
        {
            onPeerAudible.Invoke(false);
            isPeerAudible = false;
        } else if (!isPeerAudible)
        {
            onPeerAudible.Invoke(true);
            isPeerAudible = true;
        }
    }

    private void SendCommnad(string command)
    {
        context.SendJson(new HostCommnad(command));
    }

    public void ProcessMessage(ReferenceCountedSceneGraphMessage message)
    {
        var data = message.FromJson<HostCommnad>();
        
        if (data.command == "ToggleVisibility") ToggleVisbility();
        else if (data.command == "ToggleAudio") ToggleAudio();
        else Debug.Log("Unknown Command: " +  data.command);
    }
    
}
