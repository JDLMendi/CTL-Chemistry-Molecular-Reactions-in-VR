using UnityEngine;
using Ubiq.Messaging;

public class NetworkMolecules : MonoBehaviour
{
    public MoleculeController moleculeController;
    
    [Header("Network Role")]
    [Tooltip("Set to true for the object that controls the state (the 'owner' or 'master' client).")]
    public bool isHost = false;

    [Header("Animation Control")]
    [Tooltip("Controls the animation's normalized time (0.0 to 1.0). This value is synchronized.")]
    [Range(0f, 0.99f)]
    public float animProgress = 0f;

    private NetworkContext context;
    private struct Message
    {
        public Quaternion rotation;
        public Vector3 scale;
        public float animProgress;

        public Message(Quaternion rotation, Transform modelTransform, float animProgress)
        {
            this.rotation = rotation;
            this.scale = modelTransform.localScale;
            this.animProgress = animProgress;
        }
    }

    void Start()
    {
        context = NetworkScene.Register(this);
    }
    
    void LateUpdate()
    {
        if (isHost)
        {
            context.SendJson(new Message(transform.rotation, moleculeController.model.transform, animProgress));
        }
    }

    public void ProcessMessage(ReferenceCountedSceneGraphMessage msg)
    {
        var data = msg.FromJson<Message>();
        moleculeController.UpdateMolecule(data.rotation, data.scale, data.animProgress);
        moleculeController.animProgress =  animProgress;
    }

    public void ToggleHost()
    {
        isHost = !isHost;
    }
}