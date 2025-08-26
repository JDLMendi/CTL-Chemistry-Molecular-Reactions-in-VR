using System.Collections.Generic;
using UnityEngine;

public class MoleculeManager : MonoBehaviour {
    // Physical Elements: Bond Variants, Bond Threshold
    public GameObject bondPrefab;
    public float bondDistanceThreshold;
    private Mesh sphere;
    private Mesh cylinder;

    // UI Elements: Charge Icons
    public GameObject uiChargePrefab;

    // Data: Bond.cs data, Bond gameobjects
    public List<Bond> bonds = new List<Bond>();
    private List<GameObject> spawnedBonds = new List<GameObject>();
    public List<BondTrigger> bondTriggers = new List<BondTrigger>();

    void Start() {
        sphere = GetPrimitiveMesh(PrimitiveType.Sphere);
        cylinder = GetPrimitiveMesh(PrimitiveType.Cylinder);
        SpawnBonds();
    }
    void Update() {
        TriggerBonds();
    }

    void SpawnBonds() {
        foreach(var bond in bonds) {
            // Assigns correct prefab to bond.
            var _bonds = 1;
            switch(bond.bondType) {
                case BondType.Double: _bonds = 2; break;
                case BondType.Triple: _bonds = 3; break;
            }
            GameObject bondObj = Instantiate(bondPrefab, transform);

            // Bond color mixing
            bondObj.GetComponent<Renderer>().material.SetColor("_AtomColorA", bond.atomA.GetComponent<Renderer>().material.GetColor("_Color"));
            bondObj.GetComponent<Renderer>().material.SetColor("_AtomColorB", bond.atomB.GetComponent<Renderer>().material.GetColor("_Color"));
            for(var i=0; i < bondObj.transform.childCount; i++) {
                var GameObj = bondObj.transform.GetChild(i);
                for (var j = 0; j < GameObj.transform.childCount; j++) {
                    GameObj.transform.GetChild(j).gameObject.GetComponent<Renderer>().material.SetColor("_AtomColorA", bond.atomA.GetComponent<Renderer>().material.GetColor("_Color"));
                    GameObj.transform.GetChild(j).gameObject.GetComponent<Renderer>().material.SetColor("_AtomColorB", bond.atomB.GetComponent<Renderer>().material.GetColor("_Color"));
                }
            }

            // Bond updater: Controls bond + bond data during animations.
            BondUpdater updater = bondObj.AddComponent<BondUpdater>();
            updater.bond_threshold = bondDistanceThreshold;
            updater.bond_data = bond;
            updater.sphere = sphere;
            updater.cylinder = cylinder;
            updater.SetBonds(_bonds);

            spawnedBonds.Add(bondObj);
        }
    }
    void TriggerBonds() {
        for(var i=0; i < bondTriggers.Count; i++) {
            var _trigger = spawnedBonds[bondTriggers[i].triggerBond].GetComponent<BondUpdater>();
            var _affect = spawnedBonds[bondTriggers[i].affectBond].GetComponent<BondUpdater>();
            if (_trigger.visible) {
                _affect.SetBonds(bondTriggers[i].newBondType);
            } else {
                _affect.SetBonds(_affect.GetIniBonds());
            }
         }
    }

    public GameObject GetBond(int location) {
        return spawnedBonds[location];
    } // Returns BondUpdater gameObject.
    public void AddTrigger(int _affect, int _trigger, int _newType) {
        bondTriggers.Add(new BondTrigger(_affect, _trigger, _newType));
    }

    [System.Serializable]
    public class BondTrigger {
        public int affectBond; // Index of bond
        public int triggerBond; // Index of bond
        public int newBondType;

        public BondTrigger(int _bond_affect, int _bond_trigger, int _newType) {
            this.affectBond = _bond_affect;
            this.triggerBond = _bond_trigger;
            this.newBondType = _newType;
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
            if (bond.bondType == BondType.Double) {
                Vector3 dir = (posB - posA).normalized;
                Vector3 offset = Vector3.Cross(dir, Vector3.up) * 0.004f * bond.atomA.transform.lossyScale.x;
                Gizmos.DrawLine(posA + offset, posB + offset);
            } else if(bond.bondType == BondType.Triple) {
                Vector3 dir = (posB - posA).normalized;
                Vector3 offset = Vector3.Cross(dir, Vector3.up) * 0.004f * bond.atomA.transform.lossyScale.x;
                Gizmos.DrawLine(posA + offset, posB + offset);
                Gizmos.DrawLine(posA - offset, posB - offset);
            }
        }
    }

    Mesh GetPrimitiveMesh(PrimitiveType type) {
        GameObject temp = GameObject.CreatePrimitive(type);
        Mesh mesh = temp.GetComponent<MeshFilter>().sharedMesh;
        GameObject.DestroyImmediate(temp);
        return mesh;
    }
}