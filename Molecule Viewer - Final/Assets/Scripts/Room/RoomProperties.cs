using Ubiq.Messaging;
using UnityEngine;

public class RoomProperties : MonoBehaviour
{
    [Header("Room Properties")]
    public string hostID;
    public string isPeersVisible;
    public string isFree;

    public void Awake()
    {
        hostID = "hostID";
        isPeersVisible = "isPeersVisible";
        isFree = "isFree";
    }
}
