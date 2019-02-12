using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instrument_UI : MonoBehaviour {

    public GameObject featuredInstrument;
    public Transform featureLocation;
    public TextMesh text;

    public InstrumentGrabbing prefabStats;

    private void Start()
    {
        text.text = null; 
    }

    public void ShowInstrument(GameObject instrument)
    {
        if (instrument != featuredInstrument)
        {
            Destroy(featuredInstrument); 
            featuredInstrument = Instantiate(instrument, featureLocation.position, Quaternion.identity, transform) as GameObject;
            prefabStats = featuredInstrument.GetComponent<InstrumentGrabbing>();
            text.text = prefabStats.type.ToString() + ": " + SuppliesManager.instance.counts[(int)prefabStats.type];
        }
    }

    public void InstrumentGrabbed()
    {
        featuredInstrument = null;
        text.text = null; 
    }


}
