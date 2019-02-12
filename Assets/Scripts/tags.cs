using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tags : MonoBehaviour {

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Transform parent;
    private Vector3 localscale;
    public bool destroy = false;
    public Transform iconParent;
    public GameObject tagDisplay; 

    // Use this for initialization
    void Start () {
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
        parent = transform.parent;
        localscale = transform.localScale;
    }

    public void Grabbed(Transform hand) {
        //Debug.Log("tag " + gameObject.name + "picked up.");
        transform.SetParent(hand);
        hand.GetComponent<valveInput>().objectHeld = transform;
        GetComponent<SelectionHighlight>().Highlight(false); 
        

    }

    public void Touched(bool state)
    {
        if (state)
        {

        }

        GetComponent<SelectionHighlight>().Highlight(state); 

    }

    public void Released() {
       // Debug.Log("tag " + gameObject.name + "dropped.");
        //gameObject.GetComponent<Rigidbody>().isKinematic = false;
        transform.parent = null; 

        // make a new tag
        GameObject newTag = Instantiate(gameObject) as GameObject;
        newTag.GetComponent<Rigidbody>().isKinematic = true;
        newTag.transform.SetParent(parent);
        newTag.transform.localScale = localscale;
        newTag.transform.localPosition = initialPosition;
        newTag.transform.localRotation = initialRotation;
        //newTag.GetComponent<SelectionHighlight>().Highlight(Color.black); 
        if (destroy) {
        Destroy(this.gameObject);
        }
        


    }

    // Update is called once per frame
    void Update () {
        if (transform.position.y < -1f) Destroy(gameObject);
	}

    private void OnCollisionEnter(Collision collision) {
        // check if the tag lands on a patient and freeze it there and report it
    }

}
