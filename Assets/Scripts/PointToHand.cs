using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointToHand : MonoBehaviour {

    public Transform cam, hand, cube;
    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = cam.position;
        transform.LookAt(hand); 

        if (Input.GetKeyDown(KeyCode.P))
        {
            cube.position = hand.position;
            cube.position += transform.forward; 
        }
      
	}
}
