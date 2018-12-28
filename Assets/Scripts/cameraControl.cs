using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraControl : MonoBehaviour {

    public bool flatMovement = false;
	public float moveSpeed = 1.0f;
	public float rotateSpeed = 1.0f;
    //public bool rotateOnlyWhenMoving = true;

    private float height;

	// Use this for initialization
	void Start () {
        height = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {



		float x = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
		float z = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
		float y = 0; 
		if (Input.GetKey (KeyCode.E)) y =  Time.deltaTime * moveSpeed;
		if (Input.GetKey (KeyCode.Q)) y = Time.deltaTime * -moveSpeed;
		transform.Translate(x, y, z);

        if (flatMovement) transform.position = new Vector3(transform.position.x, height, transform.position.z);

		//if (rotateOnlyWhenMoving && ( x != 0 || z != 0 || y != 0 ) ) {
        if (Input.GetMouseButton(1)) {
			float pan = Input.GetAxis ("Mouse X") * Time.deltaTime * rotateSpeed;
			float tilt = -Input.GetAxis ("Mouse Y")* Time.deltaTime * rotateSpeed;
			transform.Rotate(0,pan,0,Space.World);
			transform.Rotate(tilt,0,0);
		}
		
	}
}
