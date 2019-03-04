using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kit_UI : MonoBehaviour {

    public GameObject featuredInstrument, goneText;
    public Transform featureLocation;
    public TextMesh text;

    private Vector3 featureRotation; 

    public InstrumentGrabbing prefabStats;

    private void Start()
    {
        text.text = null;
        featureRotation = new Vector3(0, 0, 0); 
    }

    public void ShowInstrument(GameObject instrument)
    {
        if (instrument != featuredInstrument)
        {
            Destroy(featuredInstrument);
            featureRotation = instrument.transform.localEulerAngles;
           // print(featureRotation.ToString()); 
            featuredInstrument = Instantiate(instrument, featureLocation.position, Quaternion.identity, featureLocation) as GameObject;
            featuredInstrument.transform.localEulerAngles = featureRotation;
           // print(featuredInstrument.transform.localEulerAngles.ToString()); 
            
            prefabStats = featuredInstrument.GetComponent<InstrumentGrabbing>();
            text.text = prefabStats.type.ToString() + ": " + SuppliesManager.instance.counts[(int)prefabStats.type];
            prefabStats.selfPrefab = instrument; 
        }
    }

    public void Clear()
    {
        Destroy(featuredInstrument);
        featuredInstrument = null;
        text.text = null;
    }

    public void InstrumentGrabbed()
    {
        text.text = prefabStats.type.ToString() + ": " + SuppliesManager.instance.counts[(int)prefabStats.type];
        featuredInstrument = null;
    }


}
