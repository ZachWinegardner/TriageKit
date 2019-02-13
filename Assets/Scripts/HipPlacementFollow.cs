using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HipPlacementFollow : MonoBehaviour {
    //This script ensures the kit will follow head rotation when rooted at hip
    //It also places the kit at the proper hip height based on the Y pos of the camera
    public float hipAngleAdjustment; 
    public bool followRotation = true;
    public float minHeadHieght, maxHeadHeight, minHip, maxHip;
    public AnimationCurve reorientCurve;
    public float lerpSpeed = 1f;
    public int threshold;
    public Transform yAxisFollower; 
    
    private BagScript bag;
    private Transform kit; 
    private Transform cam;
    private Vector3 kitPos;
    Quaternion startRotation;
    private Vector3 yFollow; 
    


    void Start () {
        cam = Camera.main.transform;
        bag = transform.GetChild(0).GetChild(0).GetComponent<BagScript>();
        kit = transform.GetChild(0).transform;
        yFollow = new Vector3(0f, cam.eulerAngles.y, 0f); 
        StoreRotation(); 
    }

    void Update() {
        //Root always follows pos of head
        //kit at hip is parented, but gets it y pos based on lerped pos of the head
        transform.position = cam.position;
        kitPos = kit.position;        
        float headPos = transform.position.y;
        float normalizedHeadHeight = (headPos - minHeadHieght) / (maxHeadHeight - minHeadHieght);
        headPos = Mathf.Clamp(headPos, minHeadHieght, maxHeadHeight);
        normalizedHeadHeight = Mathf.Clamp(normalizedHeadHeight, 0, 1);
        kitPos.y = Mathf.Lerp(minHip, maxHip, normalizedHeadHeight);
        kit.position = kitPos;

        if (followRotation && !bag.isOpen)
        {
            transform.localEulerAngles = new Vector3(0, cam.localEulerAngles.y - hipAngleAdjustment, 0);
        }

        //Follower only tracks y axis
        yFollow.y = cam.eulerAngles.y;
        yAxisFollower.eulerAngles = yFollow;

        //quaternion comparison only on y axis rotations
        float distance = Quaternion.Angle(yAxisFollower.rotation, startRotation);
        print(distance.ToString());

        if (distance >= threshold)
        {
            StoreRotation();
            StartCoroutine(ReorientBag()); 

        }


    }

    IEnumerator ReorientBag()
    {
        float lerper = 1f;
        Quaternion source = transform.rotation; 
        while (lerper > 0f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, startRotation, lerper); 
            lerper -= Time.deltaTime * lerpSpeed; 
            yield return null;

            //transform.localEulerAngles = new Vector3(0f, (Mathf.Lerp(transform.localEulerAngles.y, degree, reorientCurve.Evaluate(lerper))), 0f);    
        }
        //transform.localEulerAngles = new Vector3(0f, degree, 0f); 
        //transform.rotation = yAxisFollower.rotation; 
    }

    void StoreRotation()
    {
        startRotation = yAxisFollower.rotation;  
    }

   
}
