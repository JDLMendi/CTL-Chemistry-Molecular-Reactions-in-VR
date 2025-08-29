using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController cc;
    public Collider moleculeCollider;
    void Start()
    {
        cc = FindFirstObjectByType<CharacterController>();
        // Prevents the collision between the Player and Molecule
        Physics.IgnoreCollision(moleculeCollider, cc, true);
    }
}