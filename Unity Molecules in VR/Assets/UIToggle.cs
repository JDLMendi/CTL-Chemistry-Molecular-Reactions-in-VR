using UnityEngine;

public class UIToggler : MonoBehaviour
{

    [Header("Main Menu")]
    public GameObject Menu;

    [Header("Toolbar")]
    public GameObject Toolbar;

    void Update()
    {
        if (Menu.activeSelf)
        {
            Toolbar.SetActive(false);
        }
        else
        {
            Toolbar.SetActive(true);
        }
    }
}
