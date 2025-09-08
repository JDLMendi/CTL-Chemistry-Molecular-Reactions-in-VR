using UnityEngine;

public class HostTools : MonoBehaviour
{
    public HostManager hostManager;
    private void Start()
    {
        hostManager = FindFirstObjectByType<HostManager>();
        hostManager.toggleHost.AddListener(HostManager_ToggleHost);
    }

    private void HostManager_ToggleHost(bool toggle)
    {   
        Debug.Log(gameObject.name + " toggled: " + toggle);
        gameObject.SetActive(toggle);
    }
}
