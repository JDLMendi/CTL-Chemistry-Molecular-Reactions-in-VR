using System.Collections.Generic;
using UnityEngine;

public class Charge : MonoBehaviour {
    public int base_charge;
    public MoleculeManager molecule_manager;
    public List<int> bond_indexes = new List<int>();
    public List<int> charges = new List<int>();

    public GameObject img_charge;

    void Update() {
        var bond_charge_total = base_charge;
        for(var i=0; i < bond_indexes.Count; i++) {
            if (!molecule_manager.bonds[bond_indexes[i]].broken) {
                bond_charge_total += charges[i];
            }
        }

        // Update UI charge icon position and charge status.
        img_charge.transform.position = Camera.main.WorldToScreenPoint(this.transform.position);
        img_charge.GetComponent<ChargeUI>().SetCharge(bond_charge_total);
    }
}