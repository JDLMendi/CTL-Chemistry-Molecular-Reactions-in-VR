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
    [Header("Room Properties")]
    public string hostID;
        
    [Header("Host")]
    public bool isHost = false;
    public string currentHostID;
    
    [Header("Peer Handler")]
    public PeerHandler peerHandler;
    
    [Header("Peer Toggles")]
    public bool isPeerAudible = true;
    public bool isPeerVisible = false;
    
    [Header("References")]
    public GameObject textObject;
    public GameObject modelSwapperPanel;
    public GameObject toolbar;
    
    // Private Variables
    private RoomClient roomClient;
    
    private void OnEnable()
    {
        roomClient = FindAnyObjectByType<RoomClient>();
        peerHandler = FindAnyObjectByType<PeerHandler>();
        
        roomClient.OnRoomUpdated.AddListener(OnRoomUpdated_RoomUpdate);
        roomClient.OnPeerAdded.AddListener(OnRoomUpdated_RoomUpdate);
    }

    private void Update()
    {
        #if UNITY_EDITOR
        currentHostID = roomClient.Room[hostID];
        #endif
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
        var hostIDProperty = room[hostID];
        if (string.IsNullOrEmpty(hostIDProperty))
        {
             roomClient.Room[hostID] = roomClient.Me.uuid;
        }
        else
        {
            roomClient.Room[hostID] = room[hostID];
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

    public void ClearHost()
    {
        roomClient.Room[hostID] = null;
    }
    
    
}
