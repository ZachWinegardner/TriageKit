using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR; 

public class BagScript : MonoBehaviour {

    //This script goes on the triage BAG, which is the grabbable object. It starts off parented to the Kit object, which is always at the hip
    //This will handle the bag's behavior and also find if it is visible in the viewport

    private Transform hipParent;
    private Vector3 bagViewportPos;   
    private popoutInstruments popout;
    private Vector3 bagHomePos;
    private Vector3 bagHomeRotation;

    public bool isOpen = false;
    public bool isSeen = false;
    public bool isTouched = false; 
    public bool timeoutRunning = false; 
    //adjustment to widen the range of view
    public float viewportRangeMax = 1f;
    public float viewportRangeMin = 0f;
    public float lerpSpeed=1f;
    public float inactivityTimeout = 20f;
    public float touchDuration = 0.3f;
    public float distThreshold = 4f; 
    public Vector3 openBagHipPosition;
    public Vector3 openBagRotation;
    public Vector3 releasedPosition;
    public Transform head; 
    public AnimationCurve bagLerpCurve;
    public Coroutine timerRoutine = null;
    public Coroutine touchRoutine = null;
    public KitHipPlacement kitRotator; 

    public void Start()
    {
        hipParent = transform.parent;
        bagHomeRotation = transform.localEulerAngles;
        popout = GetComponent<popoutInstruments>(); 
        SetToViewport();
        bagHomePos = transform.localPosition;
        bagHomeRotation = transform.localEulerAngles;
        head = Camera.main.transform; 
    }
    private void Update()
    {
		if (SteamVR_Input._default.inActions.Teleport.GetStateDown(SteamVR_Input_Sources.Any))
        {
            ReturnToHip();            
        }

        if (SteamVR_Input._default.inActions.GrabPinch.GetStateDown(SteamVR_Input_Sources.Any))
        {
          //  Open();
        }

        SetToViewport();
        isSeen = IsWithinViewport(bagViewportPos);
    }

    //Grabbed() or Touched() is a ubiquitous function for any interactive. Each  interactive object, including the bag, would have Grabbed()
    //When event occurs, like a click, or touch, or button, (SendMessage "Grabbed") is called to that spcecific item
    //The bag here, when touched, opens the tools | but the tools when "Grabbed()" might instantiate or do something different
    //Easy way for player (mouse, controller, etc) to call the same function with one line of code, but each item does its own unique thing wihtin Grabbed()

    //public void Touched(bool state)
    //{
    //    isTouched = state; 
    //}

    public void Grabbed(Transform hand)
    {
        //Set a "follow hand" bool in update to match position instead?
        transform.SetParent(hand);
        
        if (!isOpen)
        {
            Open();
        }
    }

    public void Touched(bool state)
    {
        GetComponent<SelectionHighlight>().Highlight(state);
    }

    public void Released()
    {
        transform.SetParent(null);
        releasedPosition = transform.position;
        //StartCoroutine(EvaluateDistance()); 
    }

    public void Open()
    {
        //if the bag is in view and closed, open it
        //StartCoroutine(BagHipLerp(openBagHipPosition, openBagRotation));
        popout.OpenBag();
        isOpen = true; 
        //timerRoutine = StartCoroutine(TimeoutCheck()); 
    }

    //while the bag is within range, calculate its distance, when its out of range, return it
    IEnumerator EvaluateDistance()
    {
        Vector3 headPos = head.position;
        float dist = Vector3.Distance(releasedPosition, headPos);
        while(dist < distThreshold)
        {
            headPos = head.position;
            dist = Vector3.Distance(releasedPosition, headPos);
            print(dist.ToString()); 
            yield return null; 
        }
        print("returning"); 
        ReturnToHip();
    }
           
    public void ReturnToHip()
    {
        //StopCoroutine(timerRoutine); 
        transform.SetParent(hipParent); 
        //StartCoroutine(BagHipLerp(bagHomePos, bagHomeRotation));
        transform.localPosition = bagHomePos;        
        popout.CloseBag();
        kitRotator.StoreRotation();
        isOpen = false;
        GetComponent<Kit_UI>().Clear();
    }

    void SetToViewport()
    {
        //find the object's position on the viewport (normalized)
        bagViewportPos = Camera.main.WorldToViewportPoint(transform.position);        
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

    //Call with position of where you want bag to lerp to
    IEnumerator BagHipLerp(Vector3 destPos, Vector3 destRotate)
    {
        float timer = 0;
       // Vector3 startPos = transform.localPosition;
        Vector3 startRot = transform.localEulerAngles;

        while (timer < 1.0f)
        {
            //transform.localPosition = Vector3.Lerp(startPos, destPos, bagLerpCurve.Evaluate(timer));
            transform.localEulerAngles = new Vector3(Mathf.Lerp(startRot.x, destRotate.x, bagLerpCurve.Evaluate(timer)), (Mathf.Lerp(startRot.y, destRotate.y, bagLerpCurve.Evaluate(timer))), 0);
            timer += Time.deltaTime * lerpSpeed;
            yield return null;
        }
        //transform.localPosition = destPos;
        transform.localEulerAngles = new Vector3(destRotate.x, destRotate.y, 0);
        //if the passed argument was not the hip rotation, then that means the bag should be open
        if (destRotate != bagHomeRotation)
        {
            isOpen = true;
        }
        //transform.LookAt(Camera.main.transform);      
    }

    //IEnumerator TouchBag()
    //{
    //    float timer = touchDuration;
    //    while (timer > 0f)
    //    {
    //        timer -= Time.deltaTime;
    //        yield return null;
    //    }
    //    if (timer <= 0f)
    //    {
    //        Open();

    //    }
    //}

    //IEnumerator TimeoutCheck()
    //{
    //    timeoutRunning = true; 
    //    float timer = inactivityTimeout;            
    //    while(timer > 0f)
    //    {
    //        timer -= Time.deltaTime;
    //        yield return null; 
    //    }
    //    if(timer <= 0f)
    //    {
    //        timeoutRunning = false;
    //        ReturnToHip();
    //    }

    //}

    //public void ResetTimeout()
    //{
    //    //if the timer is running, reset it and keep it going
    //    //(only way to start timer if it isn't already running is through the Open() function

    //    if (timeoutRunning)
    //    {
    //        print("resetting");

    //        StopCoroutine(timerRoutine);

    //        timeoutRunning = false; 
    //        timerRoutine = StartCoroutine(TimeoutCheck());
    //    }
    //}
}
