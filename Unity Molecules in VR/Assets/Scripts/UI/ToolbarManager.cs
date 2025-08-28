using System;
using UnityEngine;

public class ToolbarManager : MonoBehaviour
{
    public MoleculeHandler handler;
    private AnimationManager animationManager;

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
}
