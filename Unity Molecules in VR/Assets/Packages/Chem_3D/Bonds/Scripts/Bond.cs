using UnityEngine;

[System.Serializable]
public class Bond {
    public GameObject atomA;
    public GameObject atomB;
    public BondType bondType;
}

public enum BondType
{
    Single,
    Double,
    Triple
}
