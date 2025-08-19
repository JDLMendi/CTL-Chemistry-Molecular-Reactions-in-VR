using System.Collections.Generic;
using UnityEngine;

public class MoleculeManager : MonoBehaviour
{
    public GameObject singleBondPrefab;
    public GameObject doubleBondPrefab;

    public float bondDistanceThreshold;

    public List<Bond> bonds = new List<Bond>();
    private List<GameObject> spawnedBonds = new List<GameObject>();

    void Start() {
        SpawnBonds();
    }

    void SpawnBonds() {
        foreach(var bond in bonds) {
            // Assigns correct prefab to bond.
            var _bonds = 1;
            GameObject prefab = bond.bondType == BondType.Double ? doubleBondPrefab : singleBondPrefab;
            if(bond.bondType == BondType.Double) { _bonds = 2; };
            GameObject bondObj = Instantiate(prefab, transform);

            // Bond color mixing
            if(_bonds >= 2) { // Multi-bonds
                for(var i=0; i < _bonds; i++) {
                    bondObj.transform.GetChild(i).gameObject.GetComponent<Renderer>().material.SetColor("_AtomColorA", bond.atomA.GetComponent<Renderer>().material.GetColor("_Color"));
                    bondObj.transform.GetChild(i).gameObject.GetComponent<Renderer>().material.SetColor("_AtomColorB", bond.atomB.GetComponent<Renderer>().material.GetColor("_Color"));
                }
            } else { // Single bond
                bondObj.GetComponent<Renderer>().material.SetColor("_AtomColorA", bond.atomA.GetComponent<Renderer>().material.GetColor("_Color"));
                bondObj.GetComponent<Renderer>().material.SetColor("_AtomColorB", bond.atomB.GetComponent<Renderer>().material.GetColor("_Color"));
            }

            // Bond updater: Controls position+rotation during animations.
            BondUpdater updater = bondObj.AddComponent<BondUpdater>();
            updater.atomA = bond.atomA;
            updater.atomB = bond.atomB;
            updater.bond_threshold = bondDistanceThreshold;
            updater.SetBonds(_bonds);

            spawnedBonds.Add(bondObj);
        }
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        // Draw bonds in inspector to help track existing ones.
        foreach(var bond in bonds) {
            Vector3 posA = bond.atomA.transform.position;
            Vector3 posB = bond.atomB.transform.position;
            Gizmos.DrawLine(posA, posB);

            // Double bond
            if (bond.bondType == BondType.Double)
            {
                Vector3 dir = (posB - posA).normalized;
                Vector3 offset = Vector3.Cross(dir, Vector3.up) * 0.004f * bond.atomA.transform.lossyScale.x;
                Gizmos.DrawLine(posA + offset, posB + offset);
            }
        }
    }
}
