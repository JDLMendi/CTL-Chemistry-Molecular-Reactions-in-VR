using UnityEngine;

public class AvatarHandler : MonoBehaviour
{
    public Renderer[] renderers;

    public void DisableRenderers()
    {
        foreach (Renderer r in renderers)
        {
                r.enabled = false;
        }
    }

    public void EnableRenderers()
    {
        foreach (Renderer r in renderers)
        {
            r.enabled = true;
        }
    }
}
