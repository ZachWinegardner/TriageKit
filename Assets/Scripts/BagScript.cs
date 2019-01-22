using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR; 

public class BagScript : MonoBehaviour {

    //This script goes on the triage BAG, which is the grabbable object. It starts off parented to the Kit object, which is always at the hip
    //This will handle the bag's behavior and also find if it is visible in the viewport

    private Transform hipParent;
    private Vector3 hipRotation;
    private Vector3 objViewPos;
    public bool isOpen = false; 
    public bool isSeen = false;
    private popoutInstruments popout; 

    //adjustment to widen the range of view
    public float viewportRangeMax = 1f;
    public float viewportRangeMin = 0f;

    public void Start()
    {
        hipParent = transform.parent;
        hipRotation = transform.localEulerAngles;
        popout = GetComponent<popoutInstruments>(); 
        SetToViewport(); 
    }
    private void Update()
    {
		if (SteamVR_Input._default.inActions.Teleport.GetStateDown(SteamVR_Input_Sources.Any))
        {
            ReturnToHip();
            popout.CloseBag();
            isOpen = false;
        }

        if (SteamVR_Input._default.inActions.GrabGrip.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            popout.OpenBag();
        }

        if (!isSeen && isOpen && transform.parent != null)
        {
            ReturnToHip();
            popout.CloseBag();
            isOpen = false;
        }

        //if (SteamVR_Input._default.inActions.GrabGrip.GetStateDown(SteamVR_Input_Sources.LeftHand))
        //{
        //    popout.CloseBag();
        //    isOpen = false; 
        //}

        SetToViewport();
        isSeen = IsWithinViewport(objViewPos);

    }

    //Grabbed() or Touched() is a ubiquitous function for any interactive. Each  interactive object, including the bag, would have Grabbed()
    //When event occurs, like a click, or touch, or button, (SendMessage "Grabbed") is called to that spcecific item
    //The bag here, when touched, opens the tools | but the tools when "Grabbed()" might instantiate or do something different
    //Easy way for player (mouse, controller, etc) to call the same function with one line of code, but each item does its own unique thing wihtin Grabbed()
    public void Touched()
    {
        //if the bag is in view and closed, open it
        if (isSeen && !isOpen)
        {
            popout.OpenBag();
            isOpen = true; 
        }                          
    }
    
    public void Grabbed(Transform hand)
    {
        transform.parent = hand; 
    }

    public void Released()
    {
        transform.parent = null;

    }

    public void ReturnToHip()
    {
        transform.parent = hipParent;
        transform.localPosition = Vector3.zero; 
        transform.localEulerAngles = hipRotation;
        popout.CloseBag();
        isOpen = false;
    }

    void SetToViewport()
    {
        //find the object's position on the viewport (normalized)
        objViewPos = Camera.main.WorldToViewportPoint(transform.position);        
    }

    public bool IsWithinViewport(Vector3 obj)
    {
        //see if the object's viewport position is within the normalized (0,0) to (1,1) space
        if (obj.x >= viewportRangeMin && obj.x <= viewportRangeMax && obj.y >= viewportRangeMin && obj.y <= viewportRangeMax && obj.z >= 0f)
        {
            return true;
        }
        else return false;

    }
}
