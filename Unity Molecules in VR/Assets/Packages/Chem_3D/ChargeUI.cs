using UnityEngine;
using UnityEngine.UI;

public class ChargeUI : MonoBehaviour
{
    private int charge;
    private GameObject charge_sprite;

    public Sprite pos_charge;
    public Sprite neg_charge;

    public void Start() {
        charge_sprite = gameObject.transform.GetChild(0).gameObject;
    }

    public void SetCharge(int new_charge) {
        charge = new_charge;

        // Update sprite.
        if(charge == 0) {
            charge_sprite.GetComponent<SpriteRenderer>().enabled = false;
        } else if(charge > 0) {
            charge_sprite.GetComponent<SpriteRenderer>().enabled = true;
            charge_sprite.GetComponent<SpriteRenderer>().sprite = pos_charge;
        } else {
            charge_sprite.GetComponent<SpriteRenderer>().enabled = true;
            charge_sprite.GetComponent<SpriteRenderer>().sprite = neg_charge;
        }
    }
    public int GetCharge() { return charge; }
}
