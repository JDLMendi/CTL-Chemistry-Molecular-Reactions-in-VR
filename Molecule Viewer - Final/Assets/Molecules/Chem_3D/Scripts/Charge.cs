using System.Collections.Generic;
using UnityEngine;

public class Charge : MonoBehaviour {
    public int base_charge;
    public MoleculeManager molecule_manager;
    public List<int> bond_indexes = new List<int>();
    public List<int> charges = new List<int>();

    public GameObject prefab_charge;

    void Update() {
        var bond_charge_total = base_charge;
        for(var i=0; i < bond_indexes.Count; i++) {
            var bond_updater = molecule_manager.GetBond(bond_indexes[i]).GetComponent<BondUpdater>();
            if (bond_updater.visible && !bond_updater.partial) {
                bond_charge_total += charges[i];
            }
        }

        // Update UI charge status.
        prefab_charge.GetComponent<ChargeUI>().SetCharge(bond_charge_total);
    }
}