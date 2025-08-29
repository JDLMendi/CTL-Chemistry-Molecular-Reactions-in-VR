using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class MoleculeHandler : MonoBehaviour
{
    [Header("Manager References")]
    [Tooltip("Reference to the script that handles animation logic.")]
    public AnimationManager animationManager;
    
    [Header("Object References")]
    [Tooltip("The GameObject(s) containing the Model (fbx) and Animator.")]
    public GameObject[] models;
    public int model_index;

    [Header("Scaling Control")]
    [Tooltip("How fast the model scales when a button is held.")]
    public float scaleSpeed = 0.25f;
    [Tooltip("The minimum uniform scale for the model.")]
    public float minScale = 0.5f;
    [Tooltip("The maximum uniform scale for the model.")]
    public float maxScale = 2.0f;

    [Header("Debug Info")]
    public Vector3 currentScale;
    public Quaternion currentRotation;
    public float animationProgress;
    
    private XRGrabInteractable grabInteractable;

    private bool isScalingUp = false;
    private bool isScalingDown = false;

    private void Start()
    {
        model_index = 0;

        if (models == null)
        {
            Debug.LogError("Model GameObject is not assigned in the Inspector!", this);
            return;
        }
        
        if (animationManager == null)
        {
            Debug.LogWarning("AnimationManager is not assigned. Animation controls will not work.", this);
        }

        grabInteractable = models[0].GetComponent<XRGrabInteractable>();
        currentScale = models[0].transform.localScale;
    }

    private void Update()
    {
        if (model == null) return;
        
        // --- Scaling Logic ---
        float scaleValue = models[model_index].transform.localScale.x;

        if (isScalingUp)
        {
            scaleValue += scaleSpeed * Time.deltaTime;
        }
        else if (isScalingDown)
        {
            scaleValue -= scaleSpeed * Time.deltaTime;
        }
        
        scaleValue = Mathf.Clamp(scaleValue, minScale, maxScale);
        models[model_index].transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);

        // Update debug info
        currentRotation = models[model_index].transform.rotation;
        currentScale = models[model_index].transform.localScale;
        animationProgress = animationManager.animationProgress;
    }

    #region InputActions

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

    /// <summary>
    /// Function used by the network to force the transformation and progress onto remote objects.
    /// </summary>
    public void UpdateMoleculeState(float newAnimationProgress, Vector3 newScale, Quaternion newRotation)
    {
        // Set Animation by delegating to the AnimationManager
        if (animationManager != null)
        {
            animationManager.SetAnimationProgress(newAnimationProgress);
        }

        // Set Scale
        newScale.x = Mathf.Clamp(newScale.x, minScale, maxScale);
        newScale.y = Mathf.Clamp(newScale.y, minScale, maxScale);
        newScale.z = Mathf.Clamp(newScale.z, minScale, maxScale);
        if (models != null)
        {
            models[model_index].transform.localScale = newScale;
        }
        currentScale = models[model_index].transform.localScale;

        // Set Rotation
        if (models != null)
        {
            models[model_index].transform.rotation = newRotation;
        }
        currentRotation = models[model_index].transform.rotation;
    }

    public void ZoomIn() {
        UpdateMoleculeState(animationProgress, currentScale + new Vector3(0.05f, 0.05f, 0.05f), currentRotation);
    }
    public void ZoomOut() {
        UpdateMoleculeState(animationProgress, currentScale - new Vector3(0.05f, 0.05f, 0.05f), currentRotation);
    }
}