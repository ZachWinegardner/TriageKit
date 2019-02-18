using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstrumentGrabbing : MonoBehaviour {

    public Transform bag, icon;
    public Kit_UI UI;
    public enum toolType: int
    {
        SurgicalTape = 0,
        Gauze = 1,
        Occlusive = 2,
        SAM_Splint = 3,
        Tourniquet = 4,
        Shears = 5,
        Needle = 6,
        Naso = 7
    }
    public toolType type;   
    public Vector3 heldPosition;
    public Vector3 orientation;

    private void Awake()
    {
        UI = GameObject.Find("Shaped_Bag").GetComponent<Kit_UI>(); 
    }

    public void Grabbed(Transform hand) {
        transform.SetParent(hand);
        transform.localPosition = heldPosition;
        transform.localEulerAngles = orientation;

        hand.GetComponent<valveInput>().objectHeld = transform;
        SuppliesManager.instance.counts[(int)type]--;
        UI.InstrumentGrabbed();
    } 
    
    public void Released()
    {
        Destroy(gameObject);       
    }

    public void Returned()
    {
        Destroy(gameObject);
        SuppliesManager.instance.counts[(int)type]++;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("Bag"))
        {
            Returned(); 
        }
    }
}
