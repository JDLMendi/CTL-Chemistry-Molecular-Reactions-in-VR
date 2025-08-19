using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class MoleculeManagerMain : MonoBehaviour
{
    [Header("Object References")]
    [Tooltip("The GameObject containing the Model (fbx) and Animator.")]
    public GameObject model;

    [Header("Animation Control")]
    [Tooltip("How fast the animation scrubs when a button is held.")]
    public float animationSpeed = 0.5f;

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
    
    private Animator anim;
    private XRGrabInteractable grabInteractable;

    private bool isMovingForward = false;
    private bool isMovingBackward = false;
    private bool isScalingUp = false;
    private bool isScalingDown = false;

    private void Start()
    {
        if (model == null)
        {
            Debug.LogError("Model GameObject is not assigned in the Inspector!", this);
            return;
        }
        
        anim = model.GetComponent<Animator>();
        grabInteractable = model.GetComponent<XRGrabInteractable>();

        if (anim == null)
        {
             Debug.LogError("No Animator component found on the model!", this);
             return;
        }

        anim.SetFloat("progress", 0f);
        currentScale = model.transform.localScale;
    }

    private void Update()
    {
        if (isMovingForward)
        {
            animationProgress += animationSpeed * Time.deltaTime;
        }
        else if (isMovingBackward)
        {
            animationProgress -= animationSpeed * Time.deltaTime;
        }
        
        animationProgress = Mathf.Clamp(animationProgress, 0.0f, 0.99f);
        anim.SetFloat("progress", animationProgress);
        
        float scaleValue = model.transform.localScale.x;

        if (isScalingUp)
        {
            scaleValue += scaleSpeed * Time.deltaTime;
        }
        else if (isScalingDown)
        {
            scaleValue -= scaleSpeed * Time.deltaTime;
        }
        
        scaleValue = Mathf.Clamp(scaleValue, minScale, maxScale);
        
        model.transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);

        currentRotation = model.transform.rotation;
        currentScale = model.transform.localScale;
    }

    #region InputActions

    public void ForwardAnimationPressed(InputAction.CallbackContext context)
    {
        Debug.Log("Forward animation pressed");
        if (context.performed) isMovingForward = true;
        if (context.canceled) isMovingForward = false;
    }

    public void BackwardAnimationPressed(InputAction.CallbackContext context)
    {
        Debug.Log("Backward animation pressed");
        if (context.performed) isMovingBackward = true;
        if (context.canceled) isMovingBackward = false;
    }

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

    // Function used by network to force the transformation and progress onto the remote objects
    public void UpdateMoleculeState(float newAnimationProgress, Vector3 newScale, Quaternion newRotation)
    {
        // Set Animation
        animationProgress = Mathf.Clamp(newAnimationProgress, 0.0f, 0.99f);
        if (anim != null)
        {
            anim.SetFloat("progress", animationProgress);
        }

        // Set Scale
        newScale.x = Mathf.Clamp(newScale.x, minScale, maxScale);
        newScale.y = Mathf.Clamp(newScale.y, minScale, maxScale);
        newScale.z = Mathf.Clamp(newScale.z, minScale, maxScale);
        if (model != null)
        {
            model.transform.localScale = newScale;
        }
        currentScale = model.transform.localScale;

        // Set Rotation
        if (model != null)
        {
            model.transform.rotation = newRotation;
        }
        currentRotation = model.transform.rotation;
    }
    
}