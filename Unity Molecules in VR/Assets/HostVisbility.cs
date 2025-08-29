using System;
using Ubiq.Avatars;
using UnityEngine;

public class HostVisbility : MonoBehaviour
{
    private HostManager hostManager;
    private AvatarManager avatarManager;
    private GameObject avatarManagerObject;

    public void Start()
    {
        hostManager = FindFirstObjectByType<HostManager>();
        avatarManager = FindFirstObjectByType<AvatarManager>();

        if (avatarManager != null)
        {
            avatarManagerObject = avatarManager.gameObject;
        }
    }

    // When called, this should disable the visibility of everyone but the host
    public void DisablePeerVisbility()
    {
        return;
    }

    // When called, this should re-enable visibility of everyone
    public void EnablePeerVisbility()
    {
        return;
    }
}
