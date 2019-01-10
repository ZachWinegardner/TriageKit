using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HipPlacementFollow : MonoBehaviour {
    //This script ensures the kit will follow head rotation when rooted at hip
    //It also places the kit at the proper hip height based on the Y pos of the camera
    public float hipAngleAdjustment; 
    public bool followRotation = true;
    public float minHeadHieght, maxHeadHeight, minHip, maxHip; 
    
    private BagScript bag;
    private Transform kit; 
    private Transform cam;
    private Vector3 kitPos; 
    


    void Start () {
        cam = Camera.main.transform;
        bag = transform.GetChild(0).GetChild(0).GetComponent<BagScript>();
        kit = transform.GetChild(0).transform;        

    }

    void Update () {
        //Root always follows pos of head
        //kit at hip is parented, but gets it y pos based on half the pos of the head
        transform.position = cam.position;
        kitPos = (kit.position);

        float headPos = transform.position.y;
        float normalizedHeadHeight = (headPos - minHeadHieght) / (maxHeadHeight - minHeadHieght);
        headPos = Mathf.Clamp(headPos, minHeadHieght, maxHeadHeight);
        normalizedHeadHeight = Mathf.Clamp(normalizedHeadHeight, 0, 1);

        kitPos.y = Mathf.Lerp(minHip, maxHip, normalizedHeadHeight); 
        kit.position = kitPos; 

        if (followRotation && !bag.isOpen)
        {
            transform.localEulerAngles = new Vector3(0, cam.localEulerAngles.y- hipAngleAdjustment, 0);
        }
        
	}

   
}
