using UnityEngine;

public class ChemAnimationControl : MonoBehaviour
{
    private Animator anim;
    public float animProgress;

    public Vector3 scale_change;
    public Vector3 curr_scale;

    void Start()
    {
        anim = GetComponent<Animator>();
        animProgress = 0f;

        scale_change = new Vector3(0.01f, 0.01f, 0.01f);
        curr_scale = transform.localScale;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.X)) AdjustAnimProgress(-0.0025f);
        if (Input.GetKey(KeyCode.C)) AdjustAnimProgress(0.0025f);
        if (Input.GetKey(KeyCode.Q)) AdjustScale(-scale_change);
        if (Input.GetKey(KeyCode.E)) AdjustScale(scale_change);

        curr_scale = transform.localScale;
        curr_scale.x = Mathf.Clamp(curr_scale.x, 0f, 4f);
        curr_scale.y = Mathf.Clamp(curr_scale.y, 0f, 4f);
        curr_scale.z = Mathf.Clamp(curr_scale.z, 0f, 4f);
        transform.localScale = curr_scale;

        animProgress = Mathf.Clamp(animProgress, 0f, 0.99f);
        anim.SetFloat("progress", animProgress);
    }

    public void AdjustScale(Vector3 scale)
    {
        transform.localScale += scale;
    }
    
    public void AdjustAnimProgress(float progress)
    {
        animProgress += progress;
    }
}
