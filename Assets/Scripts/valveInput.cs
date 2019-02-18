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
    public Transform draggableObject;    
    public Transform objectHeld; 
    public bool pinchHold = false;
    public bool releaseOnPinchUp = false; 
    
	void Start () {
        hand = gameObject.GetComponent<Hand>();
    }

    //For grabbing, special function detects if touching, then stores touched G.O. if its tagged "Draggable"
    //Operations in update will reference that stored touched object, named "draggableObject"
    //Specific items, like tools and tags, will also store themselves as "objectHeld" if grabbed)
    void Update() {
       
        //Pulling the trigger can both pickup a new item, but also use a currently held item
        // With an empty hand, Send Grabbed message to touched object when trigger pulled
        // With tool in hand, Cannot pickup new objects if a tool is being held
            //If a tools is held and trigger pulled, the tool is used/returned to bag
        if (getPinchDown())
        {
           
            if (!releaseOnPinchUp)
            {
                //For holding trigger to grasp objects (if held for 1 second, switches release condition)
                StartCoroutine(HoldingPinch());

                //if holding something already, release that thing 
                if (objectHeld != null)
                {
                    Send(objectHeld, "Released");
                    objectHeld = null;
                }
                else
                {
                    //FOR EMPTY HAND
                    //Pickup if touching object (stores tools and tags as "objectHeld")
                    if (draggableObject != null)
                    {
                        Send(draggableObject, "Grabbed");
                    }
                }
            }                                 
        }

        if (getPinchUp())
        {
            StopAllCoroutines(); 

            //For letting go of bag (will optimize to include letting go of tools upon specific condition)
            if (draggableObject != null && objectHeld == null)
            {
                Send(draggableObject, "Released");                 
            }   
            
            if (releaseOnPinchUp)
            {
                if (objectHeld != null)
                {
                    //Let go of thing in hand (for pinch holding logic)                   
                    Send(objectHeld, "Released");
                    objectHeld = null;
                }               
                releaseOnPinchUp = false;

            }
        }       
    }
    

    //Called when touching an object
    private void OnCollisionEnter(Collision collision)
    {
        //If the object is interactable
        if (collision.gameObject.tag == "draggable" )
        {

            //If there is already an object being touched, turn its highlight off (objects have own script for hightlight, calls on that)
            if (draggableObject != null)
            {
                Send(draggableObject, "Touched", false); 
            }

            //Set the newly touched object and highlight it as long as there's no tool in hand or if you touch the bag
            if (objectHeld == null || collision.gameObject.name.Contains("Bag"))
            {
                draggableObject = collision.transform;
                Send(draggableObject, "Touched", true);
            }           
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //Deselect the exited object
        if (collision.gameObject.tag == "draggable")
        {

            Send(draggableObject, "Touched", false); 

            //Clears draggableObject as long as the exited object is equal to draggableObj
            //If there is alread a draggableObject stored while a new object is touched, this ensures that new object will get stored as the new draggableObj
            //Previous issues of clearing out draggableObj completely when new obj touched, would equal null instead of storing new thing
            if (collision.transform == draggableObject)
            {
                draggableObject = null;
            }
        }         
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
