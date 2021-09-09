﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameHidingScript : MonoBehaviour
{
    //This script is attached to the FlameController GameObject and controls visibility of the Unbonding Flame Icon

    public GameObject FlameIcon;

    // Start is called before the first frame update
    void Start()
    {
        //FlameIcon = GameObject.Find("FlameUIElement");
        if(GameObject.Find("TutorialMarker").GetComponent<TutorialScript>().Tutorial == true)
        {
            FlameIcon.SetActive(false);
        }
        
    }

    public void FlameOn()  //called from GoldenJewelMover script
    {
        FlameIcon.SetActive(true);
    }

    public void FlameOff()  //called from GoldenJewelMover script
    {
        FlameIcon.SetActive(false);
    }

    
}
