using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionHighlight : MonoBehaviour {

    public bool canBeHighlighted = true;
    public Color highlightColor;


    // Use this for initialization
    void Awake () {
        transform.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
    }
   
    public void Highlight(bool state)
    {
        Color color;
       
        if (canBeHighlighted)
        {
            if (state)
            {
                color = highlightColor;
            }
            else
            {
                color = Color.black; 
            }
            //check if object has children, but dont highlight if has more than 3 (for bag) optimize?
            if (transform.childCount > 0)
            {
                transform.GetChild(0).GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
                transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", color);

                transform.GetChild(1).GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
                transform.GetChild(1).GetComponent<Renderer>().material.SetColor("_EmissionColor", color);

                print("set highlight"); 

                

            }
            else
            {
                transform.GetComponent<Renderer>().material.SetColor("_EmissionColor", color);

            }
        }
        
    }
}
