using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;


public class PointToHand : MonoBehaviour {

    public Transform cam, hand, cube;
    public float dist = 1f; 
    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = cam.position;
        transform.LookAt(hand); 

        if (SteamVR_Input._default.inActions.GrabGrip.GetStateDown(SteamVR_Input_Sources.Any))
        {
            cube.position = hand.position;
            cube.position += transform.forward*dist;
            cube.LookAt(this.transform);
            cube.localEulerAngles = new Vector3(0, cube.localEulerAngles.y+180, 0);
        }
      
	}
}
