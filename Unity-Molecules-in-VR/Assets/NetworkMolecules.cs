using UnityEngine;
using Ubiq.Messaging;

public class NetworkMolecules : MonoBehaviour
{
    private NetworkContext context;
    public ChemAnimationControl chemAnimControl;
    public bool isHost = false;
    private struct Message
    {
        public Vector3 scale;
        public float animProgress;

        public Message(Vector3 scale, float animProgress)
        {
            this.scale = scale;
            this.animProgress = animProgress;
        }
    }

    private void Start()
    {
        context = NetworkScene.Register(this);
    }

    private void FixedUpdate()
    {
        if (isHost)
        {
            Vector3 currentScale = chemAnimControl.transform.localScale;
            float currentProgress = chemAnimControl.animProgress;
            context.SendJson(new Message(currentScale, currentProgress));
        }
    }

    public void ProcessMessage(ReferenceCountedSceneGraphMessage msg)
    {
        var data = msg.FromJson<Message>();
        chemAnimControl.transform.localScale = data.scale;
        chemAnimControl.animProgress = data.animProgress;
    }
}