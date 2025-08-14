using UnityEngine;

public class MoleculeController : MonoBehaviour
{
    [Header("Configuration")]
    public GameObject model;

    [Header("Animator Control")]
    [Range(0f, 0.99f)]
    public float animProgress = 0f;
    
    private Animator animator;
    
    [Header("Scale Control")]
    public Vector3 currentScaleVector;

    [Header("Rotation Control")]
    public Quaternion currRotation;
    
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        currentScaleVector = model.transform.localScale;
        currRotation = transform.rotation;
    }

    public void UpdateMolecule(Quaternion rotation, Vector3 scale, float remoteAnimProgress)
    {
        UpdateRotation(rotation);
        UpdateScale(scale);
        UpdateAnimation(remoteAnimProgress);
    }

    void UpdateAnimation(float progress)
    {
        this.animProgress = progress;
        animator.SetFloat("progress", this.animProgress);
    }

    void UpdateRotation(Quaternion rotation)
    {
        this.currRotation = rotation;
        transform.rotation = this.currRotation;
    }

    void UpdateScale(Vector3 scale)
    {
        scale.x = Mathf.Clamp(scale.x, 0, 2);
        scale.y = Mathf.Clamp(scale.y, 0, 2);
        scale.z = Mathf.Clamp(scale.z, 0, 2);
        this.currentScaleVector = scale;
        model.transform.localScale = this.currentScaleVector;
    }
}