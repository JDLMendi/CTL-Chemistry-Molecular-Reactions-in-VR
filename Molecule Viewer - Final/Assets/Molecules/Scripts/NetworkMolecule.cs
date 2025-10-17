using Ubiq.Messaging;
using Ubiq.Rooms;
using UnityEngine;
using UnityEngine.UI;

public class NetworkMolecule : MonoBehaviour
{
    public GameObject toolbar;
    public Button swapperButton;
    
    [Header("Molecule Room Property")] 
    public string moleculeIndex = "Molecule Index";

    public int currentIndex;
    
    [Header("Ubiq")]
    public RoomClient roomClient;

    [Header("Molecule Handler")]
    public bool isFreeMovement;
    public MoleculeHandler handler;
    
    private NetworkContext context;
    
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
        int.TryParse(roomClient.Room[moleculeIndex], out currentIndex);
        
        // Only send the message if the peer is host and that free movement is disabled
        if (handler.isHost && isFreeMovement == false)
        {
            SendMoleculeState();
        }
        
        if (handler.currentMoleculeTransformer == null) return;

        if (isFreeMovement || handler.isHost)
        {
            ToggleFreeMovement(true);
        }
        else
        {
            ToggleFreeMovement(false);
        }
    }

    private void ToggleFreeMovement(bool toggle)
    {
        var toolbarManager = toolbar.GetComponent<ToolbarManager>();
        
        handler.currentMoleculeTransformer.ToggleGrabbable(toggle);
        handler.animationHandler.enabled = toggle;
        toolbar.SetActive(toggle);

        swapperButton.interactable = handler.isHost != false;
        
        if (toggle == false)
        {
            Debug.Log("Toolbar Reset");
            swapperButton.interactable = true;
            toolbarManager.ResetToolbar();
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
