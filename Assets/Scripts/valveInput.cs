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
    public Transform toolInHand;
    public Transform tagInHand;
    public Color highlightColor; 
    public bool holdingTag = false;
    public bool pinchHold = false;
    public bool releaseOnPinch = false; 

    private bool bagTouch = false; 
    

	void Start () {
        hand = gameObject.GetComponent<Hand>();
    }

    //For grabbing, special function detects if touching, then stores touched G.O. if its tagged "Draggable"
    //Operations in update will reference the stored touched object, named "draggableObject"
    //Specific items, like tools and tags, will also store themselves in toolInHand, and tagInHand)
    void Update() {
       
        //Pulling the trigger can both pickup a new item, but also use a currently held item
        // With an empty hand, Send Grabbed message to touched object when trigger pulled
        // With tool in hand, Cannot pickup new objects if a tool is being held
            //If a tools is held and trigger pulled, the tool is used/returned to bag
        if (getPinchDown())
        {
            pinchHold = true; 
            if (!releaseOnPinch)
            {
                StartCoroutine(HoldingPinch());
                //if holding a tool (grabbed function on tools' instrumentSelection.cs set "toolInHand") 
                if (toolInHand != null)
                {
                    //Let go of tool                   
                    toolInHand.GetComponent<instrumentSelection>().SendMessage("Released", transform, SendMessageOptions.DontRequireReceiver);
                    toolInHand = null;
                }
                else
                {
                    if (tagInHand != null)
                    {
                        tagInHand.SendMessage("Released", SendMessageOptions.DontRequireReceiver);
                        tagInHand = null;
                    }
                    else
                    {
                        //FOR EMPTY HAND
                        //Pickup if touching object (if that object is a tool, stores "toolInHand" || if tag, stores "tagInHand")
                        if (draggableObject != null)
                        {
                            draggableObject.SendMessage("Grabbed", transform, SendMessageOptions.DontRequireReceiver);
                        }
                    }
                }
            }
            else
            {
                //dependent on release on pinch

            }
                       
        }

        if (getPinchUp())
        {
            pinchHold = false; 
            //For letting go of bag (will optimize to include letting go of tools upon specific condition)
            if (draggableObject != null && toolInHand == null && tagInHand == null)
            {
                draggableObject.SendMessage("Released", SendMessageOptions.DontRequireReceiver);                
            }   
            
            if (releaseOnPinch)
            {
                if (toolInHand != null)
                {
                    //Let go of tool                   
                    toolInHand.GetComponent<instrumentSelection>().SendMessage("Released", transform, SendMessageOptions.DontRequireReceiver);
                    toolInHand = null;
                }
                else
                {
                    if (tagInHand != null)
                    {
                        tagInHand.SendMessage("Released", SendMessageOptions.DontRequireReceiver);
                        tagInHand = null;
                    }
                    else
                    {
                        //FOR EMPTY HAND
                        //Pickup if touching object (if that object is a tool, stores "toolInHand" || if tag, stores "tagInHand")
                        if (draggableObject != null)
                        {
                            draggableObject.SendMessage("Grabbed", transform, SendMessageOptions.DontRequireReceiver);
                        }
                    }
                }
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
                    //draggableObject.GetComponent<BagScript>().Touched();
                    bagTouch = true;
                    StartCoroutine(TouchBag()); 
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
            bagTouch = false; 
            collision.gameObject.GetComponent<SelectionHighlight>().Highlight(Color.black);
            collision.gameObject.SendMessage("HideDetails", SendMessageOptions.DontRequireReceiver);
            //Clears draggableObject as long as the exited object is equal to draggableObj
            //If there is alread a draggableObject stored while a new object is touched, this ensures that new object will get stored as the new draggableObj
            //Previous issues of clearing out draggableObj completely when new obj touched, would equal null instead of storing new thing
            if (collision.transform == draggableObject)
            {
                draggableObject = null;
            }
        }         
    }

    IEnumerator TouchBag()
    {
        float timer = 0.5f;
        while (bagTouch && timer > 0f)
        {
            timer-=Time.deltaTime;
            yield return null; 
        }
        if (timer <= 0f)
        {
            draggableObject.GetComponent<BagScript>().Touched();

        }
    }

    IEnumerator HoldingPinch()
    {
        float timer = 1.5f;
        while (pinchHold && timer > 0f)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        if (timer <= 0f)
        {
            releaseOnPinch = true; 
        }
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
