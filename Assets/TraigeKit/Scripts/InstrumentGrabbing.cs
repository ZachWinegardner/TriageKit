using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstrumentGrabbing : MonoBehaviour {
    private Transform handGrabber;
    private bool canBePutBack = false; 

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            print(selfPrefab.transform.localEulerAngles.ToString()); 
        }
    }

    public void Grabbed(Transform hand) {
        transform.SetParent(hand);
        transform.localPosition = heldPosition;
        transform.localEulerAngles = orientation;

        SuppliesManager.instance.counts[(int)type]--;
        UI.InstrumentGrabbed();       

    }

    public void Touched(bool state)
    {
        GetComponent<SelectionHighlight>().Highlight(state);
    }

    public void Released()
    {        
    }

    public void Apply()
    {
        Destroy(gameObject);
    }

    public void Returned()
    {
        //print("destroying");       
        SuppliesManager.instance.counts[(int)type]++;
        UI.ShowInstrument(selfPrefab); 
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PutBack" && canBePutBack)
        {
           Returned(); 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        canBePutBack = true; 
    }
}
