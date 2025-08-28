using UnityEngine;
using UnityEngine.UI;

public class ModelSwapper : MonoBehaviour
{
    public int model_index;
    public Image model_img;
    public Text model_text;

    public Sprite[] model_sprites;
    public string[] model_names;
    public GameObject[] molecule_models;

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
            molecule_models[i].SetActive(i == model_index);
        }
    }
}
