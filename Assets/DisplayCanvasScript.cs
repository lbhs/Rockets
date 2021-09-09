using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayCanvasScript : MonoBehaviour  //this script is attached to the "Display Canvas" GameObject 
{
    public static int JoulesOfHeat;
    public Text jouleText;
    //public static int ScoreTotal;
    public static int NetEnergyProfit;
    
    public Text scoreText;
    public static int ActivationEnergyCount;
    public TMP_Text EnergyInvestmentText;       // Text 
    public TMP_Text HeatReleasedWhenFormingBondsText;  //new method of accounting--need to keep track of energy changes upon bonding and unbonding events (Bondmaker and UnbondingScript2)
    public TMP_Text PotentialEnergyText;
    public TMP_Text NetEnergyProfitText;
    public TMP_Text DeltaHText;

    public bool PotentialEnergyExample;
    public static int HeatInvestedToBreakBonds;
    public static int PotentialEnergyInScene;
    public static int HeatReleasedWhenBondsForm;
    public int InitialPE;
    public int InitialHeatJoules;
    

    // Start is called before the first frame update
    void Start()
    {
        JoulesOfHeat = 0;
        //ScoreTotal = 0;
        NetEnergyProfit = 0;
        ActivationEnergyCount = 0;
        HeatReleasedWhenBondsForm = 0;
        HeatInvestedToBreakBonds = 0;

        //PotentialEnergyInScene = InitialPE;  //So far, this is only used in the Potential Energy Example scene
    }

    // Update is called once per frame
    void Update()
    {
        if (PotentialEnergyExample)
        {
            NetEnergyProfit = HeatReleasedWhenBondsForm - HeatInvestedToBreakBonds;            
            EnergyInvestmentText.text = "Heat Invested to break bonds = " + HeatInvestedToBreakBonds.ToString() + " Joules";
            HeatReleasedWhenFormingBondsText.text = "Heat gained when bonds form = " + HeatReleasedWhenBondsForm.ToString() + " Joules";
            NetEnergyProfitText.text = "Net Profit of Heat = " + NetEnergyProfit.ToString() + " Joules ";
            PotentialEnergyText.text = "Potential Energy = " + PotentialEnergyInScene.ToString() + " Joules";
        }

        if (GameObject.Find("BondEnergyMatrixAdvanced"))
        {
            NetEnergyProfit = HeatReleasedWhenBondsForm - HeatInvestedToBreakBonds;
            EnergyInvestmentText.text = "Heat Invested to break bonds = " + HeatInvestedToBreakBonds.ToString() + " kJ/mole";
            HeatReleasedWhenFormingBondsText.text = "Heat gained when bonds form = " + HeatReleasedWhenBondsForm.ToString() + " kJ/mole";
            NetEnergyProfitText.text = "Net Profit of Heat = " + NetEnergyProfit.ToString() + " kJ/mole ";
            int DeltaH = -NetEnergyProfit;
            if(DeltaH > 0)
            {
                DeltaHText.text = "ΔH = +" + -NetEnergyProfit + " kJ/mole";
            }
            else
            {
                DeltaHText.text = "ΔH = " + -NetEnergyProfit + " kJ/mole";
            }
            
        }

        else
        {
            NetEnergyProfit = HeatReleasedWhenBondsForm - HeatInvestedToBreakBonds;      
            EnergyInvestmentText.text = "Heat Invested to break bonds = " + HeatInvestedToBreakBonds.ToString() + " Joules";
            HeatReleasedWhenFormingBondsText.text = "Heat gained when bonds form = " + HeatReleasedWhenBondsForm.ToString() + " Joules";
            NetEnergyProfitText.text = "Net Profit of Heat = " + NetEnergyProfit.ToString() + " Joules ";
        }
        
        //ScoreTotal = 10 * JoulesOfHeat + NetEnergyProfit;
        //scoreText.text = ScoreTotal.ToString() + " Total Points";
        
    }

    public void ResetValuesForScene()  //this is not used yet.  Was created because restarting the PotentialEnergyExample scene didn't produce the right values in Display Canvas
    {
        JoulesOfHeat = 0;
        //ScoreTotal = 0;
        NetEnergyProfit = 0;
        HeatInvestedToBreakBonds = 0;
        HeatReleasedWhenBondsForm = 0;
        PotentialEnergyInScene = InitialPE;
    }

}
