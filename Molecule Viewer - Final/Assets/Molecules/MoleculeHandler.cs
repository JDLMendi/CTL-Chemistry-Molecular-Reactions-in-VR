using System;
using Ubiq.Rooms;
using Ubiq.Spawning;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoleculeHandler : MonoBehaviour
{
    public Transform spawnPoint;
    public ModelSwapper modelSwapper;
    public NetworkSpawnManager  networkSpawnManager;
    public HostManager hostManager;
    public AnimationHandler animationHandler;
    
    [Header("Messaging Data")]
    public bool isHost;
    
    [Header("Scaling Control")]
    [Tooltip("How fast the model scales when a button is held.")]
    public float scaleSpeed = 0.25f;
    
    [Header("Molecule Data")]
    public GameObject currentMolecule;
    public MoleculeTransformer currentMoleculeTransformer;
    public Animator currentAnimator;

    public float currentAnimationProgress;
    public Quaternion currentRotation;
    public Vector3 currentScale;

    
    private void Awake()
    {
        networkSpawnManager = FindFirstObjectByType<NetworkSpawnManager>();
        hostManager = FindFirstObjectByType<HostManager>();
        animationHandler = FindFirstObjectByType<AnimationHandler>();
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
            
            // --- Scaling Logic ---
            Vector3 newScale = currentScale;
        
            if (isScalingUp)
            {
                newScale += new Vector3(scaleSpeed, scaleSpeed, scaleSpeed) * Time.deltaTime;
            }
            else if (isScalingDown)
            {
                newScale -= new Vector3(scaleSpeed, scaleSpeed, scaleSpeed) * Time.deltaTime;
                newScale = Vector3.Max(newScale, Vector3.zero);
            }
        
            if (newScale != currentScale)
            {
                UpdateMoleculeTransform(newScale);
            }
            
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
        currentAnimator =  currentMolecule.GetComponentInChildren<Animator>();
        currentAnimationProgress = 0.0f;
        currentMoleculeTransformer.SetMoleculeAnimation(currentAnimationProgress);
    }

    public void SpawnMolecule(int moleculeIndex)
    {
        if (networkSpawnManager)
        {
            networkSpawnManager.Despawn(currentMolecule);
            networkSpawnManager.SpawnWithRoomScope(networkSpawnManager.catalogue.prefabs[moleculeIndex]);
        }
    }

    public void UpdateMoleculeTransform(Vector3 scale, Quaternion rotation)
    {
        if (currentMoleculeTransformer)
        {
            currentMoleculeTransformer.SetMoleculeTransform(scale, rotation);
        }
    }

    public void UpdateMoleculeTransform(Vector3 scale)
    {
        if (currentMoleculeTransformer)
        {
            currentMoleculeTransformer.SetMoleculeTransform(scale, currentRotation);
        }
    }
    
    public void UpdateMoleculeTransform(Quaternion rotation)
    {
        if (currentMoleculeTransformer)
        {
            currentMoleculeTransformer.SetMoleculeTransform(currentScale, rotation);
        }
    }

    public void UpdateMoleculeAnimation(float animationProgress)
    {
        if (currentMoleculeTransformer)
        {
            currentMoleculeTransformer.SetMoleculeAnimation(animationProgress);
        }
    }

    public void UpdateMoleculeState(Vector3 scale, Quaternion rotation, float animationProgress)
    {
        if (currentMoleculeTransformer)
        {
            currentMoleculeTransformer.SetMoleculeState(scale, rotation, animationProgress);
        }
    } 
    
    
    
    #region InputActions
    
    private bool isScalingUp = false;
    private bool isScalingDown = false;
    public void ScaleUpPressed(InputAction.CallbackContext context)
    {
        if (context.performed) isScalingUp = true;
        if (context.canceled) isScalingUp = false;
    }

    public void ScaleDownPressed(InputAction.CallbackContext context)
    {
        if (context.performed) isScalingDown = true;
        if (context.canceled) isScalingDown = false;
    }

    #endregion
}

