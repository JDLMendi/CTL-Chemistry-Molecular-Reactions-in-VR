using System;
using UnityEngine;
using Ubiq.Rooms;

public class HostCheck : MonoBehaviour
{
    [Header("Room Properties")]
    public RoomProperties properties;
    
    [Header("References")]
    public GameObject socialMenu;
    public GameObject hostDialogue;
    
    private RoomClient _roomClient;
    
    
    
    private void Awake()
    {
        properties = FindFirstObjectByType<RoomProperties>();
        
        _roomClient = FindFirstObjectByType<RoomClient>();
        _roomClient.OnRoomUpdated.AddListener(RoomClient_OnRoomUpdated);
    }

    private void RoomClient_OnRoomUpdated(IRoom room)
    {
        // Checks if we've joined a room
        if (!_roomClient.JoinedRoom) return;
        
        // Does a check if a Host ID exists for the room we joined
        var data = room[properties.hostID];
        if (string.IsNullOrEmpty(data) )
        {
            socialMenu.SetActive(false);
            hostDialogue.SetActive(true);
        }
    }
}
