using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followHeadRotations : MonoBehaviour {

    public Vector3 hipOffset;
    public float adjustment; 
    public float lookThreshold; 
    public bool followRotation = true;

    private FindVisibility kitVis;
    private BagScript bag; 
    private Transform cam;


    void Start () {
        cam = Camera.main.transform;
        //kitVis = transform.GetChild(0).GetComponent<FindVisibility>();
        bag = transform.GetChild(0).GetChild(0).GetComponent<BagScript>(); 
	}
	
	void Update () {
        //kit always at position of head, with offset adjustment
        transform.position = cam.position + hipOffset;        

        if (followRotation && !bag.isOpen)
        {
            transform.localEulerAngles = new Vector3(0, cam.localEulerAngles.y-adjustment, 0);
        }
        
	}
}
