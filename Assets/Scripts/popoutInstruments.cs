using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR; 

public class popoutInstruments : MonoBehaviour {

    public Transform popoutRoot, bagLid;
    public float speed;
    public AnimationCurve popoutCurve;
    public AnimationCurve retractCurve;
    public AnimationCurve xSpreadCurve;
    public AnimationCurve scaleCurve;
    public AnimationCurve lidOpenCurve;


    private Transform[] instruments;
    private Vector3[] positions;
    private Quaternion[] rotations;
    private Vector3[] scales;

    private Vector3 lidRotationDest;
    

    // Use this for initialization
    void Start () {
        //Find the bag lid, and set its rotation to zero
        //Also stores position it starts in, may not use
        lidRotationDest = bagLid.localEulerAngles;
        bagLid.localEulerAngles = Vector3.zero; 


        //Store all the transform info for any parented tools, then set their starting pos and scale
        int count = popoutRoot.childCount;
        instruments = new Transform[count];
        positions = new Vector3[count];
        rotations = new Quaternion[count];
        scales = new Vector3[count];

        //zero tools out into closed position
        for (int i = 0; i < count; i++) {
            instruments[i] = popoutRoot.GetChild(i);
            positions[i] = instruments[i].localPosition;
            rotations[i] = instruments[i].localRotation;
            scales[i] = instruments[i].localScale;
            instruments[i].localPosition = Vector3.zero;
            instruments[i].localScale *= 0.1f;
            //StartCoroutine(PopOpen(instruments[i]));
        }

    }
   
    
    public void OpenBag() {
        int count = popoutRoot.childCount;
        instruments = new Transform[count];
        for (int i = 0; i < count; i++) {
            instruments[i] = popoutRoot.GetChild(i);
            StartCoroutine(PopOpen(i));
        }        
    }

    public void CloseBag()
    {
        int count = popoutRoot.childCount;
        instruments = new Transform[count];
        for (int i = 0; i < count; i++)
        {
            instruments[i] = popoutRoot.GetChild(i);
            StartCoroutine(Close(i));
        }      
    }
    //Animates the tools from V3.zero, to their starting position in the scene, which is their specific menu position
    IEnumerator PopOpen(int instrumentIndex) {
        Transform instrument = instruments[instrumentIndex];
        Vector3 destScale = scales[instrumentIndex];
        Vector3 destPos = positions[instrumentIndex];
        Quaternion destRot = rotations[instrumentIndex];
        Vector3 startScale = instrument.localScale;
        Vector3 startPos = Vector3.zero; // to improve        
        float lerper = 0;
        while (lerper < 1.0f) {
            instrument.localPosition = Vector3.Lerp(startPos, destPos, popoutCurve.Evaluate(lerper));
            instrument.localScale = Vector3.Lerp(startScale,destScale, scaleCurve.Evaluate(lerper));
            bagLid.localEulerAngles = new Vector3(Mathf.Lerp(0, -90, lidOpenCurve.Evaluate(lerper)), 0, 0); 
            lerper += Time.deltaTime * speed;
            yield return null;
        }
        instrument.localPosition = destPos;
        instrument.localScale = destScale;

    }

    IEnumerator Close(int instrumentIndex) {
        print("closing"); 
        Transform instrument = instruments[instrumentIndex];
        Vector3 destScale = (scales[instrumentIndex]*01f);
        Vector3 destPos = Vector3.zero;
        Quaternion destRot = rotations[instrumentIndex];
        Vector3 startScale = instrument.localScale;
        Vector3 startPos = instrument.localPosition;
        float lerper = 0;
        while (lerper < 1.0f)
        {

            instrument.localPosition = Vector3.Lerp(startPos, destPos, popoutCurve.Evaluate(lerper));
            instrument.localScale = Vector3.Lerp(startScale, destScale, popoutCurve.Evaluate(lerper));
            bagLid.localEulerAngles = new Vector3(Mathf.Lerp(-90, 0, lidOpenCurve.Evaluate(lerper)), 0, 0);
            lerper += Time.deltaTime * speed;
            yield return null;
        }
        instrument.localPosition = destPos;
        instrument.localScale = destScale;

        //Vector3 destScale = instrument.localScale;
        //Vector3 destPos = instrument.localPosition;
        //Quaternion destRot = instrument.localRotation;
        //Vector3 startScale = instrument.localScale * 0.05f;
        //Vector3 startPos = Vector3.zero; // to improve
        //float lerper = 0;
        //while (lerper < 1.0f) {

        //    instrument.localPosition = Vector3.Lerp(startPos, destPos, popoutCurve.Evaluate(lerper));
        //    instrument.localScale = Vector3.Lerp(startScale, destScale, popoutCurve.Evaluate(lerper));
        //    lerper += Time.deltaTime * speed;
        //    yield return null;
        //}
        //instrument.localPosition = destPos;
        //instrument.localScale = destScale;
    }

    // Update is called once per frame
    void LateUpdate () {
        // set position of triage kit to player - do this later
        // transform.root.position = new Vector3(Camera.main.transform.position.x, 0, Camera.main.transform.position.z);
	}
}
