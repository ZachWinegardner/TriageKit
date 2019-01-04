using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DragRigidbody : MonoBehaviour {

 
	public LayerMask raycastLayer;
    public bool debugMessages;
    public bool mouseMode;
	public float rotateSpeed;
	private Transform currentSelection;

    void Update()
    {
        if (mouseMode) {
            if (Input.GetKey("1")) currentSelection.Rotate(-rotateSpeed * Time.deltaTime, 0, 0, Space.Self);
            if (Input.GetKey("2")) currentSelection.Rotate(rotateSpeed * Time.deltaTime, 0, 0, Space.Self);
            if (Input.GetKey("3")) currentSelection.Rotate(0, -rotateSpeed * Time.deltaTime, 0, Space.Self);
            if (Input.GetKey("4")) currentSelection.Rotate(0, rotateSpeed * Time.deltaTime, 0, Space.Self);
            if (Input.GetKey("5")) currentSelection.Rotate(0, 0, -rotateSpeed * Time.deltaTime, Space.Self);
            if (Input.GetKey("6")) currentSelection.Rotate(0, 0, rotateSpeed * Time.deltaTime, Space.Self);

            // Make sure the user pressed the mouse down
            if (Input.GetMouseButtonDown(0)) {
                var mainCamera = FindCamera();
                // We need to actually hit an object
                RaycastHit hit = new RaycastHit();
                if ( Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition).origin,
                        mainCamera.ScreenPointToRay(Input.mousePosition).direction, out hit, 100, raycastLayer)) {
                    if (hit.collider.tag == "draggable") {
                        StartCoroutine(DragObject(hit.distance, hit.point, hit.collider.transform));
                    }
                    if (debugMessages) Debug.Log(hit.collider.name); // + " " + hit.textureCoord);
                }
               
             }
                

         }


        
    }

    IEnumerator DragFlat(Vector3 hitPos, Transform hitObj) {
        currentSelection = hitObj;
        var mainCamera = FindCamera();
        hitObj.gameObject.GetComponent<Collider>().enabled = false;
        Vector3 offset = hitObj.position - hitPos;
        RaycastHit hit = new RaycastHit();
        while (Input.GetMouseButton(0)) {
            
           if ( Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition).origin, mainCamera.ScreenPointToRay(Input.mousePosition).direction, out hit, 100, raycastLayer) )
            hitObj.position = hit.point + offset + new Vector3(0,0.1f,0);
            yield return null;
        }
        hitObj.gameObject.GetComponent<Collider>().enabled = true;
    }


	IEnumerator DragObject(float distance, Vector3 hitPos, Transform hitObj)
    {

        hitObj.SendMessage("Grabbed",SendMessageOptions.DontRequireReceiver);

		currentSelection = hitObj;
        var mainCamera = FindCamera();

		Vector3 offset = hitObj.position - hitPos;

		//Rigidbody rb = hitObj.GetComponent<Rigidbody> ();
		//bool wasKinematic = false;
		//if (rb.isKinematic)
		//	wasKinematic = true;
		//rb.isKinematic = true;

        while (Input.GetMouseButton(0))
        {
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
			hitObj.position = ray.GetPoint(distance) + offset;
            //if (hitObj.name.Contains("avatarPositioner")) hitObj.position = new Vector3(hitObj.position.x, 0.02f, hitObj.position.z);
            yield return null;
        }
        //if (!wasKinematic)
        //	rb.isKinematic = false;  

        hitObj.SendMessage("Released", SendMessageOptions.DontRequireReceiver);

    }

    public void Release() {

        StopAllCoroutines();
    }


    private Camera FindCamera()
    {
        if (GetComponent<Camera>())
        {
            return GetComponent<Camera>();
        }

        return Camera.main;
    }
}

