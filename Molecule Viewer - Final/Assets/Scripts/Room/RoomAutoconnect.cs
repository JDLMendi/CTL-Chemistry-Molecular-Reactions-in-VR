using System;
using System.Collections.Generic;
using Ubiq.Rooms;
using UnityEngine;

public class RoomAutoconnect : MonoBehaviour
{
    // Public Variables
    public RoomClient roomClient;

    [Header("Should autoconnect?")]
    public bool isAutoConnect;

    private void Awake()
    {
        roomClient = FindFirstObjectByType<RoomClient>();
        roomClient.OnRooms.AddListener(RoomClient_OnRoomDiscovered);
    }

    private void OnEnable()
    {
        roomClient.DiscoverRooms();
    }

    private void RoomClient_OnRoomDiscovered(List<IRoom> rooms, RoomsDiscoveredRequest request)
    {
        Debug.Log("There are " +  rooms.Count + " rooms discovered.");
        if (rooms.Count == 0)
        {
            AutoCreateRoom();
            return;
        }

        // There should only be ONE instance of the room that a person can join to. If there is more than one then proceed with solo-version
        if (rooms.Count != 1) return;

        IRoom room = rooms[0];
        AutojoinRoom(room);
    }

    private void AutojoinRoom(IRoom room)
    {
        var joincode = room.JoinCode;
        roomClient.Join(joincode);
    }

    private void AutoCreateRoom()
    {
        roomClient.Join(name: "Host Room", publish: true);
    }
    
    
}


