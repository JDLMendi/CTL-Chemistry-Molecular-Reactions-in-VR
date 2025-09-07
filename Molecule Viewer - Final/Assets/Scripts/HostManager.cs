using System;
using Ubiq.Rooms;
using UnityEngine;
using UnityEngine.Events;

public class HostManager : MonoBehaviour
{
    public bool isHost;
    
    [Header("Peer Settings")]
    public bool isPeersVisible = true;
    
    [Header("Room Properties")] public RoomProperties properties;

    [Header("Host Events")]
    public UnityEvent<bool>  toggleHost;
    
    private RoomClient _roomClient;

    private void Awake()
    {
        toggleHost = new UnityEvent<bool>();
        
        properties = FindFirstObjectByType<RoomProperties>();
        _roomClient = FindFirstObjectByType<RoomClient>();

        _roomClient.OnRoomUpdated.AddListener(RoomClient_OnRoomUpdated);
    }

    public void RoomClient_OnRoomUpdated(IRoom room)
    {
        var hostID = room[properties.hostID];
        isHost = hostID == _roomClient.Me.uuid ? true : false;

        if (isHost)
        {
            ToggleTools(true);
        }
        else
        {
            ToggleTools(false);
        }
    }

    private void ToggleTools(bool toggle)
    {
        Debug.Log("Host has been toggled as:" + toggle);
        toggleHost?.Invoke(toggle);
    }
    
    public void ToggleVisibility()
    {
        isPeersVisible = !isPeersVisible;
        _roomClient.Room[properties.isPeersVisible] = (isPeersVisible).ToString();
    }
}
