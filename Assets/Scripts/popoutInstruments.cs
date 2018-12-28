using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class popoutInstruments : MonoBehaviour {

    public Transform popoutRoot;
    public float speed;
    public AnimationCurve popoutCurve;
    public AnimationCurve xSpreadCurve;
    public AnimationCurve retractCurve;

    private Transform[] instruments;
    private Vector3[] positions;
    private Quaternion[] rotations;
    private Vector3[] scales;

	// Use this for initialization
	void Start () {

        int count = popoutRoot.childCount;
        instruments = new Transform[count];
        positions = new Vector3[count];
        rotations = new Quaternion[count];
        scales = new Vector3[count];
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

    public void Grabbed() {
        int count = popoutRoot.childCount;
        instruments = new Transform[count];
        for (int i = 0; i < count; i++) {
            instruments[i] = popoutRoot.GetChild(i);
            StartCoroutine(PopOpen(i));
        }
    }

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
            instrument.localScale = Vector3.Lerp(startScale,destScale, popoutCurve.Evaluate(lerper));
            lerper += Time.deltaTime * speed;
            yield return null;
        }
        instrument.localPosition = destPos;
        instrument.localScale = destScale;
    }

    IEnumerator Close(Transform instrument) {
        Vector3 destScale = instrument.localScale;
        Vector3 destPos = instrument.localPosition;
        Quaternion destRot = instrument.localRotation;
        Vector3 startScale = instrument.localScale * 0.05f;
        Vector3 startPos = Vector3.zero; // to improve
        float lerper = 0;
        while (lerper < 1.0f) {

            instrument.localPosition = Vector3.Lerp(startPos, destPos, popoutCurve.Evaluate(lerper));
            instrument.localScale = Vector3.Lerp(startScale, destScale, popoutCurve.Evaluate(lerper));
            lerper += Time.deltaTime * speed;
            yield return null;
        }
        instrument.localPosition = destPos;
        instrument.localScale = destScale;
    }

    // Update is called once per frame
    void LateUpdate () {
        // set position of triage kit to player - do this later
        // transform.root.position = new Vector3(Camera.main.transform.position.x, 0, Camera.main.transform.position.z);
	}
}
