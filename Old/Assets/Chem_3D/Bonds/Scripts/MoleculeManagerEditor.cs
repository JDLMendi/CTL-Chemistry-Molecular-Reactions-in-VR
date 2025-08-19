using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(MoleculeManager))]
public class MoleculeManagerEditor : Editor {
    GameObject atomA;
    GameObject atomB;
    BondType bondType;

    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        MoleculeManager manager = (MoleculeManager)target;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Add Bond", EditorStyles.boldLabel);

        atomA = (GameObject)EditorGUILayout.ObjectField("Atom A", atomA, typeof(GameObject), true);
        atomB = (GameObject)EditorGUILayout.ObjectField("Atom B", atomB, typeof(GameObject), true);
        bondType = (BondType)EditorGUILayout.EnumPopup("Bond Type", bondType);

        if (GUILayout.Button("Add Bond")) {
            if (atomA != null && atomB != null && atomA != atomB) {
                bool exists = manager.bonds.Exists(b =>
                    (b.atomA == atomA && b.atomB == atomB) ||
                    (b.atomA == atomB && b.atomB == atomA)
                );

                if (!exists) {
                    Undo.RecordObject(manager, "Add Bond");
                    manager.bonds.Add(new Bond { atomA = atomA, atomB = atomB, bondType = bondType });
                    EditorUtility.SetDirty(manager);
                } else {
                    Debug.LogWarning("Bond already exists between these atoms!");
                }
            } else {
                Debug.LogWarning("Select two different atoms.");
            }
        }
    }
}
#endif
