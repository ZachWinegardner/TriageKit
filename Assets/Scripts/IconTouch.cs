using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconTouch : MonoBehaviour {
    //This script is for displaying the proper tool when the corresponding icon is touched
    //Goes on the sprites displaying the tools

    public Transform instrumentParent;
    public GameObject displayedInstrument;
    public GameObject allOutMessage;   

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
    }

}
