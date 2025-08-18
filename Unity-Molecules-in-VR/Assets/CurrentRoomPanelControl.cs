using TMPro;
using Ubiq.Rooms;
using UnityEngine;
using UnityEngine.UI;

public class CurrentRoomPanelControl : MonoBehaviour
{
    public TMP_Text Joincode;

    private string existing;

    public void Bind(RoomClient client)
    {
        Joincode.text = client.Room.JoinCode.ToUpperInvariant();
    }
}
