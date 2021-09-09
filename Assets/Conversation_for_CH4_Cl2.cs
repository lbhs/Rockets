using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Conversation_for_CH4_Cl2 : MonoBehaviour  //this script is attached to ConversationDisplay GameObject
{
    public static int messageStatus;   //goes from 1 to 4 to update student progress
    public Text ConversationTextBox;
    public GameObject TutorialCircle;

    // Start is called before the first frame update
    void Awake()
    {
        messageStatus = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Conversation_for_CH4_Cl2.messageStatus < 3 && DisplayCanvasScript.NetEnergyProfit == 1)
        {
            ConversationTextBox.text = "You have now made an Energy PROFIT!  Drag out another Cl2 molecule to make more profit!";
            TutorialCircle.SetActive(true);
            Conversation_for_CH4_Cl2.messageStatus = 2;
            //StartCoroutine(countdown());
        }

        if (Conversation_for_CH4_Cl2.messageStatus < 5 && DisplayCanvasScript.NetEnergyProfit == 2)
        {
            ConversationTextBox.text = "Keep Adding Cl atoms until you reach a Profit of 4 Joules!";
            Conversation_for_CH4_Cl2.messageStatus = 4;
            //StartCoroutine(countdown());
        }

        if (DisplayCanvasScript.NetEnergyProfit == 4)
        {
            ConversationTextBox.text = "You have achieved your goal!  GOOD JOB!!!";
            
        }

       


    }
}
