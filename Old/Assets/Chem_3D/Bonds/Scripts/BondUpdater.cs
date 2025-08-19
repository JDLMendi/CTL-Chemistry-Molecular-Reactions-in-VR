using UnityEngine;

public class BondUpdater : MonoBehaviour
{
    public GameObject atomA;
    public GameObject atomB;
    public float bond_threshold;

    private int bonds; // 1 = Single bond, 2 = Double bond, etc.

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
            atomA.transform.lossyScale.x * 0.006f,
            dir.magnitude / 2f,
            atomA.transform.lossyScale.z * 0.006f
        );

        // Bond Threshold: Only show bond if atoms are close enough together.
        if(Vector3.Distance(posA, posB) > bond_threshold * atomA.transform.lossyScale.x) {
            if (bonds > 1) {
                for (var i=0; i < bonds; i++) {
                    transform.GetChild(i).gameObject.GetComponent<Renderer>().enabled = false;
                }
            } else {
                GetComponent<Renderer>().enabled = false;
            }
        } else {
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