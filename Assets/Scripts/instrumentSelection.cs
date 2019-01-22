using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class instrumentSelection : MonoBehaviour {

    public GameObject operableInstrumentPrefab;
    //public int limitQuantity;
    //public int count;   
    public bool noneLeft = false;
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
    
    public void Grabbed(Transform hand) {
        //Debug.Log("instrument " + gameObject.name + "picked up.");
        //gameObject.GetComponent<Renderer>().enabled = false;
       if (SuppliesManager.instance.counts[(int)type] > 0)
        {
            GameObject instrument = Instantiate(operableInstrumentPrefab, transform.position, transform.rotation) as GameObject;
            instrument.transform.SetParent(hand);
            hand.GetComponent<valveInput>().toolInHand = instrument.transform;
            SuppliesManager.instance.counts[(int)type]--;
            if (SuppliesManager.instance.counts[(int)type] <= 0)
            {
                noneLeft = true;
            }
            //SuppliesManager.instance.SetText();
            gameObject.SetActive(false);
        }
        else
        {
            print("out of that instrument");
            //SuppliesManager.instance.texts[typeIndexer].text = "None Remaining"; 
        }
         


        //Set position and orientation based on specific tool                                
    }

    //public void ToggleVis(bool state)
    //{
    //    if (transform.childCount > 0)
    //    {
    //        MeshRenderer[] childRend = transform.GetComponentsInChildren<MeshRenderer>(); 
    //        foreach(MeshRenderer child in childRend)
    //        {
    //            child.enabled = state; 
    //        }
    //    }
    //    else
    //    {
    //        GetComponent<MeshRenderer>().enabled = state; 
    //    }

    //    GetComponent<Collider>().enabled = state;  

    //}    

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

    // the controller hand that selects the instrument calls this function
    //public void CreateInstrument(Transform whichHand) {

    //    GameObject instrument = Instantiate(operableInstrumentPrefab) as GameObject;
    //    // attach instrument to controller - instrument's own script should align itself appopriately
    //    instrument.transform.SetParent(whichHand);
    //    count++;

    //}
   

    // Update is called once per frame
    void Update () {
        // if there is limited quantity then hide the instrument when used up		
        
	}
}
