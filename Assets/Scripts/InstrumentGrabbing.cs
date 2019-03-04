using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstrumentGrabbing : MonoBehaviour {
    private Transform handGrabber;

    public Transform bag, icon;
    public Kit_UI UI;
    public GameObject selfPrefab; 
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

        SuppliesManager.instance.counts[(int)type]--;
        UI.InstrumentGrabbed();
        handGrabber = hand;
        handGrabber.GetComponent<valveInput>().objectHeld = transform;

    }

    public void Released()
    {
        //the hand clears out objectHeld on Grip up. Since this doesn't actually release the instruments, they need to remind the controller script its still holding it
        
    }

    public void Apply()
    {
        handGrabber.GetComponent<valveInput>().touchedObject = null;
        Destroy(gameObject);
    }

    public void Returned()
    {
        print("destroying");
        handGrabber.GetComponent<valveInput>().objectHeld = null;
        handGrabber.GetComponent<valveInput>().touchedObject = null;
        SuppliesManager.instance.counts[(int)type]++;
        UI.ShowInstrument(selfPrefab); 
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Finish")
        {
            Returned(); 
        }
    }   
}
