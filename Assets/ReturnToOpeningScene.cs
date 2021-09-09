using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToOpeningScene : MonoBehaviour
{
    public GameObject DisplayCanvas;

    // Start is called before the first frame update
    public void OpeningSceneLoader()
    {
        //RESET ALL THE STATIC VARIABLES!!!
        TutorialSpeechBubbleScript.DraggingDisabled = false;
        DisplayCanvasScript.ActivationEnergyCount = 0;
        DisplayCanvasScript.JoulesOfHeat = 0;
        DisplayCanvasScript.HeatInvestedToBreakBonds = 0;
        DisplayCanvasScript.HeatReleasedWhenBondsForm = 0;
        DisplayCanvasScript.PotentialEnergyInScene = 0;
        DisplayCanvasScript.NetEnergyProfit = 0;


        //DisplayCanvas.GetComponent<DisplayCanvasScript>().ResetValuesfor
        ButtonsDelayedAppearance.frameMarker = 160;
        SceneManager.LoadScene(0);
    }
}
