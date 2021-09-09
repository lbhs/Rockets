using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JouleHolderScript : MonoBehaviour
{
    public GameObject JUIPrefab;
    public Text JoulesOfHeatInCorral;
    public GameObject DisplayCanvas;
    private int i;


    void Start()
    {
        if (DisplayCanvas.GetComponent<DisplayCanvasScript>().PotentialEnergyExample)
        {
            for (i = 1; i < 4; i++)
            {
                DisplayCanvasScript.JoulesOfHeat++;
                JSpawnPE();
            }

        }
    }

    public void JSpawn()  //This function is called from the JewelMover script that uses the BondEnergy variable passed from BondMaker script 
    {
        GameObject JouleInCorral = Instantiate(JUIPrefab);        
        JouleInCorral.transform.parent = gameObject.transform;
        JouleInCorral.transform.localPosition = new Vector3(Random.Range(-50, 50), Random.Range(-36, 50), 0);  //instatiates joule at a random position inside the corral
        AdjustJoulesInCorral();
    }

    public void JSpawnPE()  //This function is called from the JewelMover script that uses the BondEnergy variable passed from BondMaker script 
    {
        GameObject JouleInCorral = Instantiate(JUIPrefab);
        JouleInCorral.transform.localScale = new Vector2(0.30f, 0.30f);
        JouleInCorral.transform.parent = gameObject.transform;
        JouleInCorral.transform.localPosition = new Vector3(Random.Range(-50, 50), Random.Range(-36, 50), 0);  //instatiates joule at a random position inside the corral
        AdjustJoulesInCorral();
    }

    public void AdjustJoulesInCorral()  //this displays text to count the joules and the number of points for these joules
    {
        JoulesOfHeatInCorral.text = DisplayCanvasScript.JoulesOfHeat + " Joules of Heat"; 
    }




    private void FixedUpdate()
    {
        //JoulesOfHeatInCorral.text = DisplayCanvasScript.JoulesOfHeat + " Joules of Heat";
    }

}


