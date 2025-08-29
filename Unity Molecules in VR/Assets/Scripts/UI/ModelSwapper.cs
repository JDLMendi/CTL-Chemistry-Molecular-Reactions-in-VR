using System;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;

public class ModelSwapper : MonoBehaviour
{
    private MoleculeHandler handler;
    private AnimationManager animationManager;
    private MoleculeHandler moleculeHandler;
    
    public int model_index;
    public Image model_img;
    public Text model_text;

    public Sprite[] model_sprites;
    public string[] model_names;
    public GameObject[] molecule_models;
    
    [Header("Events")]
    public UnityEvent<int> OnModelSwapped;

    private void Start()
    {
        animationManager = FindFirstObjectByType<AnimationManager>();
        moleculeHandler = FindObjectOfType<MoleculeHandler>();
    }

    public MoleculeHandler molecule_handler;
    public AnimationManager anim_manager;

    public GameObject model_canvas;
    private bool panel_enable;

    void Start() {
        panel_enable = true;
    }

    void Update() {
        model_img.sprite = model_sprites[model_index];
        model_text.text = model_names[model_index];
    }

    public void NextIndex() {
        model_index = Mathf.Min(model_index + 1, molecule_models.Length - 1);
    }
    public void PrevIndex() {
        model_index = Mathf.Max(model_index - 1, 0);
    }

    public void LoadModel() {
        for(var i=0; i < molecule_models.Length; i++) {
            var model =  molecule_models[i];
            model.SetActive(i == model_index);

            if (model.activeSelf)
            {
                animationManager.anim = model.GetComponentInChildren<Animator>();
                animationManager.animationProgress = 0.0f;
                
                Transform fbxTransform = model.transform.Cast<Transform>().FirstOrDefault(t => t.name.StartsWith("fbx_"));
                GameObject fbxChild = fbxTransform != null ? fbxTransform.gameObject : null;

                moleculeHandler.model = fbxChild;
            }
            
            OnModelSwapped?.Invoke(model_index);
        }

        molecule_handler.model_index = model_index;
        molecule_handler.UpdateMoleculeState(0f, Vector3.one, Quaternion.identity);
        anim_manager.anim_index = model_index;
        anim_manager.animationProgress = 0f;
    }

    public void TogglePanel() {
        panel_enable = !panel_enable;
        model_canvas.SetActive(panel_enable);
    }
}
