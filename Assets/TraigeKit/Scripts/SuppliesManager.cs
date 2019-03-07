using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuppliesManager : MonoBehaviour
{

    public static SuppliesManager instance = null;
    public static SuppliesManager Instance { get { return instance; } }

    public int surgTapeCount, gauzeCount, occluCount, splintCount, tournCount, shearsCount, needleCount, nasoCount;
    public int[] counts;
    public int supplyResetCount = 5; 

    void Awake()
    {
        if (instance == null)

            instance = this;

        else if (instance != this)

            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        counts = new int[] { surgTapeCount, gauzeCount, occluCount, splintCount, tournCount, shearsCount, needleCount, nasoCount };
      
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            for(int i = 0; i<counts.Length; i++)
            {
                counts[i] = supplyResetCount; 
            }
        }
    }   

}
