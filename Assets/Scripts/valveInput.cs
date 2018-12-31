using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR; 

public class valveInput : MonoBehaviour {

    public GameObject summonedObject; 
    public float offsetSummonPos; 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		

		if (SteamVR_Input._default.inActions.Teleport.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            summonedObject.transform.position = Camera.main.transform.position;
            summonedObject.transform.rotation = Camera.main.transform.rotation; 
            summonedObject.transform.Translate(0, 0, offsetSummonPos); 
        }


    }
}
