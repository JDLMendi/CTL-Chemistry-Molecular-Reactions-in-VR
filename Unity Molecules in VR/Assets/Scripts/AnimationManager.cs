using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationManager : MonoBehaviour
{
    [Header("Component References")]
    [Tooltip("The Animator component to control.")]
    public Animator anim;

    [Header("Animation Control")]
    [Tooltip("How fast the animation scrubs or plays.")]
    public float animationSpeed = 0.5f;

    [Header("Debug Info")]
    [Tooltip("The current normalized progress of the animation (0.0 to 1.0).")]
    [Range(0.0f, 0.99f)]
    public float animationProgress;
    [Tooltip("Is the animation set to loop when it finishes?")]
    public bool isLooping = false;
    
    // Flags for manual input control
    private bool isMovingForward = false;
    private bool isMovingBackward = false;
    
    // Flag for automatic playback control
    private bool isPlaying = false;

    private void Start()
    {
        if (anim == null)
        {
             Debug.LogError("Animator component is not assigned in the Inspector!", this);
             return;
        }

        // Initialize the animation at the beginning
        SetAnimationProgress(0f);
    }

    private void Update()
    {
        // Prioritize automatic playback if 'isPlaying' is true.
        // Otherwise, respond to manual scrubbing input.
        if (isPlaying || isMovingForward)
        {
            animationProgress += animationSpeed * Time.deltaTime;
        }
        else if (isMovingBackward)
        {
            animationProgress -= animationSpeed * Time.deltaTime;
        }
        
        // If the animation is auto-playing and reaches the end...
        if (isPlaying && animationProgress >= 0.99f)
        {
            if (isLooping)
            {
                // If looping is on, reset the progress to the beginning.
                animationProgress = 0f;
            }
            else
            {
                // Otherwise, just pause the playback.
                isPlaying = false;
            }
        }
        
        // Apply the updated progress to the Animator
        SetAnimationProgress(animationProgress);
    }
    
    /// <summary>
    /// Sets the normalized progress of the animation and updates the Animator.
    /// </summary>
    /// <param name="newProgress">The new animation progress to set.</param>
    public void SetAnimationProgress(float newProgress)
    {
        // Clamp the value to ensure it stays within the valid range
        animationProgress = Mathf.Clamp(newProgress, 0.0f, 0.99f);
        
        if (anim != null)
        {
            anim.SetFloat("progress", animationProgress);
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
    
    #region Toolbar Buttons
    
    /// <summary>
    /// Resets the animation to the beginning and starts automatic playback.
    /// </summary>
    public void PlayAgain()
    {
        animationProgress = 0f;
        isPlaying = true;
    }
    
    /// <summary>
    /// Toggles the automatic playback state (pause/resume).
    /// </summary>
    public void Pause()
    {
        isPlaying = !isPlaying;
    }

    /// <summary>
    /// Toggles the looping behavior for when the animation finishes playing.
    /// </summary>
    public void ToggleLoop()
    {
        isLooping = !isLooping;
    }
    #endregion
}