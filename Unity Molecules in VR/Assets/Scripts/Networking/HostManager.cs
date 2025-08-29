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
    }

    private void OnRoomUpdated_RoomUpdate(IRoom room)
    {
        // We establish a property in the room which identifies who is the host, in our case the host is the first person in the room
        var hostIDProperty = room[hostID];
        if (string.IsNullOrEmpty(hostIDProperty))
        {
             roomClient.Room[hostID] = roomClient.Me.uuid;
        }
    }

    private void Update()
    {
        currentHostID = roomClient.Room[hostID];

        if (roomClient.JoinedRoom)
        {
            if ((roomClient.Room[hostID] == roomClient.Me.uuid))
            {
                isHost = true;
                EnableHostWindows();
            }
            else
            {
                isHost = false;
                DisableHostWindows();
            }
        }
    }

    private void DisableHostWindows()
    {
        textObject.SetActive(false);
        toolbar.SetActive(false);
        modelSwapperPanel.SetActive(false);
    }
    
    private void EnableHostWindows()
    {
        textObject.SetActive(true);
        toolbar.SetActive(true);
        modelSwapperPanel.SetActive(true);
    }
}
