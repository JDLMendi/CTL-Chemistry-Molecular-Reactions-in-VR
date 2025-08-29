using System;
using UnityEngine;

public class HostVisbility : MonoBehaviour
{
    [SerializeField] private string hostUuid;
    private NetworkManager networkManager;
    private void Start()
    {
        networkManager = FindObjectOfType<NetworkManager>();
    }

    private void Update()
    {
        hostUuid = networkManager.currentHostUuid;
    }
}

