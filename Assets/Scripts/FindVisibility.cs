using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindVisibility : MonoBehaviour {

    //This script goes on the traige kit to check if it is within view

    Vector3 objViewPos, lastPos, center;
    public bool isSeen = false;

	void Start () {
        center = new Vector3(0.5f, 0.5f, 0f);
        SetToViewport(); 
        lastPos = objViewPos;
        Debug.Log(objViewPos.ToString()); 
	}
	
	void Update () {
        SetToViewport();
        isSeen = IsWithinViewport(objViewPos);
        if (objViewPos != lastPos)
        {
            Debug.Log(objViewPos.ToString());
        }
        lastPos = objViewPos;
    }

    void SetToViewport()
    {
        //find the object's position on the viewport (normalized)
        objViewPos = Camera.main.WorldToViewportPoint(transform.position);

        //set to 0 in case V3.Distance is used, z won't interfere
        objViewPos.z = 0f;
    }

    public bool IsWithinViewport(Vector3 obj)
    {
        //see if the object's viewport position is within the normalized (0,0) to (1,1) space
        if (obj.x >= 0f && obj.x <= 1f && obj.y >= 0f && obj.y <= 1f)
        {
            return true;
        }
        else return false; 

    }
}
