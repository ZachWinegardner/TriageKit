using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindVisibility : MonoBehaviour {

    //This script goes on the traige kit (rooted at hip) to check if it is within view
    //The kit G.O. never moves from the hip, so we need to check it, not the bag. 
    //When the bag is at the hip, its transform coincides with the kit's

    Vector3 objViewPos, lastPos, center;
    public bool isSeen = false;

    //adjustment to widen the range of view
    public float viewportRangeMax = 1f;
    public float viewportRangeMin = 0f;


    void Start () {
        center = new Vector3(0.5f, 0.5f, 0f);
        SetToViewport(); 
        lastPos = objViewPos;
        Debug.Log(objViewPos.ToString()); 
	}
	
	void Update () {
        SetToViewport();
        isSeen = IsWithinViewport(objViewPos);
        //if (objViewPos != lastPos)
        //{
        //    Debug.Log(objViewPos.ToString());
        //}
        lastPos = objViewPos;
    }

    void SetToViewport()
    {
        //find the object's position on the viewport (normalized)
        objViewPos = Camera.main.WorldToViewportPoint(transform.position);

        //set to 0 in case V3.Distance is used, z won't interfere
        //objViewPos.z = 0f;
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
}
