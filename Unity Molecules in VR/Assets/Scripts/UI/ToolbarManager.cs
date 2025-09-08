using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Add this namespace for event handling

public class ToolbarManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public MoleculeHandler handler;
    public Image loop_button;
    private AnimationManager animationManager;

    [Header("Visibility Settings")]
    public CanvasGroup toolbarCanvasGroup;
    public float defaultAlpha = 0.1f;
    public float hoverAlpha = 1f;
    public float fadeDuration = 0.3f;

    private bool isHovering = false;
    private float currentFadeTime = 0f;

    public bool isLooping = false;

    private void Start() {
        animationManager = handler.animationManager;

        // Initialize canvas group if not set
        if (toolbarCanvasGroup == null)
        {
            toolbarCanvasGroup = GetComponent<CanvasGroup>();
            if (toolbarCanvasGroup == null)
            {
                toolbarCanvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
        }

        // Set initial alpha
        toolbarCanvasGroup.alpha = defaultAlpha;
    }

    private void Update()
    {
        // Smoothly fade between alpha values
        if (isHovering && toolbarCanvasGroup.alpha < hoverAlpha)
        {
            currentFadeTime += Time.deltaTime;
            float t = Mathf.Clamp01(currentFadeTime / fadeDuration);
            toolbarCanvasGroup.alpha = Mathf.Lerp(defaultAlpha, hoverAlpha, t);
        }
        else if (!isHovering && toolbarCanvasGroup.alpha > defaultAlpha)
        {
            currentFadeTime += Time.deltaTime;
            float t = Mathf.Clamp01(currentFadeTime / fadeDuration);
            toolbarCanvasGroup.alpha = Mathf.Lerp(hoverAlpha, defaultAlpha, t);
        }
        else
        {
            currentFadeTime = 0f;
        }
    }

    // IPointerEnterHandler implementation for hover detection
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        currentFadeTime = 0f;
    }

    // IPointerExitHandler implementation for hover exit detection
    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        currentFadeTime = 0f;
    }

    public void Play() {
        animationManager.PlayAgain();
    }

    public void Pause() {
        animationManager.Pause();
    }

    public void ToggleLoop() {
        animationManager.ToggleLoop();
        if(animationManager.isLooping) {
            loop_button.color = new Color32(80, 255, 255, 255);
        } else {
            loop_button.color = new Color32(255, 255, 255, 255);
        }
    }

    public void NextFrame() {
        var _progress = animationManager.animationProgress;
        _progress += 0.03f;
        _progress = Mathf.Clamp(_progress, 0f, 0.99f);
        animationManager.animationProgress = _progress;
    }
    public void PrevFrame() {
        var _progress = animationManager.animationProgress;
        _progress -= 0.03f;
        _progress = Mathf.Clamp(_progress, 0f, 0.99f);
        animationManager.animationProgress = _progress;
    }

    public void RestartToolBar()
    {
        // Reset the isLooping flag
        if (animationManager.isLooping)
        {
            animationManager.isLooping = false;
            loop_button.color = new Color32(255, 255, 255, 255);
        }
        
        // Reset the Pause flag

    }

    public void EnableVisibility()
    {
        // You can use this method to force visibility if needed
        isHovering = true;
        currentFadeTime = 0f;
        toolbarCanvasGroup.alpha = hoverAlpha;
    }

    // Optional: Method to disable visibility
    public void DisableVisibility()
    {
        isHovering = false;
        currentFadeTime = 0f;
        toolbarCanvasGroup.alpha = defaultAlpha;
    }
}
