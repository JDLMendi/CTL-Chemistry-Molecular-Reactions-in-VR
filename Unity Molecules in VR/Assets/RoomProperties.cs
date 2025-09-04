using System;
using Ubiq.Rooms;
using UnityEngine;

public class RoomProperties : MonoBehaviour
{
    [Header("Room Client")]
    public RoomClient roomClient;
    
    [Header("Server Propoerties")]
    public string hostID;
    public string moleculeIndex;
    public string isPeerVisible;
    public string isPeerAudible;

    private void Start()
    {
        roomClient = FindFirstObjectByType<RoomClient>();
        if (roomClient == null)
        {
            Debug.LogError("No room client found");
            return;
        }
        
        // Initialising Room Propoerties with NAN or NULL
        roomClient.Room[hostID] = null;
        roomClient.Room[moleculeIndex] = null;
        roomClient.Room[isPeerVisible] = null;
        roomClient.Room[isPeerAudible] = null;

    }
}
