using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR; 

public class BagScript : MonoBehaviour {


    Transform hipParent;
    Vector3 hipRotation; 

    public void Start()
    {
        hipParent = transform.parent;
        hipRotation = transform.localEulerAngles; 
    }
    private void Update()
    {
		if (SteamVR_Input._default.inActions.Teleport.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            ReturnToHip(); 
        }
    }
   
    public void Grabbed()
    {
        transform.parent = null; 
    }

    public void ReturnToHip()
    {
        transform.parent = hipParent;
        transform.localPosition = Vector3.zero; 
        transform.localEulerAngles = hipRotation;
    }
}
