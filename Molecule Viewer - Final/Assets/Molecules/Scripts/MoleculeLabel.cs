using System;
using UnityEngine;
using UnityEngine.UI;

public class MoleculeLabel : MonoBehaviour
{
    public Text labelText;
    public GameObject canvas;
    public ModelSwapper swapper;

    private void Update()
    {
        if (swapper.currentMoleculeName != "")
        {
            labelText.text = swapper.currentMoleculeName;
            canvas.SetActive(true);
        }
        else
        {
            labelText.text = "";
            canvas.SetActive(false);
        }
    }
}
