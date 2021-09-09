using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CallActivationEnergy : MonoBehaviour
{
    private int i;
    public Button ActivationEnergyButton;

    public void CallEAJoules()
    {
        //this updates the total JoulesOfHeat variable from all bonds that the player has formed so far
        DisplayCanvasScript.JoulesOfHeat++;
        GameObject.Find("JouleHolder").GetComponent<JouleHolderScript>().JSpawn();
        DisplayCanvasScript.ActivationEnergyCount++;

        if(TutorialSpeechBubbleScript.TutorialMessageNumber == 10)
        {
            for (i = 1; i < 5; i++)
            {
                DisplayCanvasScript.JoulesOfHeat++;
                GameObject.Find("JouleHolder").GetComponent<JouleHolderScript>().JSpawn();  //spawn a total of 5 Joules of Ea in the tutorial round
                DisplayCanvasScript.ActivationEnergyCount++;
            }
            GameObject.Find("TutorialSpeechBubble").GetComponent<TutorialSpeechBubbleScript>().SendTutorialMessage();
        }

       
      

    }

    public void SparkIt()
    {
        for (i = 1; i < 6; i++)
        {
            DisplayCanvasScript.JoulesOfHeat++;
            GameObject.Find("JouleHolder").GetComponent<JouleHolderScript>().JSpawn();  //spawn a total of 5 Joules of Ea in the tutorial round
            DisplayCanvasScript.ActivationEnergyCount++;
        }

        ActivationEnergyButton.interactable = false;
    }

    
}
