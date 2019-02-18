using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconTouch : MonoBehaviour {
    //This script is for displaying the proper tool when the corresponding icon is touched
    //Goes on the sprites displaying the tools
       
    public GameObject operableInstrumentPrefab;
    public Kit_UI UI;
    public BagScript bag; 
    public bool noneLeft = false;

    private int typeIndex;

    private void Start()
    {
        typeIndex = (int)operableInstrumentPrefab.GetComponent<InstrumentGrabbing>().type;
        print(typeIndex.ToString()); 
    }

    public void Touched(bool state)
    {
        if (state)
        {
            if (SuppliesManager.instance.counts[typeIndex] > 0)
            {
                UI.ShowInstrument(operableInstrumentPrefab);
            }
            else
            {
                UI.ShowInstrument(UI.goneText); 
            }
        }
       
        GetComponent<SelectionHighlight>().Highlight(state);
        bag.ResetTimeout();
        print("reset called"); 


        //if (SuppliesManager.instance.counts[(int)type] > 0)
        //{
        //    GameObject instrument = Instantiate(operableInstrumentPrefab, transform.position, transform.rotation) as GameObject;            
        //    SuppliesManager.instance.counts[(int)type]--;
        //    if (SuppliesManager.instance.counts[(int)type] <= 0)
        //    {
        //        noneLeft = true;
        //    }
        //    instrument.GetComponent<instrumentSelection>().StartCoroutine(CalculateDist(instrument.transform));
        //    //SuppliesManager.instance.SetText();
        //    gameObject.SetActive(false);
        //}
        //else
        //{
        //    print("out of that instrument");
        //    //SuppliesManager.instance.texts[typeIndexer].text = "None Remaining"; 
        //}
    }

    public void Grabbed(Transform hand)
    {
        UI.prefabStats.Grabbed(hand);       
    }  

}
