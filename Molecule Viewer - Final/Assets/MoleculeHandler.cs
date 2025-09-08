using System;
using Ubiq.Rooms;
using Ubiq.Spawning;
using UnityEngine;

public class MoleculeHandler : MonoBehaviour
{
    public Transform spawnPoint;
    public ModelSwapper modelSwapper;
    public NetworkSpawnManager  networkSpawnManager;
    public HostManager hostManager;
    
    [Header("Messaging Data")]
    public bool isHost;
    
    [Header("Molecule Data")]
    public GameObject currentMolecule;
    public MoleculeTransformer currentMoleculeTransformer;

    public float currentAnimationProgress;
    public Quaternion currentRotation;
    public Vector3 currentScale;

    
    private void Awake()
    {
        networkSpawnManager = FindFirstObjectByType<NetworkSpawnManager>();
        hostManager = FindFirstObjectByType<HostManager>();
    }

    private void Start()
    {
        modelSwapper.onModelLoaded.AddListener(SpawnMolecule);
        networkSpawnManager.OnSpawned.AddListener(SpawnManager_OnSpawned);
        hostManager.toggleHost.AddListener(HostManager_ToggleHost);
    }

    private void Update()
    {
        if (currentMoleculeTransformer)
        {
            currentRotation = currentMoleculeTransformer.currentRotation;
            currentScale = currentMoleculeTransformer.currentScale;
            currentAnimationProgress = currentMoleculeTransformer.currentAnimationProgress;
        }
    }

    public void HostManager_ToggleHost(bool toggle)
    {
        isHost = toggle;
    }
    
    public void SpawnManager_OnSpawned(GameObject obj, IRoom room, IPeer peer, NetworkSpawnOrigin origin)
    {
        spawnPoint.GetPositionAndRotation(out var pos, out var rot);
        obj.transform.SetPositionAndRotation(pos, rot);
        currentMolecule = obj;
        currentMoleculeTransformer = obj.GetComponent<MoleculeTransformer>();
    }

    public void SpawnMolecule(int moleculeIndex)
    {
        if (networkSpawnManager)
        {
            networkSpawnManager.Despawn(currentMolecule);
            networkSpawnManager.SpawnWithRoomScope(networkSpawnManager.catalogue.prefabs[moleculeIndex]);
        }
    }

    public void UpdateMoleculeState(Vector3 scale, Quaternion rotation, float animationProgress)
    {
        if (currentMoleculeTransformer)
        {
            currentMoleculeTransformer.SetMoleculeTransform(scale, rotation, animationProgress);
        }
    }
}
