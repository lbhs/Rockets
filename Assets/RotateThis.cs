using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RotateThis : MonoBehaviour
{
    private float holdTime = 0;
    private Vector2 initialMousePosition;
    private Vector2 finalMousePosition;

    public const double holdTimeThreshold = 0.2; //let go within 0.2 seconds to rotate--no longer valid in this Shahrestany-modified script
    public static bool DisableRotation;


    private void OnMouseDown()
    {
        holdTime = 0;
        initialMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    void Update()
    {
        holdTime += Time.deltaTime;
    }
    private void OnMouseUp()
    {
        finalMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (JewelMover.JewelsInMotion == true  || DisableRotation == true)
        {  //disable rotation while jewels are moving
            print("Can't rotate while jewels are moving");
            return;
        }

        else if (Math.Abs(initialMousePosition.x - finalMousePosition.x) < 0.1 && Math.Abs(initialMousePosition.y - finalMousePosition.y) < 0.1)
        {
            if (gameObject.GetComponent<BondMaker>().bonded == false)
            {
               transform.Rotate(0, 0, 90);

                if (TutorialSpeechBubbleScript.TutorialMessageNumber == 19)  //message 19 = rotate the hydrogen atom
                {                        
                    if (TutorialSpeechBubbleScript.RotateMessageGiven > 0)
                    {
                        GameObject.Find("TutorialSpeechBubble").GetComponent<TutorialSpeechBubbleScript>().SendTutorialMessage();
                        TutorialSpeechBubbleScript.RotateMessageGiven = -1;
                        //DisableRotation = true;   //don't allow user to over-rotate?

                    }

                    TutorialSpeechBubbleScript.RotateMessageGiven++; //increment this variable so that two rotation events are completed
                }

                if (TutorialSpeechBubbleScript.TutorialMessageNumber == 23)  //message 23 = rotate the hydrogen atom
                {
                    print("rotate message given =" + TutorialSpeechBubbleScript.RotateMessageGiven);
                    if (TutorialSpeechBubbleScript.RotateMessageGiven > 0)
                    {
                        GameObject.Find("TutorialSpeechBubble").GetComponent<TutorialSpeechBubbleScript>().SendTutorialMessage();
                        //TutorialSpeechBubbleScript.RotateMessageGiven = 0;
                        //DisableRotation = true;   //don't allow user to over-rotate?

                    }

                    TutorialSpeechBubbleScript.RotateMessageGiven++; //increment this variable so that two rotation events are completed
                }




            }
        }


    }
}


