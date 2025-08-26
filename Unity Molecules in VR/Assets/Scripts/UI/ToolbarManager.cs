using System;
using UnityEngine;

public class ToolbarManager : MonoBehaviour
{
    public MoleculeHandler handler;
    private AnimationManager animationManager;

    private void Start()
    {
        animationManager = handler.animationManager;
    }

    public void Play()
    {
        animationManager.PlayAgain();
    }

    public void Pause()
    {
        animationManager.Pause();
    }

    public void ToggleLoop()
    {
        animationManager.ToggleLoop();
    }
}
