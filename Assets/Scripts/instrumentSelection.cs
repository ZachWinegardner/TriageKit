using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class instrumentSelection : MonoBehaviour {

    public GameObject operableInstrumentPrefab;
    public int limitQuantity;
    private int count;

    // Use this for initialization
    void Start() {

    }

    public void Grabbed() {
        Debug.Log("instrument " + gameObject.name + "picked up.");
        gameObject.GetComponent<Renderer>().enabled = false;
        GameObject instrument = Instantiate(operableInstrumentPrefab, transform.position, transform.rotation) as GameObject;
        count++;
    }

    // the controller hand that selects the instrument calls this function
    public void CreateInstrument(Transform whichHand) {

        GameObject instrument = Instantiate(operableInstrumentPrefab) as GameObject;
        // attach instrument to controller - instrument's own script should align itself appopriately
        instrument.transform.SetParent(whichHand);
        count++;

    }

	
	
	// Update is called once per frame
	void Update () {
        // if there is limited quantity then hide the instrument when used up
		if (limitQuantity > 0 && count >= limitQuantity) {
            gameObject.SetActive(false);
        }
	}
}
