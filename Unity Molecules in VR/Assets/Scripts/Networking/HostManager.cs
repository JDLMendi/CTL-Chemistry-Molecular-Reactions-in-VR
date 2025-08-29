using System;
using System.Collections.Generic;
using System.Linq;
using Ubiq.Messaging;
using Ubiq.Rooms;
using Ubiq.Samples;
using UnityEngine;

public class HostManager : MonoBehaviour
{
    [Header("Host")]
    public bool isHost = false;
    public string hostID;
    public string currentHostID;

    
    [Header("References")]
    public GameObject textObject;
    public GameObject modelSwapperPanel;
    public GameObject toolbar;
    
    // Private Variables
    private RoomClient roomClient;
    
    public void OnEnable()
    {
        roomClient = FindAnyObjectByType<RoomClient>();
        roomClient.OnJoinedRoom.AddListener(OnRoomUpdated_RoomUpdate);
        roomClient.OnPeerAdded.AddListener(OnRoomUpdated_RoomUpdate);
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

    private void Update()
    {
        #if UNITY_EDITOR
        currentHostID = roomClient.Room[hostID];
        #endif
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
    
}
