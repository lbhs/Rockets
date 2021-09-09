using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConversationTextDisplayScript: MonoBehaviour  //this script is attached to ConversationDisplay GameObject
{
    //public Text ConversationTextBox;
    public TMP_Text ConversationTextBox;
    //public TextMeshProUGUI TutorialTextBox;
    //public TextMeshProUGUI TutorialTextBoxRight;
    public static bool final = false;
    public Color PEtoHeat;
    public Color HeattoPE;
    public Color DieColor;
    public Color ScoreColor;
    public Color NiceGreenText;
    //public GameObject DiceActiveOrNot;
    //public GameObject TutorialLeftSpeechBubble;
    //public GameObject TutorialRightSpeechBubble;
    //public GameObject TutorialArrow;
    //public Sprite endgametextsprite;
    //public GameObject TutorialCircle;
    public Vector2 TextDisplayLocation;
    public GameObject Reactant3DImage;
    
    // Start is called before the first frame update
    void Start()
    {
        final = false;

        StartCoroutine("countdown");
        //ConversationTextBox.text = null;
        //TextDisplayLocation = gameObject.transform.position  MAKE THIS AUTOMATICALLY NOTE THE INITIAL POSITION OF THE CONVERSATION TEXT DISPLAY!


        if(Conversation_for_CH4_Cl2.messageStatus == 1)
        {
            ConversationTextBox.text = "Drag Out a Cl2 Molecule to react with the CH4 molecule";
        }
        
      
    }
   


     public void Denied()
    {
        ConversationTextBox.GetComponent<RectTransform>().anchoredPosition = new Vector2(640, 240);  //new line of code!
        ConversationTextBox.fontSize = 30;  //new line of code!
        ConversationTextBox.text = "You don't have enough Joules to break this bond!";
        StartCoroutine(countdown());
    }

    public void noStack()
    {
        ConversationTextBox.text = "Drag your molecule into an EMPTY SPACE in the playing field.";
        ConversationTextBox.color = Color.yellow;
        StartCoroutine(countdown());
    }

    public void NoBondToBreak()
    {
        ConversationTextBox.text = "This Bond is Already Broken";
        StartCoroutine(countdown());
    }

    public void HeatToPEConversion(int JouleCost)
    {
       
       ConversationTextBox.color = HeattoPE;
        if (GameObject.Find("BondEnergyMatrixAdvanced"))
        {        
            ConversationTextBox.text = JouleCost.ToString() + " kJ/Mole of Heat converted to Potential Energy";         
        }

        else
        {
            ConversationTextBox.text = JouleCost.ToString() + " Joules of Heat converted to Potential Energy";
        }

        StartCoroutine(countdown());
    }

    public void PEtoHeatConversion(int BondEnergy)
    {
        ConversationTextBox.color = PEtoHeat;

        if (GameObject.Find("BondEnergyMatrixAdvanced")) 
        {
            print("Printing AdvancedBondEnergy = " + BondEnergy);
            ConversationTextBox.text = BondEnergy.ToString() + " kJ/Mole of Potential Energy converted to Heat";
            //Invoke("Profit1", 6);  This function is not in the "Conversation_for_CH4_Cl2" script
        }

        else
        {
            ConversationTextBox.text = BondEnergy.ToString() + " Joules of Potential Energy converted to Heat";            
            
        }

        StartCoroutine(countdown());

    }

    


    public void CantSwap()
    {
        ConversationTextBox.text = "You can only swap UNBONDED atoms";
        StartCoroutine(countdown());
    }

    public void CantRotate()
    {
        ConversationTextBox.text = "You can only rotate UNBONDED atoms";
        StartCoroutine(countdown());
    }

    public void ReachedMaximumNumberOfMolecules()
    {
        ConversationTextBox.text = "You already have reached the limit of SIX molecules in play.";
        StartCoroutine(countdown());
    }

    

    private IEnumerator countdown()  //this is a co-routine, can run in parallel with other scripts/functions
    {
        yield return new WaitForSeconds(6);

        if(Reactant3DImage != null)
        {
            Reactant3DImage.SetActive(false);
        }
               
        ConversationTextBox.text = null;
        ConversationTextBox.color = Color.white;
        //ConversationTextBox.fontStyle = TMP_Style;

        ConversationTextBox.GetComponent<RectTransform>().anchoredPosition = TextDisplayLocation;   //This location differs depending on the location of the product molecule checkboxes.  new Vector2(262, 78)
        ConversationTextBox.fontSize = 24;
        
        yield break;
    }
}



