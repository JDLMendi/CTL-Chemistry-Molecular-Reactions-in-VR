using System;
using UnityEngine;
using Ubiq.Rooms;

public class HostSetter : MonoBehaviour
{
    [Header("Room Properties")]
    public RoomProperties properties;
    
    [Header("References")]
    public GameObject socialMenu;

    private RoomClient _roomClient;

    private void OnEnable()
    {
        properties = FindFirstObjectByType<RoomProperties>();
        _roomClient = FindFirstObjectByType<RoomClient>();;
    }

    private void CloseDialogue()
    {
        this.gameObject.SetActive(false);
        socialMenu.SetActive(true);
    }
    
    public void OnYes()
    {
        _roomClient.Room[properties.hostID] = _roomClient.Me.uuid;
        CloseDialogue();
    }

    public void OnNo()
    {
        CloseDialogue();

    }
}
