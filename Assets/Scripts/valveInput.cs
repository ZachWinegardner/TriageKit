using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;


public class valveInput : MonoBehaviour {
    
    private Hand hand;
    public Transform draggableObject;
    public Transform toolInHand;
    public Transform tagInHand;
    public Color highlightColor; 
    public bool holdingTag = false; 
    

	void Start () {
        hand = gameObject.GetComponent<Hand>();
    }

    void Update() {
       
        //Send Grabbed message to touched object when trigger pulled
        //Cannot pickup new objects if a tool is being held
        //If a tools is held and trigger pulled, the tool is used/returned to bag
        if (getPinchDown())
        {
            //if holding a tool (grabbed function on tools' instrumentSelection.cs set this variable) 
            if (toolInHand != null)
            {
                //Let go of tool
                print("letting go of tool"); 
                toolInHand.GetComponent<instrumentSelection>().SendMessage("Released", transform, SendMessageOptions.DontRequireReceiver);
                toolInHand = null;
            }
            else
            {
                if(tagInHand != null)
                {
                    tagInHand.SendMessage("Released", SendMessageOptions.DontRequireReceiver);
                    tagInHand = null;
                }
                else
                {
                    //Pickup if touching object (if that object is a tool, stores "toolInHand" || if tag, stores "tagInHand")
                    if (draggableObject != null)
                    {
                        draggableObject.SendMessage("Grabbed", transform, SendMessageOptions.DontRequireReceiver);
                    }
                }
                
            }

            
        }

        if (getPinchUp())
        {
            //For letting go of bag
            if (draggableObject != null && toolInHand == null && tagInHand == null)
            {
                draggableObject.SendMessage("Released", SendMessageOptions.DontRequireReceiver);                
            }           
        }
    }
    

    //Called when touching an object
    private void OnCollisionEnter(Collision collision)
    {
        //If the object is interactable
        if (collision.gameObject.tag == "draggable" )
        {

            //If there is already an object being touched, turn its highlight off
            if (draggableObject != null)
            {
                draggableObject.GetComponent<SelectionHighlight>().Highlight(Color.black);  
            }

            //Set the newly touched object and highlight it as long as there's no tool in hand or if you touch the bag
            if (toolInHand == null || collision.gameObject.name.Contains("Bag"))
            {
                draggableObject = collision.transform;
                draggableObject.GetComponent<SelectionHighlight>().Highlight(highlightColor);
            }
            

            //If the touched object was the Bag, open it
            if (draggableObject.GetComponent<BagScript>())
            {
                //If the bag was touched while holding a tool, that tool is "put back"
                if (toolInHand != null)
                {
                    toolInHand.GetComponent<instrumentSelection>().Returned();
                }
                else
                {
                    draggableObject.GetComponent<BagScript>().Touched();
                }
            }

            //If the touched object was a UI icon
            if (draggableObject.GetComponent<IconTouch>())
            {
                draggableObject.GetComponent<IconTouch>().Touched(); 
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //Deselect the exited object
        if (collision.gameObject.tag == "draggable")
        {
            collision.gameObject.GetComponent<SelectionHighlight>().Highlight(Color.black);
            collision.gameObject.SendMessage("HideDetails", SendMessageOptions.DontRequireReceiver);
            //Clears draggableObject as long as the exited object is equal to draggableObj
            //Prevents newly touched object from getting cleared as draggableObj if touched while another object still touched
            if (collision.transform == draggableObject)
            {
                draggableObject = null;
            }
        } 

        
    }
    
    ////Called when touching an object with a trigger collider
    //private void OnTriggerEnter(Collider other)
    //{
    //    //If the object is interactable
    //    if (other.gameObject.tag == "draggable")
    //    {
    //        //If there is already an object being touched, turn its highlight off
    //        if (draggableObject != null)
    //        {
    //            draggableObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.black);                
    //        }

    //        //Highlight the object being touched
    //        draggableObject = other.transform;
    //        draggableObject.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
    //        draggableObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", highlightColor);

    //        //If the touched object was the Bag, open it
    //        if (draggableObject.GetComponent<BagScript>())
    //        {
    //            draggableObject.GetComponent<BagScript>().Touched();
    //        }
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    other.gameObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.black);

    //    //Clears draggableObject as long as the exited object is equal to draggableObj
    //    //Prevents newly touched object from getting cleared as draggableObj if touched while another object still touched
    //    if (other.transform == draggableObject)
    //    {
    //        draggableObject = null; 
    //    }
       
        
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    draggableObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.black);
    //    draggableObject = null;


    //}

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
