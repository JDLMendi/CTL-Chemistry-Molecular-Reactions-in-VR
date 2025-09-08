using Ubiq.Messaging;
using Ubiq.Rooms;
using UnityEngine;

public class NetworkMolecule : MonoBehaviour
{
    [Header("Molecule Room Property")] 
    public string moleculeIndex = "Molecule Index";
    
    [Header("Ubiq")]
    public RoomClient roomClient;

    [Header("Molecule Handler")]
    public bool isFreeMovement;
    public MoleculeHandler handler;

    private NetworkContext context;
    private int previousIndex = 0;

    private struct MoleculeState
    {
        public Vector3 scale;
        public Quaternion rotation;
        public float animationProgress;

        public MoleculeState(Vector3 scale, Quaternion rotation, float animationProgress)
        {
            this.scale = scale;
            this.rotation = rotation;
            this.animationProgress = animationProgress;
        }
    }
    public void Awake()
    {
        roomClient = FindFirstObjectByType<RoomClient>();
        handler = FindFirstObjectByType<MoleculeHandler>();
    }

    private void Start()
    {
        context = NetworkScene.Register(this);
        roomClient.OnRoomUpdated.AddListener(RoomClient_OnRoomUpdate);
        handler.modelSwapper.onModelLoaded.AddListener(ModelSwapper_OnModelLoaded);
    }

    private void RoomClient_OnRoomUpdate(IRoom room)
    {
        Debug.Log("Room has been updated!");
        bool.TryParse(room["isFree"], out isFreeMovement);
    }

    private void ModelSwapper_OnModelLoaded(int index)
    {
        // Disable automatic molecule model update if freemovement is set to true
        if (isFreeMovement == false && handler.isHost)
        {
            roomClient.Room[moleculeIndex] = index.ToString();
        }
    }

    private void Update()
    {
        // Only send the message if the peer is host and that free movement is disabled
        if (handler.isHost && isFreeMovement == false)
        {
            SendMoleculeState();
        }
        
        if (handler.currentMoleculeTransformer == null) return;

        if (isFreeMovement || handler.isHost)
        {
            handler.currentMoleculeTransformer.ToggleGrabbable(true);
        }
        else
        {
            handler.currentMoleculeTransformer.ToggleGrabbable(false);
        }
    }

    public void SendMoleculeState()
    {
        var scale = handler.currentScale;
        var rotation = handler.currentRotation;
        var animationProgress = handler.currentAnimationProgress;
        context.SendJson(new MoleculeState(scale, rotation, animationProgress));
    }

    public void ProcessMessage(ReferenceCountedSceneGraphMessage message)
    {
        // Process the incoming message from host IF the local peer is not the host or free movement is granted
        if ((handler.isHost == false) || isFreeMovement)
        {
            var data = message.FromJson<MoleculeState>();
            var scale = data.scale;
            var rotation = data.rotation;
            var animationProgress = data.animationProgress;
        
            handler.UpdateMoleculeState(scale, rotation, animationProgress);    
        }

    }
}
