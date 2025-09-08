using UnityEngine;
using UnityEngine.EventSystems;

public class ToolbarManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [Header("Molecule Handlers")] 
    public MoleculeHandler handler;
    public AnimationHandler animationHandler;
    public ModelSwapper modelSwapper;

    [Header("Visibility Settings")] 
    public GameObject socialMenu;
    public GameObject toolbarCanvas;
    public float defaultAlpha = 0.1f;
    public float hoverAlpha = 1.0f;
    public float fadeDuration = 0.3f;

    private bool _isHovering = false;
    private float _currentFadeTime = 0.0f;
    private CanvasGroup _toolbarCanvasGroup;

    private void Awake()
    {
        modelSwapper = FindFirstObjectByType<ModelSwapper>();
        handler = FindFirstObjectByType<MoleculeHandler>();
        animationHandler = FindFirstObjectByType<AnimationHandler>();
        
        if (_toolbarCanvasGroup == null)
        {
            _toolbarCanvasGroup = GetComponent<CanvasGroup>();
            if (_toolbarCanvasGroup == null)
            {
                _toolbarCanvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
        }

        _toolbarCanvasGroup.alpha = defaultAlpha;
    }

    public void ResetToolbar()
    {
        animationHandler.animationProgress = 0.0f;
        animationHandler.isPlaying = false;
        animationHandler.isLooping = false;
    }

    private void Update()
    {
        HandleToolbarVisbility();
        HandleToolbarAlpha();
    }

    #region Visbility Handling

    private void HandleToolbarAlpha()
    {
        // Smoothly fade between alpha values
        if (_isHovering && _toolbarCanvasGroup.alpha < hoverAlpha)
        {
            _currentFadeTime += Time.deltaTime;
            float t = Mathf.Clamp01(_currentFadeTime / fadeDuration);
            _toolbarCanvasGroup.alpha = Mathf.Lerp(defaultAlpha, hoverAlpha, t);
        }
        else if (!_isHovering && _toolbarCanvasGroup.alpha > defaultAlpha)
        {
            _currentFadeTime += Time.deltaTime;
            float t = Mathf.Clamp01(_currentFadeTime / fadeDuration);
            _toolbarCanvasGroup.alpha = Mathf.Lerp(hoverAlpha, defaultAlpha, t);
        }
        else
        {
            _currentFadeTime = 0f;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isHovering = true;
        _currentFadeTime = 0f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isHovering = false;
        _currentFadeTime = 0f;
    }

    private void HandleToolbarVisbility()
    {
        if (socialMenu.activeSelf)
        {
            toolbarCanvas.SetActive(false);
        }
        else
        {
            toolbarCanvas.SetActive(true);
        }
    }

    #endregion

    #region Button Functions

    public void Replay()
    {
        animationHandler.animationProgress = 0.0f;
        animationHandler.isPlaying = true;
    }

    public void PlayPause()
    {
        animationHandler.isPlaying = !animationHandler.isPlaying;
    }

    public void ToggleLoop()
    {
        animationHandler.isLooping = !animationHandler.isLooping;
    }

    public void ForwardStep()
    {
        var data =  animationHandler.animationProgress;
        float progress = Mathf.Clamp(data + animationHandler.stepLength, 0f, 0.99f);
        animationHandler.SetAnimationProgress(progress);
    }

    public void BackStep()
    {
        var data =  animationHandler.animationProgress;
        float progress = Mathf.Clamp(data - animationHandler.stepLength, 0f, 0.99f);
        animationHandler.SetAnimationProgress(progress);
    }

    public void ScaleUp()
    {
        Debug.Log("Scale Up");
        
    }

    public void ScaleDown()
    {
        Debug.Log("Scale Down");
    }

    public void Reset()
    {
        modelSwapper.LoadModel();
        ResetToolbar();
    }

    public void ToggleSwapperMenu()
    {
        var active  = modelSwapper.gameObject.activeSelf;
        modelSwapper.gameObject.SetActive(!active);
    }

#endregion
}
