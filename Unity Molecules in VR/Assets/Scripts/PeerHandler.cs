using System;
using System.Collections.Generic;
using System.Linq;
using Ubiq.Avatars;
using Ubiq.Rooms;
using UnityEngine;

public class PeerHandler : MonoBehaviour
{
    public GameObject[] peersObjects;
    
    [Header("Host Manager")]
    public HostManager hostManager;

    [Header("Ubiq Avatars")]
    public AvatarManager avatarManager;
    public Dictionary<string, GameObject> peerDict;
    
    [Header("Ubiq Room Client")]
    public RoomClient roomClient;
    
    public void Start()
    {
        peerDict = new Dictionary<string, GameObject>();
        
        roomClient = FindFirstObjectByType<RoomClient>();
        hostManager = FindFirstObjectByType<HostManager>();
        avatarManager = FindFirstObjectByType<AvatarManager>();
        
        // Listens whenever a toggleable has been invokved
        hostManager.onPeerVisible.AddListener(ToggleVisibility);
        
        // Listens to whenever a peer joins or leaves the room
        roomClient.OnPeerAdded.AddListener(AddPeer);
        roomClient.OnPeerRemoved.AddListener(RemovePeer);
    }

    // Finds the GameObject of the joined Peer via their UUID into the Peers List
    private void AddPeer(IPeer peer)
    {
        Transform foundPeer = null;
        foreach (Transform peerTransform in avatarManager.transform)
        {
            if (peerTransform.name.Contains(peer.uuid, StringComparison.OrdinalIgnoreCase))
            {
                foundPeer = peerTransform;
                break;
            }
        }
        
        if (foundPeer != null) 
        {
            peerDict.Add(peer.uuid, foundPeer.gameObject);
        }
    }
    
    // Removes the GameObject of the joined Peer via their UUID from the Peers List

    private void RemovePeer(IPeer peer)
    {
        peerDict.Remove(peer.uuid);
    }

    #region Visibility Functions

    private void ToggleVisibility(bool isVisible)
    {
        if (peerDict == null) return;
        
        var hostID = hostManager.currentHostID;

        foreach (var peer in peerDict.Values)
        {
            Debug.Log(peer.name);
            // Checks if the peer is either the local avatar or the Host depending on the ID, if so then move to the  next peer
            if (peer.name.Contains(hostID)) continue;
            
            TogglePeerVisible(peer, isVisible);
        }
    }

    // Search for all Renderers within a given peer gameobject and toggles base on the 'isVisible'
    private void TogglePeerVisible(GameObject obj, bool isVisible)
    {
        var avatarHandler =  obj.GetComponent<AvatarHandler>();
        
        if (isVisible) avatarHandler.EnableRenderers();
        else  avatarHandler.DisableRenderers();
        
    }

    #endregion
    
    private void Update()
    {
        if (peerDict == null) return;
        peersObjects = peerDict.Values.ToArray();
    }
}
