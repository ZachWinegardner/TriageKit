using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LidOpener : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		

        if (Input.GetKeyDown(KeyCode.L))
        {
            StartCoroutine(openLid()); 
        }
	}

    IEnumerator openLid()
    {
        
        float lerper = 0f;
        float blend = 0f; 
        while(lerper < 1f)
        {
            GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, blend);
            blend += Time.deltaTime * 100f;
            lerper += Time.deltaTime;
            yield return null;

        }
        GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, 100f);

        yield return null;

    }
}
