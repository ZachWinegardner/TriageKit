using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class instrumentSelection : MonoBehaviour {

    public Transform bag, icon;      
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
    
    public void Grabbed(Transform hand) {        
       if (SuppliesManager.instance.counts[(int)type] > 0)
        {            
            transform.SetParent(hand);
            transform.localPosition = heldPosition; 
            transform.localEulerAngles = orientation; 
            hand.GetComponent<valveInput>().toolInHand = transform;
            SuppliesManager.instance.counts[(int)type]--;                                   
        }                 
    } 
    
    public void Released()
    {
        Destroy(gameObject);        
    }

    public void Returned()
    {
        Destroy(gameObject);
        SuppliesManager.instance.counts[(int)type]++;
        //SuppliesManager.instance.SetText();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform == icon || collision.transform == bag)
        {
            Returned(); 
        }
    }
}
