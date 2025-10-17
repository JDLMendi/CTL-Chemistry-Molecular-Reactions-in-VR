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
            canvas.SetActive(true);
            labelText.text = swapper.currentMoleculeName;
        }
        else
        {
            canvas.SetActive(false);
            labelText.text = "";
        }
    }
}
