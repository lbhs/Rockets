using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TutorialSpeechBubbleScript : MonoBehaviour
{
    public GameObject SpeechBubble;
    public TMP_Text TextboxInSpeechBubble;
    public Button ContinueButton;
    public float Xpos;
    public float Ypos;
    public static int TutorialMessageNumber;
    public static int RotateMessageGiven;  //used in the tutorial to know when to advance to making a bond (Tutorial message #18)
    public List<string> TutorialMessages = new List<string>();
    public GameObject RedCircle;
    public GameObject BlackCircle;
    public GameObject GrayOval;
    public GameObject TutorialArrow;
    public Sprite TutorialTargetMoleculeImage2;
    public GameObject ClickArrowRight;
    public GameObject ClickArrowLeft;
    public GameObject ClickArrowRightSprite;
    public GameObject PanelToShieldFlameEaButtonAndJouleCorral;  //used in the tutorial to prevent the user from interacting with buttons before they have received instruction
    public GameObject PanelToShieldO2UI;  //used in the  tutorial to prevent the user from dragging out molecules before they have received instruction
    public GameObject[] BlueJoulesInScene;
    public GameObject HydrogenToBreakOff;  //this is the hydrogen on the top of the formaldehyde molecule.  Used to set coordinates for unbonding flame even if formaldehyde molecule has been dragged around
    public GameObject MainCam;
    public GameObject ArrowForUnbondingFlameTarget;
    public GameObject SecondArrowForUnbondingFlameTarget;
    public GameObject MoleculeCheckBox1;
    public GameObject MoleculeCheckBox2;
    private int i;
    public Sprite ClickArrowLeftSprite;
    public static bool DraggingDisabled;
    public GameObject FlameImage;
    public Button ActivationEnergyButton;
    

    Camera cam;
    
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        TutorialMessageNumber = 0;
        RotateMessageGiven = 0;
        TextboxInSpeechBubble.text = "" + TutorialMessages[TutorialMessageNumber];
        ClickArrowLeft.GetComponent<RectTransform>().anchoredPosition = new Vector2(-170, -160);

        //DISABLE DRAGGING OF THE FORMALDEHYDE MOLECULE??  OR FIND THE POSITION OF THE C-H BOND. . .
        DraggingDisabled = true;  //only allow dragging of atoms when needed in the tutorial
        FlameImage.GetComponent<UIDrop2World>().enabled = false;    //ability to break bonds will be regulated
        ActivationEnergyButton.interactable = false;  //only need this to be active once in the tutorial
        MoleculeCheckBox1.GetComponent<Button>().interactable = false;
        MoleculeCheckBox2.GetComponent<Button>().interactable = false;

    }

    // Update is called once per frame
    void Update()
    {
        //THE CODE BELOW IS USED TO TEST THE GETMOLECULESCREENPOSITION FUNCTION (attached to the Main Camera)
        //if (Input.GetKeyDown("n"))  //this will get coordinates of Molecule1!
        //{
        //    Vector2 TargetHydrogenPosition = MainCam.GetComponent<SpriteWorldToScreenCoordinates>().GetMoleculeScreenPosition(HydrogenToBreakOff);
        //    print("Target = " + TargetHydrogenPosition);

        //}
    }

    public void SendTutorialMessage()
    {
        TutorialMessageNumber++;
        print("TutorialMessage# = " + TutorialMessageNumber);
        TextboxInSpeechBubble.text = "" + TutorialMessages[TutorialMessageNumber];
        MoveCircles();
        
    }

    public void GoBack()
    {
        TutorialMessageNumber--;
        TextboxInSpeechBubble.text = "" + TutorialMessages[TutorialMessageNumber];
    }

    public void MoveCircles()
    {
        if (TutorialMessageNumber == 1)  //Identifies formaldehyde as a reactant molecule
        {           
            BlackCircle.GetComponent<RectTransform>().anchoredPosition = new Vector2(-200, -370);
            BlackCircle.GetComponent<RectTransform>().localScale = new Vector2(2f, 2f);
            //TutorialArrow.GetComponent<RectTransform>().anchoredPosition = new Vector2(-300, -500);
            //TutorialArrow.transform.eulerAngles = new Vector3(0, 0, 50);
            //TutorialArrow.GetComponent<RectTransform>().localScale = new Vector2(1f, 1f);
            //TutorialArrow.GetComponent<Image>().color = new Color32(0, 0, 0, 255);
            ClickArrowLeft.GetComponent<RectTransform>().anchoredPosition = new Vector2(2000, -160);
            //ContinueButton.interactable = false;            
        }


        if (TutorialMessageNumber == 2)  //Identifies oxygen molecule as a reactant
        {
            BlackCircle.GetComponent<RectTransform>().anchoredPosition = new Vector2(245, -84);
            BlackCircle.GetComponent<RectTransform>().localScale = new Vector2(1.4f, 1f);
            //TutorialArrow.GetComponent<RectTransform>().anchoredPosition = new Vector2(140  , -210);
            ////TutorialArrow.transform.eulerAngles = new Vector3(0, 0, 50);
            ////TutorialArrow.GetComponent<RectTransform>().localScale = new Vector2(1f, 1f);
            //TutorialArrow.GetComponent<Image>().color = new Color32(208, 0, 0, 255);   //red color for oxygen
            //ClickArrowLeft.GetComponent<RectTransform>().anchoredPosition = new Vector2(2000, -160);
            //ContinueButton.interactable = false;            
            //SET OXYGEN MOLECULE POSITION SO THAT THE ARROW FOR BOND BREAKING WILL LINE UP--CAN TRY TO DO THIS WITH RIGIDBODY.MOVEPOSITION
        }

        if (TutorialMessageNumber == 3)  //Identifies CO2 and H2O as product molecules
        {
            BlackCircle.GetComponent<RectTransform>().anchoredPosition = new Vector2(-10, -660);
            BlackCircle.GetComponent<RectTransform>().localScale = new Vector2(1.4f, 0.7f);
            //TutorialArrow.GetComponent<RectTransform>().anchoredPosition = new Vector2(-180, -670);
            //TutorialArrow.transform.eulerAngles = new Vector3(0, 0, 0);            
            //TutorialArrow.GetComponent<Image>().color = new Color32(200, 200, 200, 255);  //gray color for hydrogen
            
            
        }


        if (TutorialMessageNumber == 4)  //Visual hints available by clicking the buttons
        {
            MoleculeCheckBox1.GetComponent<Button>().interactable = true;
            MoleculeCheckBox2.GetComponent<Button>().interactable = true;
            BlackCircle.GetComponent<RectTransform>().anchoredPosition = new Vector2(2000, -660);
            //TutorialArrow.GetComponent<RectTransform>().anchoredPosition = new Vector2(2000, -670);
            //TutorialArrow.transform.eulerAngles = new Vector3(0, 0, 0);
            //TutorialArrow.GetComponent<Image>().color = new Color32(200, 200, 200, 255);  //gray color for hydrogen
            ClickArrowRight.GetComponent<RectTransform>().anchoredPosition = new Vector2(-150, -670);
        }


        if (TutorialMessageNumber == 5)  //Drag out oxygen molecule
        {
            MoleculeCheckBox1.GetComponent<ShowMoleculeStructure>().HideMolecularStructure();  //remove clutter from the scene during this part of the tutorial
            MoleculeCheckBox2.GetComponent<ShowMoleculeStructure>().HideMolecularStructure();
            ContinueButton.interactable = false;
            PanelToShieldO2UI.SetActive(false);   //this will allow the user to drag out an oxygen molecule
            RedCircle.GetComponent<RectTransform>().anchoredPosition = new Vector2(100,-260);
            RedCircle.GetComponent<Image>().color = new Color32(208, 0, 0, 255);
            TutorialArrow.GetComponent<RectTransform>().anchoredPosition = new Vector2(145,-180);
            TutorialArrow.transform.eulerAngles = new Vector3(0, 0, 240);
            TutorialArrow.GetComponent<RectTransform>().localScale = new Vector2(.5f, 1f);
            TutorialArrow.GetComponent<Image>().color = new Color32(208, 0, 0, 255);  //red color for oxygen
            ClickArrowRight.GetComponent<RectTransform>().anchoredPosition = new Vector2(2000, -670);
            
            
            //GameObject.Find("Image1").SetActive(false);  //remove clutter from the scene during this part of the tutorial
            //GameObject.Find("Image2").SetActive(false);  //remove clutter from the scene during this part of the tutorial
        }

        if (TutorialMessageNumber == 6)  //To start the reaction, you need to break a bond.
        {
            PanelToShieldO2UI.SetActive(true);  //limit the user to one oxygen molecule!
            RedCircle.GetComponent<RectTransform>().anchoredPosition = new Vector2(2000, -314);
            TutorialArrow.GetComponent<RectTransform>().anchoredPosition = new Vector2(2000, -269);
            ContinueButton.interactable = true;
            //ClickArrowLeft.GetComponent<RectTransform>().anchoredPosition = new Vector2(-160, -150);
        }

        if (TutorialMessageNumber == 7)  //Drag the flame onto the O=O bond to separate the atoms!
        {
            FlameImage.GetComponent<UIDrop2World>().enabled = true;
            ContinueButton.interactable = false;
            ClickArrowLeft.GetComponent<RectTransform>().anchoredPosition = new Vector2(2000, 0);
            TutorialArrow.GetComponent<RectTransform>().anchoredPosition = new Vector2(180, -460);
            TutorialArrow.transform.eulerAngles = new Vector3(0, 0, 115);
            TutorialArrow.GetComponent<RectTransform>().localScale = new Vector2(1.8f, 1f);
            TutorialArrow.GetComponent<Image>().color = new Color32(255, 177, 0, 255);  //orange flame color

        }



        if (TutorialMessageNumber == 8)  //You need ENERGY to break a bond!
        {

            RedCircle.GetComponent<RectTransform>().anchoredPosition = new Vector2(2000, -314);
            TutorialArrow.GetComponent<RectTransform>().anchoredPosition = new Vector2(2000, -269);
            ContinueButton.interactable = true;
            //ClickArrowLeft.GetComponent<RectTransform>().anchoredPosition = new Vector2(-170, -160);  //points at "continue" button
        }

        if (TutorialMessageNumber == 9)  //the amount of energy needed to break an O=O double bond is listed in this red table!
        {
            FlameImage.GetComponent<UIDrop2World>().enabled = false;
            BlackCircle.GetComponent<RectTransform>().anchoredPosition = new Vector2(-590, -420);
            BlackCircle.GetComponent<RectTransform>().localScale = new Vector2(0.6f, 1f);
            ClickArrowLeft.GetComponent<RectTransform>().anchoredPosition = new Vector2(2000, -160);
            ContinueButton.interactable = true;
            
        }



        if (TutorialMessageNumber == 10)  //Get some Activation Energy (5  Joules of Ea)
        {
            ActivationEnergyButton.interactable = true;
            ContinueButton.interactable = false;
            //PanelToShieldFlameEaButtonAndJouleCorral.SetActive(false);
            BlackCircle.GetComponent<RectTransform>().anchoredPosition = new Vector2(2000, -420);
            ClickArrowLeft.GetComponent<RectTransform>().anchoredPosition = new Vector2(490, -567);
            //TutorialArrow.GetComponent<RectTransform>().anchoredPosition = new Vector2(135, -392);
            ClickArrowLeft.transform.eulerAngles = new Vector3(0, 0, 40);
            //TutorialArrow.GetComponent<RectTransform>().localScale = new Vector2(0.5f, 0.5f);            
            //ClickArrowLeft.GetComponent<RectTransform>().anchoredPosition = new Vector2(-50, -470);            

        }

        if (TutorialMessageNumber == 11)  //You now have 5-Joules of Heat, enough to break a bond!
        {
            ClickArrowLeft.GetComponent<RectTransform>().anchoredPosition = new Vector2(2000, -160);
            FlameImage.GetComponent<DragJoule>().enabled = false;
            ActivationEnergyButton.interactable = false;
            BlackCircle.GetComponent<RectTransform>().anchoredPosition = new Vector2(550, -630);
            BlackCircle.GetComponent<RectTransform>().localScale = new Vector2(1.2f, 1.1f);
            TutorialArrow.GetComponent<RectTransform>().anchoredPosition = new Vector2(2000, -269);            
            ContinueButton.interactable = true;
            //ClickArrowLeft.GetComponent<RectTransform>().anchoredPosition = new Vector2(-170, -160);
            //ClickArrowLeft.transform.eulerAngles = new Vector3(0, 0, 0);
        }

        if (TutorialMessageNumber == 12)  //Drag the flame onto the O=O bond to separate the atoms!
        {
            FlameImage.GetComponent<DragJoule>().enabled = true;
            FlameImage.GetComponent<UIDrop2World>().enabled = true;
            BlackCircle.GetComponent<RectTransform>().anchoredPosition = new Vector2(2000, -630);
            ContinueButton.interactable = false;            
            TutorialArrow.GetComponent<RectTransform>().anchoredPosition = new Vector2(180, -460);
            TutorialArrow.transform.eulerAngles = new Vector3(0, 0, 115);
            TutorialArrow.GetComponent<RectTransform>().localScale = new Vector2(1.8f, 1f);
            TutorialArrow.GetComponent<Image>().color = new Color32(255, 177, 0, 255);  //orange flame color
            ClickArrowLeft.GetComponent<RectTransform>().anchoredPosition = new Vector2(2000, 0);

        }

        if (TutorialMessageNumber == 13)  //Breaking a bond increases POTENTIAL ENERGY!
        {
            FlameImage.GetComponent<UIDrop2World>().enabled = false;  //no additional bond breaking events can occur at this time. . .
            ContinueButton.interactable = true;
            //ClickArrowLeft.GetComponent<RectTransform>().anchoredPosition = new Vector2(2000, 0);
            TutorialArrow.GetComponent<RectTransform>().anchoredPosition = new Vector2(2000, -506);
            //TutorialArrow.transform.eulerAngles = new Vector3(0, 0, 120);
            //TutorialArrow.GetComponent<RectTransform>().localScale = new Vector2(1.4f, 1f);
            //TutorialArrow.GetComponent<Image>().color = new Color32(255, 177, 0, 255);  //orange flame color

        }

        if(TutorialMessageNumber == 14)  //Blue Joules in the atoms represent PE 
        {           
            BlueJoulesInScene = GameObject.FindGameObjectsWithTag("PEJewel");
            print("BlueJoules = " + BlueJoulesInScene.Length);
            
            StartCoroutine("Blink");
        }

        if (TutorialMessageNumber == 15)  //Blue badge shows PE in a completed molecule
        {
            BlueJoulesInScene = GameObject.FindGameObjectsWithTag("MCToken");
            StartCoroutine("Blink");
            RedCircle.GetComponent<RectTransform>().anchoredPosition = new Vector2(-245, -344);  //Circle for the blue badge!
            RedCircle.GetComponent<RectTransform>().localScale = new Vector2(0.5f, 0.5f);
            RedCircle.GetComponent<Image>().color = new Color32(83, 139, 248, 255);  //nice blue color for blue badge
        }


        if (TutorialMessageNumber == 16)  //Break a C-H bond
        {
            RedCircle.GetComponent<RectTransform>().anchoredPosition = new Vector2(2000, -344);  //THIS NEEDS TO BE ADJUSTED
            FlameImage.GetComponent<UIDrop2World>().enabled = true;  //need to enable flame
            ContinueButton.interactable = false;
            ArrowForUnbondingFlameTarget.SetActive(true);
            

        }




        if (TutorialMessageNumber == 17)  //Examine the structure of the product H2O molecule 
        {
            FlameImage.GetComponent<UIDrop2World>().enabled = false;  //flame disabled
            ArrowForUnbondingFlameTarget.SetActive(false);
            ContinueButton.interactable = true;
            BlackCircle.GetComponent<RectTransform>().anchoredPosition = new Vector2(30, -570);
            BlackCircle.GetComponent<RectTransform>().localScale = new Vector2(0.8f, 0.8f);

            MoleculeCheckBox2.GetComponent<ShowMoleculeStructure>().ShowTheMolecularStructure();

        }


        if (TutorialMessageNumber == 18)  //Use a RIGHT click or LONG click to change the form of this oxygen atom 
        {
            BlackCircle.GetComponent<RectTransform>().anchoredPosition = new Vector2(2000, -630);
            ContinueButton.interactable = false;
            TutorialArrow.GetComponent<RectTransform>().anchoredPosition = new Vector2(-100, -190);
            TutorialArrow.transform.eulerAngles = new Vector3(0, 0, 340);
            TutorialArrow.GetComponent<RectTransform>().localScale = new Vector2(1.1f, 1f);
            TutorialArrow.GetComponent<Image>().color = new Color32(100, 66, 100, 255);  //dark purply color
            
        }



        if (TutorialMessageNumber == 19)  //Click on this hydrogen atom to rotate it
        {            
            //GameObject.Find("OxygenST(Clone)").transform.position = new Vector2(2f, 2f);  //move the oxygen atom to a known location for bonding with hydrogen
            TutorialArrow.GetComponent<RectTransform>().anchoredPosition = new Vector2(2000, -230);
            //ClickArrowLeft.GetComponent<RectTransform>().anchoredPosition = new Vector2(-100, -260);
            //ClickArrowLeft.transform.eulerAngles = new Vector3(0, 0, 0);
            ClickArrowRight.GetComponent<RectTransform>().anchoredPosition = new Vector2(-350, -260);
            ClickArrowRight.transform.eulerAngles = new Vector3(0, 0, 0);
            ContinueButton.interactable = false;
            //GameObject.Find("OxygenSTDB(Clone)").transform.position = new Vector2(5.6f, 0f);  //moves the unused oxygen atom to declutter the scene  (done in the SwapIt script!)
        }

        if (TutorialMessageNumber == 20)  //Make a BOND betweeen H and O!!!
        {
            DraggingDisabled = false;
            TutorialArrow.GetComponent<RectTransform>().anchoredPosition = new Vector2(-85, -290);
            TutorialArrow.GetComponent<Image>().color = new Color32(200, 200, 200, 255);  //gray color for hydrogen
            TutorialArrow.GetComponent<RectTransform>().localScale = new Vector2(1f, 1f);
            TutorialArrow.transform.eulerAngles = new Vector3(0, 0, -10);
            ClickArrowRight.GetComponent<RectTransform>().anchoredPosition = new Vector2(2000, -260);
            ClickArrowRight.transform.eulerAngles = new Vector3(0, 0, 0);
            ContinueButton.interactable = false;

        }

        if (TutorialMessageNumber == 21)  //When a bond forms, PE is converted to heat   (called from JewelMover script)
        {
            DraggingDisabled = true;
            TutorialArrow.GetComponent<RectTransform>().anchoredPosition = new Vector2(2000, -290);
            BlackCircle.GetComponent<RectTransform>().anchoredPosition = new Vector2(550, -630);
            BlackCircle.GetComponent<RectTransform>().localScale = new Vector2(1.2f, 1.1f);            
            ContinueButton.interactable = true;

        }



        if (TutorialMessageNumber == 22)  //Use the flame to break the bond between the remaining C-H bond 
        {
            FlameImage.GetComponent<UIDrop2World>().enabled = true;
            DraggingDisabled = true;
            ContinueButton.interactable = false;
            SecondArrowForUnbondingFlameTarget.SetActive(true);            
            BlackCircle.GetComponent<RectTransform>().anchoredPosition = new Vector2(2000, -630);           
            

        }

        if (TutorialMessageNumber == 23)  //Click TWICE to rotate the hydrogen atom so it can bond with the oxygen atom
        {
            
            ClickArrowRight.GetComponent<RectTransform>().anchoredPosition = new Vector2(-430, -360);
            //ClickArrowRight.transform.eulerAngles = new Vector3(0, 0, 0);
            ContinueButton.interactable = false;
            SecondArrowForUnbondingFlameTarget.SetActive(false);
            

        }

        if (TutorialMessageNumber == 24)  //Bond the hydrogen atom to oxygen to complete an H2O molecule!  SendTutorialMessage activated in AtomInventory Script CheckMoleculeCodes function
        {
            DraggingDisabled = false;
            ClickArrowRight.GetComponent<RectTransform>().anchoredPosition = new Vector2(2000, -60);
            TutorialArrow.GetComponent<RectTransform>().anchoredPosition = new Vector2(-130, -260);
            TutorialArrow.GetComponent<Image>().color = new Color32(200, 200, 200, 255);  //gray color for hydrogen
            TutorialArrow.GetComponent<RectTransform>().localScale = new Vector2(1.2f, 1f);
            TutorialArrow.transform.eulerAngles = new Vector3(0, 0, 10);
            //ClickArrowRight.transform.eulerAngles = new Vector3(0, 0, 0);
            ContinueButton.interactable = false;     


        }


        if (TutorialMessageNumber == 25)  //Checkbox Turns Green when you complete a product molecule
        {
            DraggingDisabled = true;
            FlameImage.GetComponent<UIDrop2World>().enabled = false;

            BlackCircle.GetComponent<RectTransform>().anchoredPosition = new Vector2(-10, -660);
            BlackCircle.GetComponent<RectTransform>().localScale = new Vector2(1.4f, 0.7f);
            TutorialArrow.GetComponent<RectTransform>().anchoredPosition = new Vector2(2000, -300);
            MoleculeCheckBox2.GetComponent<ShowMoleculeStructure>().HideMolecularStructure();
            ContinueButton.interactable = true;           

        }

        if (TutorialMessageNumber == 26)  //Examine structure of CO2 product molecule
        {
            BlackCircle.GetComponent<RectTransform>().anchoredPosition = new Vector2(-110, -560);
            BlackCircle.GetComponent<RectTransform>().localScale = new Vector2(1.1f, 1f);
            MoleculeCheckBox1.GetComponent<ShowMoleculeStructure>().ShowTheMolecularStructure();
            //ContinueButton.interactable = true;
        }


        if (TutorialMessageNumber == 27)  //Break the C=O double bond
        {
            FlameImage.GetComponent<UIDrop2World>().enabled = true;

            BlackCircle.GetComponent<RectTransform>().anchoredPosition = new Vector2(2000, -660);
            
            TutorialArrow.GetComponent<RectTransform>().anchoredPosition = new Vector2(70, -550);
            TutorialArrow.transform.eulerAngles = new Vector3(0, 0, 150);
            TutorialArrow.transform.localScale = new Vector2(1.5f, 1);
            TutorialArrow.GetComponent<Image>().color = new Color32(255, 177, 0, 255);  //orange flame color
            ContinueButton.interactable = false;
        }

        if (TutorialMessageNumber == 28)  //Use right click or long click to Change form of Carbon atom 
        {
            TutorialArrow.GetComponent<RectTransform>().anchoredPosition = new Vector2(-250, -240);
            TutorialArrow.transform.eulerAngles = new Vector3(0, 0, 290);
            TutorialArrow.transform.localScale = new Vector2(0.7f, 1);
            TutorialArrow.GetComponent<Image>().color = new Color32(5, 5, 5, 255);
            ContinueButton.interactable = false;
        }

        if (TutorialMessageNumber == 29)  //Bond both oxygen atoms to the carbon to make  CO2 molecule  --SendTutorialMessage triggered from AtomInventory script, CheckMoleculeCodes function
        {
            DraggingDisabled = false;
            TutorialArrow.GetComponent<RectTransform>().anchoredPosition = new Vector2(2000, -300);
            ContinueButton.interactable = false;
        }

        if (TutorialMessageNumber == 30)  //When all the check boxes are green, you have completed your mission! 
        {
            FlameImage.GetComponent<UIDrop2World>().enabled = false;
            BlackCircle.GetComponent<RectTransform>().anchoredPosition = new Vector2(-10, -660);
            BlackCircle.GetComponent<RectTransform>().localScale = new Vector2(1.4f, 0.7f);
            TutorialArrow.GetComponent<RectTransform>().anchoredPosition = new Vector2(2000, -300);
            MoleculeCheckBox1.GetComponent<ShowMoleculeStructure>().HideMolecularStructure();
            ContinueButton.interactable = true;

        }

        if (TutorialMessageNumber == 31)  //energy Accounting shows that you invested 11 Joules to Break Bonds
        {
            BlackCircle.GetComponent<RectTransform>().anchoredPosition = new Vector2(410, -366);
            BlackCircle.GetComponent<RectTransform>().localScale = new Vector2(3f, 0.5f);
            TutorialArrow.GetComponent<RectTransform>().anchoredPosition = new Vector2(2000, -300);
            ContinueButton.interactable = true;
        }

        if (TutorialMessageNumber == 32)  //energy Accounting shows that you received 14 Joules of Heat when forming bonds
        {
            //black circle stays put
            BlackCircle.GetComponent<RectTransform>().anchoredPosition = new Vector2(410, -416);
            ContinueButton.interactable = true;
        }

        if (TutorialMessageNumber == 33)  //This reaction is exothermic because it makes a heat "profit"
        {
            BlackCircle.GetComponent<RectTransform>().anchoredPosition = new Vector2(410, -466);
           
            
            ContinueButton.interactable = true;
        }

        if (TutorialMessageNumber == 34)  //In real life, exothermic reactions are used to cook food, run automobiles, etc.
        {
            BlackCircle.GetComponent<RectTransform>().anchoredPosition = new Vector2(2000, -466);

            
            ContinueButton.interactable = true;
        }

        if (TutorialMessageNumber == 35)  //Return to main menu
        {
            TutorialArrow.GetComponent<RectTransform>().anchoredPosition = new Vector2(-470, -100);
            TutorialArrow.transform.eulerAngles = new Vector3(0, 0, 150);
            TutorialArrow.transform.localScale = new Vector2(0.7f, 1);
            TutorialArrow.GetComponent<Image>().color = new Color32(174, 212, 0, 255);
            ContinueButton.interactable = false;
        }




        
        

    }

    private IEnumerator Blink()
    {
        print("blink");

        yield return new WaitForSeconds(1f);

        for(i = 1; i <4; i++)
        {
            foreach (GameObject BlueJoule in BlueJoulesInScene)
            {
                //print("Set" + BlueJoule + "to inactive");
                BlueJoule.SetActive(false);
            }

            yield return new WaitForSeconds(0.5f);

            foreach (GameObject BlueJoule in BlueJoulesInScene)
            {
                BlueJoule.SetActive(true);
            }

            yield return new WaitForSeconds(0.5f);
        }        
       
    }


}
