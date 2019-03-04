using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;



public class mcivr_controllerInput : MonoBehaviour {

    public SteamVR_Input_Sources handType;
    private Hand hand;

    private GameObject touchedObject;
    private Transform grabber;

    void Start() {
       hand = gameObject.GetComponent<Hand>();
        grabber = transform.Find("Aim");

    }

    private void Update() {

        if (getGrip_Down()) {
            
           
            // if there is a touched object but nothing grabbed, then grab the object
            if (touchedObject && grabber.childCount == 0) {
                touchedObject.SendMessage("Touched", false, SendMessageOptions.DontRequireReceiver);
                touchedObject.SendMessage("Grabbed", grabber, SendMessageOptions.DontRequireReceiver);
            }
        }

        if (getGrip_Up()) {
            
            // if there is a grabbed object then release it
            if (grabber.childCount > 0) {
                grabber.GetChild(0).gameObject.SendMessage("Released", SendMessageOptions.DontRequireReceiver);
            }

        }

        if (getPinchDown()) {
            // if there is a grabbed object then tell it to apply itself
           if (grabber.childCount > 0) {
                grabber.GetChild(0).gameObject.SendMessage("Apply", grabber, SendMessageOptions.DontRequireReceiver);
           }
           // tell a touched object to "apply"
           else if (touchedObject) {
                touchedObject.SendMessage("Apply", grabber, SendMessageOptions.DontRequireReceiver);
            }

        }

    }

  
    private void OnCollisionEnter(Collision collision) {
        // detect a touchable object and tell the object it has been touched
        if (collision.gameObject.tag == "draggable" && touchedObject == null && grabber.childCount == 0) {
            touchedObject = collision.gameObject;
            touchedObject.SendMessage("Touched", true, SendMessageOptions.DontRequireReceiver);
            HapticPulse(); 
        }

    }


    private void OnCollisionExit(Collision collision) {
        // if there was a touched object then tell it it is no longer touched
        if (touchedObject) {
            touchedObject.SendMessage("Touched", false, SendMessageOptions.DontRequireReceiver);
            touchedObject = null;
        }
 
    }


    private void OnTriggerEnter(Collider other) {
        // for pulse and perhaps other things
        // only vibrate if holding the trigger AND the trigger collider has "pulse" in its name
        if (getPinchDown() && other.gameObject.name.Contains("pulse")) {

            // one way to have the collider tell the hand what the pulse rate is would be to name the collider object
            // something like "pulse_1_0.25" and then this script could parse those values from the name
            float strength = 1.0f;
            float rate = 0.25f;
            StartCoroutine(MakePulse(strength, rate));
        }

    }

    private void OnTriggerExit(Collider other) {
        StopAllCoroutines();
    }

    // coroutine that makes continuous pulse - not tested yet
    private IEnumerator MakePulse(float strength, float rate) {
        while (true) {
            hand.TriggerHapticPulse((ushort)Mathf.Lerp(0, 3999, strength));
            yield return new WaitForSeconds(0.1f);
            hand.TriggerHapticPulse((ushort)Mathf.Lerp(0, 3999, strength * 0.5f));
            yield return new WaitForSeconds((60f / rate) - 0.1f);
        }
    }



    public void HapticPulse() {
        hand.TriggerHapticPulse(1000f,60f,1f); // duration, frequency amplitude;
    }

    //public Vector2 getTrackPadPos() {
    //    SteamVR_Action_Vector2 trackpadPos = SteamVR_Input._default.inActions.TouchPos;
    //    return trackpadPos.GetAxis(handType);
    //}

    public bool getPinch() {
        return SteamVR_Input._default.inActions.GrabPinch.GetState(handType);
    }

    public bool getPinchDown() {
        return SteamVR_Input._default.inActions.GrabPinch.GetStateDown(handType);
    }

    public bool getPinchUp() {
        return SteamVR_Input._default.inActions.GrabPinch.GetStateUp(handType);
    }

    public bool getGrip() {
        return SteamVR_Input._default.inActions.GrabGrip.GetState(handType);
    }

    public bool getGrip_Down() {
        return SteamVR_Input._default.inActions.GrabGrip.GetStateDown(handType);
    }

    public bool getGrip_Up() {
        return SteamVR_Input._default.inActions.GrabGrip.GetStateUp(handType);
    }

    //public bool getMenu() {
    //    return SteamVR_Input._default.inActions.MenuButton.GetState(handType);
    //}

    //public bool getMenu_Down() {
    //    return SteamVR_Input._default.inActions.MenuButton.GetStateDown(handType);
    //}

    //public bool getMenu_Up() {
    //    return SteamVR_Input._default.inActions.MenuButton.GetStateUp(handType);
    //}

    //public bool getTouchPad() {
    //    return SteamVR_Input._default.inActions.Touchpad.GetState(handType);
    //}

    public bool getTouchPad_Down() {
       return SteamVR_Input._default.inActions.Teleport.GetStateDown(handType);
    }

    public bool getTouchPad_Up() {
        return SteamVR_Input._default.inActions.Teleport.GetStateUp(handType);
    }

    public Vector3 getControllerPosition() {
        SteamVR_Action_Pose[] poseActions = SteamVR_Input._default.poseActions;
        if (poseActions.Length > 0) {
            return poseActions[0].GetLocalPosition(handType);
        }
        return new Vector3(0, 0, 0);
    }

    public Quaternion getControllerRotation() {
        SteamVR_Action_Pose[] poseActions = SteamVR_Input._default.poseActions;
        if (poseActions.Length > 0) {
            return poseActions[0].GetLocalRotation(handType);
        }
        return Quaternion.identity;
    }







 }



