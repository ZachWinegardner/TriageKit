using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuppliesManager : MonoBehaviour
{

    public static SuppliesManager instance = null;
    public static SuppliesManager Instance { get { return instance; } }

    public int surgTapeCount, gauzeCount, occluCount, splintCount, tournCount, shearsCount, needleCount, nasoCount;
    //TextMesh tapeText, gauzeText, splintText, tournText, needleText;
    public int[] counts;
   // public TextMesh[] texts; 

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
        //texts = new TextMesh[] { tapeText, gauzeText, splintText, tournText, needleText }; 
      
    }
    //public void SetText()
    //{
    //    for (int i = 0; i < counts.Length; i++){
    //        texts[i].text = counts[i].ToString(); 
    //    }
    //}

}
