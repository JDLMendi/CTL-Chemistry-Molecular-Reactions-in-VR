using UnityEngine;

public class SocialMenuVisibility : MonoBehaviour
{
    public bool isToggled = true;

    public void ToggleSocialMenu()
    {
        gameObject.SetActive(isToggled);
        isToggled = !isToggled;
    }
}
