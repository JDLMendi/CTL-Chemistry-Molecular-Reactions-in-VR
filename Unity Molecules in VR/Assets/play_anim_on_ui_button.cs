using UnityEngine;
using UnityEngine.UI;

public class play_anim_on_ui_button : MonoBehaviour
{
    public Animator targetAnimator;
    public string animationName = "YourAnimationName";

    void Start()
    {
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(PlayAnim);
        }
    }

    void PlayAnim()
    {
        if (targetAnimator != null)
        {
            targetAnimator.Play(animationName, -1, 0f);
        }
    }
}
