using System;
using UnityEngine;
using Ubiq.Rooms;

public class PeerManager : MonoBehaviour
{
    public bool isPeersVisible;
    public string currentHostID;
    
    [Header("Room Properties")] 
    public RoomProperties properties;
    
    private RoomClient _roomClient;
    private VisbilityHandler _visbilityHandler;

    private void Awake()
    {
        properties = FindFirstObjectByType<RoomProperties>();
        _roomClient = FindFirstObjectByType<RoomClient>();
        _visbilityHandler = FindFirstObjectByType<VisbilityHandler>();

        _roomClient.OnRoomUpdated.AddListener(RoomClient_OnRoomUpdated);
    }

    private void RoomClient_OnRoomUpdated(IRoom room)
    {
        var hostID = room[properties.hostID];
        if (!String.IsNullOrEmpty(hostID))
        {
            currentHostID = hostID;
        }
        
        var data = room[properties.isPeersVisible];
        bool.TryParse(data, out isPeersVisible);
        
        _visbilityHandler.ToggleVisibilty(isPeersVisible);

    }
}
