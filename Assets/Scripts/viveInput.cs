using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

// started this from: https://stackoverflow.com/questions/52686286/how-to-update-your-unity-project-input-to-steamvr-2-0

public class viveInput : MonoBehaviour {

    //private LineRenderer LR;

    private Hand hand;

    // Use this for initialization
    void Start() {
        hand = gameObject.GetComponent<Hand>();
       // LR = gameObject.GetComponent<LineRenderer>();
    }

    void Update() {
       // Debug.Log(getPinch());
    }

    public void HapticPulse()
    {
        hand.TriggerHapticPulse(1000f, 60f, 1f); // duration, frequency amplitude;
    }

    public Vector2 getTrackPadPos() {
        // SteamVR_Action_Vector2 trackpadPos = SteamVR_Input._default.inActions.TouchPos;
        // return trackpadPos.GetAxis(hand.handType);
        return Vector2.zero;
    }

    public bool getPinch() {
        return SteamVR_Input._default.inActions.GrabPinch.GetState(hand.handType);
    }

    public bool getPinchDown() {
        return SteamVR_Input._default.inActions.GrabPinch.GetStateDown(hand.handType);
    }

    public bool getPinchUp() {
        return SteamVR_Input._default.inActions.GrabPinch.GetStateUp(hand.handType);
    }

    public bool getGrip() {
        return SteamVR_Input._default.inActions.GrabGrip.GetState(hand.handType);
    }

    public bool getGrip_Down() {
        return SteamVR_Input._default.inActions.GrabGrip.GetStateDown(hand.handType);
    }

    public bool getGrip_Up() {
        return SteamVR_Input._default.inActions.GrabGrip.GetStateUp(hand.handType);
    }

    public bool getMenu() {
        //return SteamVR_Input._default.inActions.MenuButton.GetState(hand.handType);
        return false;
    }

    public bool getMenu_Down() {
        //return SteamVR_Input._default.inActions.MenuButton.GetStateDown(hand.handType);
        return false;
    }

    public bool getMenu_Up() {
       // return SteamVR_Input._default.inActions.MenuButton.GetStateUp(hand.handType);
        return false;
    }

    public bool getTouchPad() {
       // return SteamVR_Input._default.inActions.Teleport.GetState(hand.handType);
        return false;
    }

    public bool getTouchPad_Down() {
       // return SteamVR_Input._default.inActions.Teleport.GetStateDown(hand.handType);
        return false;
    }

    public bool getTouchPad_Up() {
       // return SteamVR_Input._default.inActions.Teleport.GetStateUp(hand.handType);
        return false;
    }

    public Vector3 getControllerPosition() {
        SteamVR_Action_Pose[] poseActions = SteamVR_Input._default.poseActions;
        if (poseActions.Length > 0) {
            return poseActions[0].GetLocalPosition(hand.handType);
        }
        return new Vector3(0, 0, 0);
    }

    public Quaternion getControllerRotation() {
        SteamVR_Action_Pose[] poseActions = SteamVR_Input._default.poseActions;
        if (poseActions.Length > 0) {
            return poseActions[0].GetLocalRotation(hand.handType);
        }
        return Quaternion.identity;
    }


    // First, a rigidbody set to iskinematic would be added to this hand controller
    // second, some colliders need to be created in the scene set as triggers
    // When the hand controller enters the trigger collider, this function is called
    //private void OnTriggerEnter(Collider other)
    //{

    //    Debug.Log("trigger");
    //    // only vibrate if holding the trigger AND the trigger collider has "pulse" in its name
    //    if (other.gameObject.name.Contains("pulse")) { //getPinchDown() && 

           
    //        float strength = 1f;
    //        float rate = 0.5f;

    //        strength = other.gameObject.GetComponent<pulseColliderSetting>().pulseStrength; 
    //        rate = other.gameObject.GetComponent<pulseColliderSetting>().pulseRate;
    //        StartCoroutine(MakePulse(strength, rate));
    //    }

    //}

    private void OnTriggerExit(Collider other)
    {
        StopAllCoroutines();
    }

    // coroutine that makes continuous pulse - not tested yet
    private IEnumerator MakePulse(float strength, float rate)
    {
        while (true)
        {
            hand.TriggerHapticPulse((ushort)Mathf.Lerp(0, 3999, strength));
            yield return new WaitForSeconds(0.1f);
            hand.TriggerHapticPulse((ushort)Mathf.Lerp(0, 3999, strength * 0.5f));
            yield return new WaitForSeconds(rate);
        }
    }


}
