using System;
using System.Threading;
using UnityEngine;

public class VisbilityHandler : MonoBehaviour
{
    public PeerManager peerManager;
    private bool _areRenderersVisible = true;
    private void Awake()
    {
        peerManager = FindFirstObjectByType<PeerManager>();
    }

    public void ToggleVisibilty(bool isVisible)
    {
        // Invert the visibility state for debugging.
        _areRenderersVisible = isVisible;

        // Get all AvatarRenderer components attached to this object or its children.
        AvatarRenderer[] allRenderers = GetComponentsInChildren<AvatarRenderer>();

        // Loop through each found AvatarRenderer.
        foreach (AvatarRenderer avatarRenderer in allRenderers)
        {
            // Check for null references and if the name contains the host ID.
            // Using string.IsNullOrEmpty for the host ID is a good safety check.
            bool isHostAvatar = peerManager != null &&
                                !string.IsNullOrEmpty(peerManager.currentHostID) &&
                                avatarRenderer.gameObject.name.Contains(peerManager.currentHostID);

            // If the GameObject's name matches one of the exclusion criteria, skip it.
            if (isHostAvatar)
            {
                continue;
            }

            // Call the function on the component with the new visibility state.
            avatarRenderer.ToggleRenderers(isVisible);
        }
    }
}
