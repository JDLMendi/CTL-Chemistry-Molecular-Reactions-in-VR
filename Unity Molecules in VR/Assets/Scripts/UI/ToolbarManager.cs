using System;
using UnityEngine;
using UnityEngine.UI;

public class ToolbarManager : MonoBehaviour
{
    public MoleculeHandler handler;
    public Image loop_button;
    private AnimationManager animationManager;

    public bool isLooping = false;

    private void Start() {
        animationManager = handler.animationManager;
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
        _progress += 0.01f;
        _progress = Mathf.Clamp(_progress, 0f, 0.99f);
        animationManager.animationProgress = _progress;
    }
    public void PrevFrame() {
        var _progress = animationManager.animationProgress;
        _progress -= 0.01f;
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
}
