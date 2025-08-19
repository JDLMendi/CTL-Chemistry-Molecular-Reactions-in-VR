using UnityEngine;
using UnityEngine.UI;

public class ChargeUI : MonoBehaviour
{
    private int charge;

    public Sprite pos_charge;
    public Sprite neg_charge;

    public void SetCharge(int new_charge) {
        charge = new_charge;

        // Update sprite.
        if(charge == 0) {
            GetComponent<Image>().enabled = false;
        } else if(charge > 0) {
            GetComponent<Image>().enabled = true;
            GetComponent<Image>().sprite = pos_charge;
            GetComponent<RectTransform>().sizeDelta = new Vector2(30, 30);
        } else {
            GetComponent<Image>().enabled = true;
            GetComponent<Image>().sprite = neg_charge;
            GetComponent<RectTransform>().sizeDelta = new Vector2(30, 10);
        }
    }
    public int GetCharge() { return charge; }
}
