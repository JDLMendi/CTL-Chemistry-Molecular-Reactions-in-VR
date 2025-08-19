using UnityEngine;

[System.Serializable]
public class Bond {
    public GameObject atomA;
    public GameObject atomB;
    public BondType bondType;
    public bool broken;
}

public enum BondType
{
    Single,
    Double
}
