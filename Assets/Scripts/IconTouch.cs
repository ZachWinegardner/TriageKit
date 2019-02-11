using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconTouch : MonoBehaviour {
    //This script is for displaying the proper tool when the corresponding icon is touched
    //Goes on the sprites displaying the tools

    public Transform instrumentParent;
    public GameObject displayedInstrument;
    public GameObject allOutMessage;
    public TextMesh suppliesText; 

    public string nameText;

    public GameObject operableInstrumentPrefab;
    public BagScript bag;      
    public bool noneLeft = false;
    public enum toolType : int
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

    public void Touched()
    {
        int count = instrumentParent.childCount;        
        for (int i = 0; i < count; i++)
        {
            instrumentParent.GetChild(i).gameObject.SetActive(false); 
        }

        if (displayedInstrument.GetComponent<instrumentSelection>().noneLeft)
        {
            Debug.Log("out of that instrument");
            allOutMessage.SetActive(true); 
        }
        else
        {
            displayedInstrument.SetActive(true); 
        }



        ShowDetails();

        if (SuppliesManager.instance.counts[(int)type] > 0)
        {
            GameObject instrument = Instantiate(operableInstrumentPrefab, transform.position, transform.rotation) as GameObject;            
            SuppliesManager.instance.counts[(int)type]--;
            if (SuppliesManager.instance.counts[(int)type] <= 0)
            {
                noneLeft = true;
            }
            instrument.GetComponent<instrumentSelection>().StartCoroutine(CalculateDist(instrument.transform));
            //SuppliesManager.instance.SetText();
            gameObject.SetActive(false);
        }
        else
        {
            print("out of that instrument");
            //SuppliesManager.instance.texts[typeIndexer].text = "None Remaining"; 
        }
    }

    public void Grabbed(Transform hand)
    {
        if (displayedInstrument != allOutMessage)
        {
            displayedInstrument.GetComponent<instrumentSelection>().Grabbed(hand);
        }
    }
    public void ShowDetails()
    {
        suppliesText.gameObject.SetActive(true);        
        suppliesText.text = (nameText + ": " + SuppliesManager.instance.counts[(int)displayedInstrument.GetComponent<instrumentSelection>().type].ToString());
    }
    public void HideDetails()
    {
        //suppliesText.gameObject.SetActive(false);  

    }

    IEnumerator CalculateDist(Transform tool)
    {
        float distance = Vector3.Distance(tool.position, bag.transform.position);

        while (distance < 0.7f)
        {
            distance = Vector3.Distance(tool.position, bag.transform.position);
            yield return null;
        }

        if (distance >= 0.7f)
        {
            print("tool far enough away");
            bag.ReturnToHip();
        }
    }

}
