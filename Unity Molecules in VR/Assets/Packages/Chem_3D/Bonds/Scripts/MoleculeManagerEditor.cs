using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(MoleculeManager))]
public class MoleculeManagerEditor : Editor {
    GameObject atomA;
    GameObject atomB;
    BondType bondType;

    GameObject chargeAtom;
    int apply_charge;

    GameObject chargeBondAtom;
    int charger_bond_index;
    int apply_bond_charge;

    int affect_bond_index;
    int trigger_bond_index;
    int new_bond_count;

    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        MoleculeManager manager = (MoleculeManager)target;

        // Add Bonds
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Add Bond", EditorStyles.boldLabel);
        atomA = (GameObject)EditorGUILayout.ObjectField("Atom A", atomA, typeof(GameObject), true);
        atomB = (GameObject)EditorGUILayout.ObjectField("Atom B", atomB, typeof(GameObject), true);
        bondType = (BondType)EditorGUILayout.EnumPopup("Bond Type", bondType);
        if (GUILayout.Button("Add Bond")) {
            if (atomA != null && atomB != null && atomA != atomB) {
                bool exists = manager.bonds.Exists(b =>
                    ((b.atomA == atomA && b.atomB == atomB) ||
                    (b.atomA == atomB && b.atomB == atomA))
                );

                if (!exists) {
                    Undo.RecordObject(manager, "Add Bond");
                    manager.bonds.Add(new Bond { atomA = atomA, atomB = atomB, bondType = bondType});
                    EditorUtility.SetDirty(manager);
                } else {
                    Debug.LogWarning("[C3D] Bond already exists between these atoms!");
                }
            } else {
                Debug.LogWarning("[C3D] Select two different atoms. (Add Bond)");
            }
        }

        // UTILITY: Get Bond Index
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Get Bond Index", EditorStyles.boldLabel);
        atomA = (GameObject)EditorGUILayout.ObjectField("Atom A", atomA, typeof(GameObject), true);
        atomB = (GameObject)EditorGUILayout.ObjectField("Atom B", atomB, typeof(GameObject), true);
        if (GUILayout.Button("Get Bond Index")) {
            if (atomA != null && atomB != null && atomA != atomB) {
                int location = -1;

                for(var i=0; i < manager.bonds.Count; i++) {
                    if ((atomA == manager.bonds[i].atomA && atomB == manager.bonds[i].atomB) || (atomA == manager.bonds[i].atomB && atomB == manager.bonds[i].atomA)) {
                        location = i;
                    }
                }

                if(location != -1) {
                    Debug.Log(location);
                } else {
                    Debug.Log("[C3D] Could not find bond between atoms");
                }
            } else {
                Debug.LogWarning("[C3D] Select two different atoms. (Get Bond Index)");
            }
        }

        // Set Charge
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Set Charge", EditorStyles.boldLabel);
        chargeAtom = (GameObject)EditorGUILayout.ObjectField("Atom", chargeAtom, typeof(GameObject), true);
        apply_charge = (int)EditorGUILayout.IntField("Charge", apply_charge);
        if ( GUILayout.Button("Set Charge")) {
            if (chargeAtom != null) {
                if (chargeAtom.GetComponent<Charge>() == null) { // Add component if doesn't exist.
                    chargeAtom.AddComponent(typeof(Charge));

                    // Create new charge prefab as child of atom.
                    Undo.RecordObject(manager, "Set Charge");
                    GameObject ui_charge = Instantiate(manager.uiChargePrefab, chargeAtom.transform);
                    chargeAtom.GetComponent<Charge>().prefab_charge = ui_charge;
                    EditorUtility.SetDirty(manager);
                }
                chargeAtom.GetComponent<Charge>().base_charge = apply_charge;
                chargeAtom.GetComponent<Charge>().molecule_manager = manager;
            } else {
                Debug.LogWarning("[C3D] Please select an atom. (Add Charge)");
            }
        }

        // Add Charged Bond
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Add Charged Bond", EditorStyles.boldLabel);
        chargeBondAtom = (GameObject)EditorGUILayout.ObjectField("Atom", chargeBondAtom, typeof(GameObject), true);
        charger_bond_index = (int)EditorGUILayout.IntField("Bond Index", charger_bond_index);
        apply_bond_charge = (int)EditorGUILayout.IntField("Charge", apply_bond_charge);
        if ( GUILayout.Button("Add Charged Bond")) {
            if(chargeBondAtom != null) {
                if (chargeBondAtom.GetComponent<Charge>() == null) { // Add component if doesn't exist.
                    chargeBondAtom.AddComponent(typeof(Charge));
                    chargeBondAtom.GetComponent<Charge>().base_charge = 0;
                    chargeBondAtom.GetComponent<Charge>().molecule_manager = manager;

                    // Create new charge sprite as child of atom.
                    Undo.RecordObject(manager, "Add Charged Bond");
                    GameObject ui_charge = Instantiate(manager.uiChargePrefab, chargeBondAtom.transform);
                    chargeBondAtom.GetComponent<Charge>().prefab_charge = ui_charge;
                    EditorUtility.SetDirty(manager);
                }

                if (charger_bond_index < manager.bonds.Count && charger_bond_index >= 0) {
                    chargeBondAtom.GetComponent<Charge>().bond_indexes.Add(charger_bond_index);
                    chargeBondAtom.GetComponent<Charge>().charges.Add(apply_bond_charge);
                } else {
                    Debug.LogWarning("[C3D] Index is outside of range (Add Charge Bond)");
                }
            } else {
                Debug.LogWarning("[C3D] Please select an atom. (Add Charge Bond)");
            }
        }

        // Add Bond Trigger
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Add Bond Trigger", EditorStyles.boldLabel);
        affect_bond_index = (int)EditorGUILayout.IntField("Affected Bond Index", affect_bond_index);
        trigger_bond_index = (int)EditorGUILayout.IntField("Trigger Bond Index", trigger_bond_index);
        new_bond_count = (int)EditorGUILayout.IntField("New bond type (1-3)", new_bond_count);
        if( GUILayout.Button("Add Bond Trigger")) {
            if(new_bond_count > 0 && new_bond_count < 4) {
                if(affect_bond_index != trigger_bond_index) {
                    if(affect_bond_index < manager.bonds.Count && affect_bond_index >= 0 && trigger_bond_index < manager.bonds.Count && trigger_bond_index >= 0) {
                        Undo.RecordObject(manager, "Add Bond Trigger");
                        manager.AddTrigger(affect_bond_index, trigger_bond_index, new_bond_count);
                        Undo.RecordObject(manager, "Add Bond Trigger");
                        EditorUtility.SetDirty(manager);
                    } else {
                        Debug.LogWarning("[C3D] Index is outside of range (Add Bond Trigger)");
                    }
                } else {
                    Debug.LogWarning("[C3D] Bond cannot act as a trigger to itself.");
                }
            } else {
                Debug.LogWarning("[C3D] Invalid bond type (1-3)");
            }
        }
    }
}
#endif
