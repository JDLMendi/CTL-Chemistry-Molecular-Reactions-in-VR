using System;
using UnityEngine;

public class RotationProperty : MonoBehaviour
{
    public Quaternion rotation;
    public ViewManager viewManager;

    private void Start()
    {
        viewManager = FindFirstObjectByType<ViewManager>();
    }

    public void UpdateAngle()
    {
        viewManager.UpdateAngle(rotation);
    }
}
