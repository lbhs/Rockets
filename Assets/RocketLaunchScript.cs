using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RocketLaunchScript : MonoBehaviour
{
    //THIS SCRIPT IS ATTACHED TO THE READYTOLAUNCHBUTTON

    public GameObject UICanvas;
    public GameObject CanvasBehindScene;
    public GameObject DisplayCanvas;

    public GameObject LaunchTheRocketButton;
    public GameObject ReturnToGameButton;

    public GameObject BadRocket;
    public GameObject BetterRocket;
    public GameObject BestRocket;

    public AudioSource Music;

    public GameObject[] AllOxygenAtomsInScene;
    public GameObject [] AllHydrogenAtomsInScene;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LaunchTime()
    {
        AllOxygenAtomsInScene =  GameObject.FindGameObjectsWithTag("Oxygen");
        AllHydrogenAtomsInScene = GameObject.FindGameObjectsWithTag("Hydrogen");

        foreach (GameObject atom in AllOxygenAtomsInScene)
        {
            atom.SetActive(false);
        }

        foreach (GameObject atom in AllHydrogenAtomsInScene)
        {
            atom.SetActive(false);
        }


        Music.Stop();
        UICanvas.SetActive(false);
        CanvasBehindScene.SetActive(false);
        DisplayCanvas.SetActive(false);
        LaunchTheRocketButton.SetActive(true);
        ReturnToGameButton.SetActive(true);

        if(DisplayCanvasScript.NetEnergyProfit > 8)
        {
            BestRocket.SetActive(true);
        }
        else if (DisplayCanvasScript.NetEnergyProfit > 5)
        {
            BetterRocket.SetActive(true);
        }
        else
        {
            BadRocket.SetActive(true);
        }
    }

    public void EndLaunchScene()
    {
        UICanvas.SetActive(true);
        CanvasBehindScene.SetActive(true);
        DisplayCanvas.SetActive(true);
        LaunchTheRocketButton.SetActive(false);
        ReturnToGameButton.SetActive(false);

        BadRocket.SetActive(false);
        BetterRocket.SetActive(false);
        BestRocket.SetActive(false);

        foreach (GameObject atom in AllOxygenAtomsInScene)
        {
            atom.SetActive(true);
        }

        foreach (GameObject atom in AllHydrogenAtomsInScene)
        {
            atom.SetActive(true);
        }
    }






}
