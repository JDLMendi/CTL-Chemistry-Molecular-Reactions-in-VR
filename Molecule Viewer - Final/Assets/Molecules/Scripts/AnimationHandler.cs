using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationHandler : MonoBehaviour
{
    public MoleculeHandler  handler;
    
    [Header("Animation Control")]
    [Tooltip("How fast the animation scrubs or plays.")]
    public float animationSpeed = 0.5f;
    public float stepLength = 0.3f;
    public bool isPlaying = false;
    public bool isLooping = false;
    public float animationProgress;
    
    
    [Header("Debug Info")]
    [Tooltip("The current normalized progress of the animation (0.0 to 1.0).")]
    [Range(0.0f, 0.99f)]
    
    // Flags for manual input control
    private bool isMovingForward = false;
    private bool isMovingBackward = false;
    
    // Flag for automatic playback control
    
    private void Start()
    {
        handler = FindObjectOfType<MoleculeHandler>();
    }

    private void Update()
    {
        if (isPlaying || isMovingForward)
        {
            animationProgress += animationSpeed * Time.deltaTime;
        }
        else if (isMovingBackward)
        {
            animationProgress -= animationSpeed * Time.deltaTime;
        }
        
        if (isPlaying && animationProgress >= 0.99f)
        {
            if (isLooping)
            {
                // If looping is on, reset the progress to the beginning.
                animationProgress = 0.0f;
            }
            else
            {
                // Otherwise, just pause the playback.
                isPlaying = false;
            }
        }
        
        SetAnimationProgress(animationProgress);
    }

    public void SetAnimationProgress(float newProgress)
    {
        // Clamp the value to ensure it stays within the valid range
        animationProgress = Mathf.Clamp(newProgress, 0.0f, 0.99f);
        
        if (handler.currentAnimator != null)
        {
            handler.UpdateMoleculeAnimation(animationProgress);
            handler.currentMoleculeTransformer.SetMoleculeAnimation(animationProgress);
        }
    }
    
    #region InputActions

    public void ForwardAnimationPressed(InputAction.CallbackContext context)
    {
        // When manual controls are used, disable automatic playback
        if (context.performed)
        {
            isMovingForward = true;
            isPlaying = false;
        }

        if (context.canceled)
        {
            isMovingForward = false;
        }
    }

    public void BackwardAnimationPressed(InputAction.CallbackContext context)
    {
        // When manual controls are used, disable automatic playback
        if (context.performed)
        {
            isMovingBackward = true;
            isPlaying = false;
        }
        
        if (context.canceled)
        {
            isMovingBackward = false;
        }
    }

    #endregion
}
