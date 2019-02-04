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

}
