using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionHighlight : MonoBehaviour {

    public bool canBeHighlighted = true; 

	// Use this for initialization
	void Start () {
        transform.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
    }
   
    public void Highlight(Color color)
    {
        if (canBeHighlighted)
        {
            //check if object has children
            if (transform.childCount > 0 && transform.childCount < 4)
            {
                Renderer[] childrenRend = transform.GetComponentsInChildren<Renderer>();
                foreach (Renderer child in childrenRend)
                {
                    child.material.SetColor("_EmissionColor", color);
                }
                transform.GetComponent<Renderer>().material.SetColor("_EmissionColor", color);

            }
            else
            {
                transform.GetComponent<Renderer>().material.SetColor("_EmissionColor", color);

            }
        }
        
    }
}
