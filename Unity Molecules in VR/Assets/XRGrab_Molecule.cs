using System;
using Ubiq.Rooms;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class XRGrab_Molecule : MonoBehaviour
{
    public RoomClient roomClient;
    public RoomProperties properties;
    public XRGrabInteractable grabInteractable;

    private void Awake()
    {
        roomClient = FindFirstObjectByType<RoomClient>();
        properties = FindFirstObjectByType<RoomProperties>();
        grabInteractable = GetComponent<XRGrabInteractable>();
        
        roomClient.OnRoomUpdated.AddListener(HostCheck);
    }

    // Enables and disables Grab Interactable if they are not the host
    private void HostCheck(IRoom room)
    {
        var hostData = room[properties.hostID];
        if (hostData != roomClient.Me.uuid)
        {
            grabInteractable.enabled = false;
        }
        else
        {
            grabInteractable.enabled = true;
        }
        
        bool.TryParse(room[properties.isPeersVisible], out bool freeroamData);
        if (freeroamData)
        {
            grabInteractable.enabled = true;
        }
        else
        {
            grabInteractable.enabled = false;
        }
    }
}
