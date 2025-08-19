using UnityEngine;

public class BondUpdater : MonoBehaviour {
    public GameObject atomA;
    public GameObject atomB;
    public GameObject larger_atom;
    public float bond_threshold;
    public Bond bond_data;

    private int bonds; // 1 = Single bond, 2 = Double bond, etc.

    void Start() {
        if (atomA.transform.lossyScale.x > atomB.transform.lossyScale.x) { larger_atom = atomA; }
        else { larger_atom = atomB; }
    }

    void LateUpdate() {
        if (atomA == null || atomB == null) return;

        Vector3 posA = atomA.transform.position;
        Vector3 posB = atomB.transform.position;

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
        if(Vector3.Distance(posA, posB) > bond_threshold * larger_atom.transform.lossyScale.x) {
            bond_data.broken = true;
            if (bonds > 1) {
                for (var i=0; i < bonds; i++) {
                    transform.GetChild(i).gameObject.GetComponent<Renderer>().enabled = false;
                }
            } else {
                GetComponent<Renderer>().enabled = false;
            }
        } else {
            bond_data.broken = false;
            if (bonds > 1) {
                for (var i = 0; i < bonds; i++) {
                    transform.GetChild(i).gameObject.GetComponent<Renderer>().enabled = true;
                }
            } else {
                GetComponent<Renderer>().enabled = true;
            }
        }
    }

    public void SetBonds(int _bonds) { bonds = _bonds; }
    public int GetBonds() { return bonds; }
}
