using Ubiq.Rooms;
using UnityEngine;

public class ChildRendererToggler : MonoBehaviour
{
    // Public field to set the key for the host ID in the Room properties.
    [Tooltip("The key used in the Room properties to identify the host's ID.")]
    public string hostID;

    private string currentHostId;
    private bool renderersAreEnabled = true;
    private RoomClient roomClient;

    private void Start()
    {
        roomClient = FindFirstObjectByType<RoomClient>();
        if (roomClient == null)
        {
            Debug.LogError("Ubiq RoomClient not found in the scene. ChildRendererToggler requires it to function.", this);
            // Disable this component if the RoomClient is missing to prevent errors.
            this.enabled = false;
            return;
        }

        // Subscribe to Room property updates to stay in sync.
        roomClient.OnRoomUpdated.AddListener(OnRoomUpdated);
        // Also call it once at the start to get the initial value.
        UpdateHostId();

        // Optionally disable non-essential renderers on start.
        DisableRenderers();
    }

    private void OnDestroy()
    {
        // Always unsubscribe from events when the object is destroyed to prevent errors.
        if (roomClient != null)
        {
            roomClient.OnRoomUpdated.RemoveListener(OnRoomUpdated);
        }
    }

    // This method is called automatically by the RoomClient when room properties change.
    private void OnRoomUpdated(IRoom room)
    {
        UpdateHostId();
        DisableRenderers();
    }

    // A helper method to safely get the host ID from the room properties.
    private void UpdateHostId()
    {
        if (roomClient.Room[hostID] != null)
        {
            // Safely get the value and convert it to a string.
            currentHostId = roomClient.Room[hostID];
        }
    }

    /// <summary>
    /// This is the main public function that the Inspector button will call.
    /// It checks the current state and calls the appropriate function.
    /// </summary>
    public void ToggleAllChildRenderers()
    {
        if (renderersAreEnabled)
        {
            DisableRenderers();
        }
        else
        {
            EnableRenderers();
        }
        // After the action is performed, invert the state for the next button press.
        renderersAreEnabled = !renderersAreEnabled;
    }

    /// <summary>
    /// Finds all Renderer components in children (except excluded ones) and enables them.
    /// </summary>
    public void EnableRenderers()
    {
        Debug.Log("Enabling renderers on children...");
        SetRenderersState(true);
    }

    /// <summary>
    /// Finds all Renderer components in children (except excluded ones) and disables them.
    /// </summary>
    public void DisableRenderers()
    {
        Debug.Log("Disabling renderers on children...");
        SetRenderersState(false);
    }

    /// <summary>
    /// The core logic that iterates through children and sets the enabled state of their renderers.
    /// </summary>
    /// <param name="state">The state to set the renderers to (true for enabled, false for disabled).</param>
    private void SetRenderersState(bool state)
    {
        // Loop through each immediate child of this GameObject.
        foreach (Transform child in transform)
        {
            // Condition 1: Is the child the "Local Avatar"?
            bool isLocalAvatar = child.name == "Local Avatar";

            // Condition 2: Is the child the host's avatar?
            // This check is safer: it won't crash if currentHostId is null or empty.
            bool isHostAvatar = !string.IsNullOrEmpty(currentHostId) && child.name.Contains(currentHostId);

            // If either condition is true, we want to skip this child and leave its renderers alone.
            if (isLocalAvatar || isHostAvatar)
            {
                continue; // Skips to the next child in the foreach loop.
            }

            // If we get here, it means the child is NOT one of the excluded avatars.
            // So we can safely apply the state change.
            Renderer[] renderersInChildren = child.GetComponentsInChildren<Renderer>();
            foreach (Renderer rend in renderersInChildren)
            {
                rend.enabled = state;
            }
        }
        Debug.Log("Operation complete.");
    }
}

