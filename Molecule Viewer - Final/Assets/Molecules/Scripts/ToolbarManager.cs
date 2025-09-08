using UnityEngine;

public class ToolbarManager : MonoBehaviour
{

    [Header("Social Menu")]
    public GameObject socialMenu;

    [Header("Toolbar Canvas")]
    public GameObject toolbarCanvas;
    
    void Update()
    {
        if (socialMenu.activeSelf)
        {
            toolbarCanvas.SetActive(false);
        }
        else
        {
            toolbarCanvas.SetActive(true);
        }
    }
}
