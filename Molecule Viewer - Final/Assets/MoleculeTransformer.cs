using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRGrabInteractable))]
public class MoleculeTransformer : MonoBehaviour
{

    [Header("Model Data")]
    public float currentAnimationProgress { get; private set; }
    public Vector3 currentScale { get; private set; }
    public Quaternion currentRotation { get; private set; }
    
    [Header("References")]
    public GameObject moleculeModel;

    [Header("Settings")]
    public float minScale = 0.1f;
    public float maxScale = 0.8f;
    
    // XR Grab Interactable
    private XRGrabInteractable _grabInteractable;
    
    // Scaling Variables
    private float _initialDistance;
    private Vector3 _initialScaleOnGrab;
    
    // Rotation Variables
    private Quaternion _initialObjectRotation;
    private Quaternion _initialControllerRotation;
    
    // Animation Variables
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _grabInteractable = GetComponent<XRGrabInteractable>();
        
        if (_animator == null) Debug.LogError("MoleculeTransformer has no animator component");
        
        if (moleculeModel != null)
        {
            currentScale = moleculeModel.transform.localScale;
            currentRotation = moleculeModel.transform.rotation;
        }
    }

    private void Start()
    {
        var characterController = FindFirstObjectByType<CharacterController>();
        var moleculeCollider = GetComponent<Collider>();
        if (characterController != null)
        {
            Physics.IgnoreCollision(moleculeCollider, characterController);
        }

        _grabInteractable.selectEntered.AddListener(OnSelectEntered);
    }

    private void Update()
    {
        if (_grabInteractable.interactorsSelecting.Count == 2)
        {
            HandleScale();
        } 
        else if (_grabInteractable.interactorsSelecting.Count == 1)
        {
            HandleRotation();
        }
        
        if (moleculeModel != null)
        {
            moleculeModel.transform.rotation = currentRotation;
            moleculeModel.transform.localScale = currentScale;
        }
        
        if (_animator != null) _animator.SetFloat("progress", currentAnimationProgress);
    }
    

    public void SetMoleculeTransform(Vector3 scale, Quaternion rotation, float animationProgress)
    {
        float clampedX = Mathf.Clamp(scale.x, minScale, maxScale);
        currentScale = new Vector3(clampedX, clampedX, clampedX);
        currentRotation = rotation;
        currentAnimationProgress = animationProgress;
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (_grabInteractable.interactorsSelecting.Count == 1)
        {
            _initialControllerRotation = args.interactorObject.transform.rotation;
            _initialObjectRotation = moleculeModel.transform.rotation;
        }
        
        if (_grabInteractable.interactorsSelecting.Count == 2)
        {
            _initialDistance = Vector3.Distance(
                _grabInteractable.interactorsSelecting[0].transform.position,
                _grabInteractable.interactorsSelecting[1].transform.position
            );
            _initialScaleOnGrab = moleculeModel.transform.localScale;
        }
    }
    
    
    private void HandleRotation()
    {
        Transform controllerTransform = _grabInteractable.interactorsSelecting[0].transform;
        Quaternion rotationDelta = controllerTransform.rotation * Quaternion.Inverse(_initialControllerRotation);
        currentRotation = rotationDelta * _initialObjectRotation; 
    }
    
    private void HandleScale()
    {
        float currentDistance = Vector3.Distance(
            _grabInteractable.interactorsSelecting[0].transform.position,
            _grabInteractable.interactorsSelecting[1].transform.position
        );

        float scaleFactor = currentDistance / _initialDistance;
        float newScaleValue = _initialScaleOnGrab.x * scaleFactor;
        float clampedScaleValue = Mathf.Clamp(newScaleValue, minScale, maxScale);
        
        currentScale = new Vector3(clampedScaleValue, clampedScaleValue, clampedScaleValue);
    }

    public void ToggleGrabbable(bool toggle)
    {
        _grabInteractable.enabled = toggle;
    }
}