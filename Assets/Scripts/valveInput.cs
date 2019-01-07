using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;


public class valveInput : MonoBehaviour {
    
    private Hand hand;
    private Transform draggableObject;
    

	void Start () {
        hand = gameObject.GetComponent<Hand>();
    }

    void Update() {

        if (getPinchDown())
        {
            if (draggableObject != null)
            {
                draggableObject.SendMessage("Grabbed", transform, SendMessageOptions.DontRequireReceiver);          
            }
        }

        if (getPinchUp())
        {
            if (draggableObject != null)
            {
                draggableObject.SendMessage("Released", SendMessageOptions.DontRequireReceiver);                
            }
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "draggable")
        {
            draggableObject = collision.transform;
            
            if (draggableObject.name == "Bag(Grabbable)")
            {
                draggableObject.GetComponent<BagScript>().Touched(); 
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        draggableObject = null; 
    }

    //private void OnCollisionStay(Collision collision)
    //{
    //    if (collision.gameObject.tag == "draggable")
    //    {
    //       // Debug.Log("touching tagged object"); 

    //        if (getPinchDown())
    //        {
    //            Debug.Log("Grabbed Object"); 
    //        }

    //    }
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log("Entered trigger");

    //    if (other.gameObject.tag == "draggable")
    //    {
    //        Debug.Log("interactive object"); 


    //    }
    //}



    public Vector2 getTrackPadPos()
    {
        // SteamVR_Action_Vector2 trackpadPos = SteamVR_Input._default.inActions.TouchPos;
        // return trackpadPos.GetAxis(hand.handType);
        return Vector2.zero;
    }

    public bool getPinch()
    {
        return SteamVR_Input._default.inActions.GrabPinch.GetState(hand.handType);
    }

    public bool getPinchDown()
    {
        return SteamVR_Input._default.inActions.GrabPinch.GetStateDown(hand.handType);
    }

    public bool getPinchUp()
    {
        return SteamVR_Input._default.inActions.GrabPinch.GetStateUp(hand.handType);
    }

    public bool getGrip()
    {
        return SteamVR_Input._default.inActions.GrabGrip.GetState(hand.handType);
    }

    public bool getGrip_Down()
    {
        return SteamVR_Input._default.inActions.GrabGrip.GetStateDown(hand.handType);
    }

    public bool getGrip_Up()
    {
        return SteamVR_Input._default.inActions.GrabGrip.GetStateUp(hand.handType);
    }

    public bool getMenu()
    {
        //return SteamVR_Input._default.inActions.MenuButton.GetState(hand.handType);
        return false;
    }

    public bool getMenu_Down()
    {
        //return SteamVR_Input._default.inActions.MenuButton.GetStateDown(hand.handType);
        return false;
    }

    public bool getMenu_Up()
    {
        // return SteamVR_Input._default.inActions.MenuButton.GetStateUp(hand.handType);
        return false;
    }

    public bool getTouchPad()
    {
        // return SteamVR_Input._default.inActions.Teleport.GetState(hand.handType);
        return false;
    }

    public bool getTouchPad_Down()
    {
        // return SteamVR_Input._default.inActions.Teleport.GetStateDown(hand.handType);
        return false;
    }

    public bool getTouchPad_Up()
    {
        // return SteamVR_Input._default.inActions.Teleport.GetStateUp(hand.handType);
        return false;
    }
}
