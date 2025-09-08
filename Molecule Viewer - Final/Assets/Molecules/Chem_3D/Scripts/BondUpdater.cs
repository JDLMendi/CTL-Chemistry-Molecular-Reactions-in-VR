using UnityEngine;

public class BondUpdater : MonoBehaviour {
    public bool visible;
    public bool partial;
    public float bond_threshold;
    public Bond bond_data; // Bond.cs data

    public Mesh sphere;
    public Mesh cylinder;

    private int bonds; // 1 = Single bond, 2 = Double bond, 3 = Triple bond
    private int ini_bonds;
    private GameObject larger_atom; // Bond forming/breaking control
    private GameObject prefab_2bonds;
    private GameObject prefab_3bonds;

    void Start() {
        ini_bonds = bonds;
        if (bond_data.atomA.transform.lossyScale.x > bond_data.atomB.transform.lossyScale.x) { larger_atom = bond_data.atomA; }
        else { larger_atom = bond_data.atomB; }

        prefab_2bonds = gameObject.transform.GetChild(0).gameObject;
        prefab_3bonds = gameObject.transform.GetChild(1).gameObject;

        visible = true;
    }

    void Update() {
        // Bond Display
        this.GetComponent<Renderer>().enabled = (bonds == 1 && visible);
        prefab_2bonds.SetActive(bonds == 2 && visible);
        prefab_3bonds.SetActive(bonds == 3 && visible);

        if (partial) {
            this.GetComponent<MeshFilter>().sharedMesh = sphere;
        } else {
            this.GetComponent<MeshFilter>().sharedMesh = cylinder;
        }
    }

    void LateUpdate() {
        if (bond_data.atomA == null || bond_data.atomB == null) return;

        Vector3 posA = bond_data.atomA.transform.position;
        Vector3 posB = bond_data.atomB.transform.position;

        // Position
        transform.position = (posA + posB) / 2f;

        // Rotation
        Vector3 dir = posB - posA;
        transform.up = dir.normalized;

        // Scale
        transform.localScale = new Vector3(
            larger_atom.transform.lossyScale.x * 0.006f,
            dir.magnitude / 2f,
            larger_atom.transform.lossyScale.z * 0.006f
        );

        // Bond Threshold: Only show bond if atoms are close enough together.
        visible = (Vector3.Distance(posA, posB)) < (bond_threshold * larger_atom.transform.lossyScale.x);
        // Partial Bond Threshold = Within 10% away from bond threshold.
        partial = (Vector3.Distance(posA, posB)) > (bond_threshold * larger_atom.transform.lossyScale.x * 0.9);
    }

    public void SetBonds(int _bonds) { 
        bonds = _bonds; 
        switch(bonds) {
            case 1: bond_data.bondType = BondType.Single; break;
            case 2: bond_data.bondType = BondType.Double; break;
            case 3: bond_data.bondType = BondType.Triple; break;
        }
    } public int GetBonds() { return bonds; }
    public int GetIniBonds() { return ini_bonds; }
}
