using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SocialMenuManager : MonoBehaviour
{
    public GameObject socialMenu;
    private bool menuActive = false;

    private void Start()
    {
        DisplayMenuUI();
    }

    public void MenuButtonPressed(InputAction.CallbackContext context)
    {
        if (context.performed) DisplayMenuUI();
    }

    private void DisplayMenuUI()
    {
        if (!menuActive)
        {
            socialMenu.SetActive(true);
            menuActive = true;
        } else {
            socialMenu.SetActive(false);
            menuActive = false;
        }
    }
}
