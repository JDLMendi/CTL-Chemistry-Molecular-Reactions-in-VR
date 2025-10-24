using UnityEngine;
using Ubiq.Messaging;

public class TeleportButton : MonoBehaviour
{
    public GameObject player;
    public Transform teleportAnchor;
    
    
    private NetworkContext context;
    
    private void Start()
    {
        context = NetworkScene.Register(this);
    }
    
    private struct TeleportCommand
    {
        public string command;

        public TeleportCommand(string command = "Teleport")
        {
            this.command = "Teleport";
        }
    }
    
    public void TeleportToAnchor()
    {
        player.transform.position = teleportAnchor.position;
        player.transform.rotation = teleportAnchor.rotation;
    }

    public void Teleport()
    {
        TeleportToAnchor();
        SendTeleportMessage();
    }

    public void SendTeleportMessage()
    {
        Debug.Log("Teleport Command sent!");
        context.SendJson(new TeleportCommand());
    }

    public void ProcessMessage(ReferenceCountedSceneGraphMessage message)
    {
        Debug.Log("Teleporting to anchor");
        var command = message.FromJson<TeleportCommand>();
        TeleportToAnchor();
    }

    
    
    
}
