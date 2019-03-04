using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;


public class valveInput : MonoBehaviour {
    //This script handles the hand collisions and grabbing functions
    //It mainly sends a standard message to interactable objects to trigger unique functions, all under a ubiquitous "grabbed" method
    //Also includes input management via Alan's "viveInput" script
    private Hand hand;
    public bool gripping = false; 
    public Transform touchedObject;    
    public Transform objectHeld; 
    public bool pinchHold = false;
    public bool releaseOnPinchUp = false;
    public bool reaching = false; 
    public BagScript bag;

    public Animator anim;
    public Transform bagTrigger; 
    public bool triggerForGrabbing = false; 
    
	void Start () {
        hand = gameObject.GetComponent<Hand>();
    }

    //For grabbing, special function detects if touching, then stores touched G.O. if its tagged "Draggable"
    //Operations in update will reference that stored touched object, named "draggableObject"
    //Specific items, like tools and tags, will also store themselves as "objectHeld" if grabbed)
    void Update() {
       
        if (triggerForGrabbing)
        {
            // With an empty hand, Send Grabbed message to touched object when grip pulled               
            if (getPinchDown())
            {

                if (!releaseOnPinchUp)
                {
                    //For holding trigger to grasp objects (if held for 1 second, switches release condition)
                    //StartCoroutine(HoldingPinch());


                    //if holding something already, release that thing 
                    if (objectHeld)
                    {
                        Send(objectHeld, "Released");
                        objectHeld = null;
                    }
                    else
                    {
                        //FOR EMPTY HAND
                        //Pickup if touching object (will store thing grabbed as "objectHeld")
                        if (touchedObject)
                        {
                            Send(touchedObject, "Grabbed");
                            objectHeld = touchedObject;
                        }
                    }
                }

                //bag.ResetTimeout(); 
            }

            if (getPinchUp())
            {
                //StopAllCoroutines(); 

                //if something held, let go of it
                if (objectHeld)
                {
                    Send(objectHeld, "Released");
                    objectHeld = null;
                }

                //if (releaseOnPinchUp)
                //{
                //    if (objectHeld != null)
                //    {
                //        //Let go of thing in hand (for pinch holding logic)                   
                //        Send(objectHeld, "Released");
                //        objectHeld = null;
                //    }               
                //    releaseOnPinchUp = false;

                //}
            }

            if (getGrip_Down())
            {
                if (objectHeld)
                {
                    Send(objectHeld, "Apply");
                    objectHeld = null;
                }
            }
        }
        else
        {
            // With an empty hand, Send Grabbed message to touched object when grip pulled               
            if (getGrip_Down())
            {
                if (touchedObject != null)
                {
                    Send(touchedObject, "Grabbed");
                }
                gripping = true;


                //objectHeld = touchedObject;

                //if (!releaseOnPinchUp)
                //{
                //    //For holding trigger to grasp objects (if held for 1 second, switches release condition)
                //    //StartCoroutine(HoldingPinch());


                //    if holding something already, release that thing
                //    if (objectHeld)
                //    {
                //        Send(objectHeld, "Released");
                //        objectHeld = null;
                //    }
                //    else
                //    {
                //        //FOR EMPTY HAND
                //        //Pickup if touching object (will store thing grabbed as "objectHeld")
                //        if (touchedObject)
                //        {
                //            Send(touchedObject, "Grabbed");
                //            objectHeld = touchedObject;
                //        }
                //    }
                //}

                //bag.ResetTimeout(); 
            }

            if (getGrip_Up())
            {
                //StopAllCoroutines(); 

                //if something held, let go of it  
                if (touchedObject != null)
                {
                    Send(touchedObject, "Released");
                }
                    gripping = false;


                //if (releaseOnPinchUp)
                //{
                //    if (objectHeld != null)
                //    {
                //        //Let go of thing in hand (for pinch holding logic)                   
                //        Send(objectHeld, "Released");
                //        objectHeld = null;
                //    }               
                //    releaseOnPinchUp = false;

                //}
            }

            if (getPinchDown())
            {
                if (objectHeld)
                {
                    Send(objectHeld, "Apply");
                    objectHeld = null;
                }
            }
        }
        
        
    }
    

    //FOR TOUCHING
    private void OnCollisionEnter(Collision collision)
    {
        //If the object is interactable and you currently aren't holding anything
        if (collision.gameObject.tag == "draggable" && !gripping)
        {            
            //Set the newly touched object if the hand is empty and you aren't currently gripping
            if (objectHeld == null)
            {
                //If there is already an object being touched, "untouch" that object
                if (touchedObject != null)
                {
                    Send(touchedObject, "Touched", false);
                }
                touchedObject = collision.transform;
                Send(touchedObject, "Touched", true);
            }           
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //Deselect the exited object
        if (collision.gameObject.tag == "draggable")
        {

            Send(touchedObject, "Touched", false); 

            //Clears touchedObject as long as the exited object is equal to touchedObj
            //If there is alread a touchedObject stored while a new object is touched, this ensures that new object will get stored as the new touchedObj
            //Previous issues of clearing out touchedObj completely when new obj touched, would equal null instead of storing new thing
            if (collision.transform == touchedObject)
            {
                touchedObject = null;
            }
        }         
    }

    private void OnTriggerEnter(Collider other)
    {
       
    }

    private void OnTriggerExit(Collider other)
    {           

    }

    public IEnumerator HoldingPinch()
    {
        //print("started hold"); 
        float timer = 1f;
        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        if (timer <= 0f)
        {
            releaseOnPinchUp = true;
        }
    }

    void Send(Transform thing, string command)
    {
        //sends message with hand's transfrom in argument
        thing.SendMessage(command, transform, SendMessageOptions.DontRequireReceiver); 
    }

    void Send(Transform thing, string command, bool state)
    {
        thing.SendMessage(command, state, SendMessageOptions.DontRequireReceiver);        
    }

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
