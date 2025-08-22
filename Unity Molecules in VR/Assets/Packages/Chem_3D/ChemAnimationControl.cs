using UnityEngine;

public class ChemAnimationControl : MonoBehaviour
{
    public Animator anim;
    public float animProgress;

    private Vector3 scale_change;
    private Vector3 curr_scale;

    void Start()
    {
        anim = GetComponent<Animator>();
        animProgress = 0f;

        scale_change = new Vector3(0.01f, 0.01f, 0.01f);
        curr_scale = transform.localScale;
    }


    void Update()
    {
        if (Input.GetKey(KeyCode.A)) animProgress -= 0.0025f;
        if (Input.GetKey(KeyCode.D)) animProgress += 0.0025f;
        if (Input.GetKey(KeyCode.Q)) transform.localScale = transform.localScale - scale_change;
        if (Input.GetKey(KeyCode.E)) transform.localScale = transform.localScale + scale_change;

        curr_scale = transform.localScale;
        curr_scale.x = Mathf.Clamp(curr_scale.x, 0f, 4f);
        curr_scale.y = Mathf.Clamp(curr_scale.y, 0f, 4f);
        curr_scale.z = Mathf.Clamp(curr_scale.z, 0f, 4f);
        transform.localScale = curr_scale;

        animProgress = Mathf.Clamp(animProgress, 0f, 0.99f);
        anim.SetFloat("progress", animProgress);

    }
}
