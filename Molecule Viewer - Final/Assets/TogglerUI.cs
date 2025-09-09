using System;
using UnityEngine;
using UnityEngine.UI;

public class TogglerUI : MonoBehaviour
{
    public Image icon;
    public Sprite activeSprite;
    public Sprite inactiveSprite;
    
    public bool isActive;
    private bool _initialToggle;

    private void Start()
    {
        if (isActive)
        {
            icon.sprite = activeSprite;
        }
        else
        {
            icon.sprite = inactiveSprite;
        }
        
        _initialToggle = isActive;
    }
    
    public void Toggle()
    {
        isActive = !isActive;
        ToggleIcons();
    }

    private void ToggleIcons()
    {
        if (isActive)
        {
            icon.sprite = activeSprite;
        }
        else
        {
            icon.sprite = inactiveSprite;
        }
    }

    public void ResetToggle()
    {
        isActive = _initialToggle;
        ToggleIcons();
    }
}
