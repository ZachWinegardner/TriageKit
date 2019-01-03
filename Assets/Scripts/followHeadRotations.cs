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
        kitVis = transform.GetChild(0).GetComponent<FindVisibility>();
        bag = transform.GetChild(0).GetChild(0).GetComponent<BagScript>(); 
	}
	
	void Update () {
        //kit always at position of head, with offset adjustment
        transform.position = cam.position + hipOffset;

        if (kitVis.isSeen && followRotation)
        {
            followRotation = false;
        }

        if (!kitVis.isSeen && !followRotation)
        {
            followRotation = true;
        }


        ////if head looks down enough, freeze the rotation of the kit
        //if (cam.localEulerAngles.x > lookThreshold && cam.localEulerAngles.x < 120f && followRotation)
        //{
        //    followRotation = false;
        //}

        ////if head looks back upward, make the kit follow head rotation again
        //if (cam.localEulerAngles.x < lookThreshold && !followRotation || cam.localEulerAngles.x > 120f && !followRotation)
        //{
        //    followRotation = true;
        //}


        //if looking ahead or upward, follow head rotations. This ensures the bag to be in the relative side hip position when we are ready to look at it
        if (followRotation || bag.transform.parent != kitVis.transform)
        {
            transform.localEulerAngles = new Vector3(0, cam.localEulerAngles.y-adjustment, 0);
        }
        
	}
}
