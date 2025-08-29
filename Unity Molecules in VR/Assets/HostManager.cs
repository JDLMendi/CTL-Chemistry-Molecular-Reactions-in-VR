using System;
using System.Collections.Generic;
using System.Linq;
using Ubiq.Messaging;
using Ubiq.Rooms;
using Ubiq.Samples;
using UnityEngine;

public class HostManager : MonoBehaviour
{
    public RoomClient roomClient;
    public string currentHostID;

    
    public SocialMenu mainMenu;

    public void OnEnable()
    {
        // roomClient.OnRoomUpdated.AddListener(OnRoomUpdated_RoomUpdate);
        roomClient.OnJoinedRoom.AddListener(OnRoomUpdated_RoomUpdate);
        // roomClient.OnPeerAdded.AddListener(OnRoomUpdated_RoomUpdate);
    }

    private void OnRoomUpdated_RoomUpdate(IRoom room)
    {
        var hostIDProperty = room[currentHostID];
        if (string.IsNullOrEmpty(hostIDProperty))
        {
             roomClient.Room[currentHostID] = roomClient.Me.uuid;
        }
    }

    private void Update()
    {
        Debug.Log("Host ID: " + roomClient.Room[currentHostID]);
    }
}
