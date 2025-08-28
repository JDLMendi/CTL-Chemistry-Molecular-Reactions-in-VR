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
    }
}
