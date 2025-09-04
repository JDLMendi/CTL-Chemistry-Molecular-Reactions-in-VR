using UnityEngine;

public class AvatarRenderer : MonoBehaviour
{
    public Renderer[]  renderers;

    public void ToggleRenderers(bool isEnabled)
    {
        foreach (Renderer r in renderers)
            {
                r.enabled = isEnabled;
            }
    }
}
