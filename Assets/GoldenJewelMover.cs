using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldenJewelMover : MonoBehaviour  //This script is attached to the Prefabs GoldenJewelsToMove (there are 4 versions of GoldenJewelToMove)
{

    
    private int i;
    private Vector3 targetForGoldenJewels;
    
    private float JouleCostPerAtom;
    private Vector2 bondDirection;
    private AudioSource SoundFX2;

    public GameObject FlameIcon;

    Vector2 Atom2TargetPosition;
    private Rigidbody2D AtomInMotionRB;



    // Start is called before the first frame update
    void Start()   
    {
        SoundFX2 = GameObject.Find("BondBrokenSound").GetComponent<AudioSource>();
        

    }

    public void MovingJewel(int JouleCost, GameObject Atom1, GameObject Atom2)
    {
        //print("time to move Golden Jewels!");
        //print("Atom1 = " + Atom1);
        //print("Atom2 = " + Atom2);
        //if (DisplayCanvasScript.HeatInvestedToBreakBonds <= GameObject.Find("Display Canvas").GetComponent<DisplayCanvasScript>().InitialHeatJoules)  //check to see if ??
        //{
        //    DisplayCanvasScript.HeatInvestedToBreakBonds += JouleCost;   //this variable is only used in the PotentialEnergyExample scene, but may be useful in future scenes. . .
        //}
        
        DisplayCanvasScript.PotentialEnergyInScene += JouleCost;  //this variable is only used in the PotentialEnergyExample scene, but may be useful in future scenes. . .
        GameObject.Find("FlameController").GetComponent<FlameHidingScript>().FlameOff();  //only one flame at a time
        JouleCostPerAtom = JouleCost / 2f;  //this is needed to add PE joules to unbonding atoms
        targetForGoldenJewels = GameObject.FindWithTag("UnbondingJoule").transform.position;  //destination for moving GoldenJewel 
        //print(targetForGoldenJewels);
        StartCoroutine(MoveJewel(JouleCost, Atom1, Atom2));

    }

    public void DiatomicDissociation(GameObject Diatomic, int JouleCost)
    {
        print("Golden jewels moving to dissociate diatomic molecule");
        //if(DisplayCanvasScript.HeatInvested <= GameObject.Find("Display Canvas").GetComponent<DisplayCanvasScript>().InitialHeatJoules)
        //{
        //    DisplayCanvasScript.HeatInvested += JouleCost;   //this variable is only used in the PotentialEnergyExample scene, but may be useful in future scenes. . .
        //}
        
        DisplayCanvasScript.PotentialEnergyInScene += JouleCost;  //this variable is only used in the PotentialEnergyExample scene, but may be useful in future scenes. . .
        GameObject.Find("FlameController").GetComponent<FlameHidingScript>().FlameOff();
        targetForGoldenJewels = GameObject.FindWithTag("UnbondingJoule").transform.position;  //destination for moving GoldenJewel 
        //print("Target location = " + targetForGoldenJewels);
        StartCoroutine(MoveJewelsToDiatomic(Diatomic, JouleCost)); 
    }

    // Update is called once per frame
    void Update()
    {  

    }

    //MoveJewel IEnumerator is used for dissociation of a "normal" bond.  This function is called from ??
    private IEnumerator MoveJewel(int JouleCost, GameObject Atom1, GameObject Atom2) 
    {        
        while (transform.position != targetForGoldenJewels)     //moves GoldenJewels to site of unbonding event        
        {
            JewelMover.JewelsInMotion = true;
            transform.position = Vector3.MoveTowards(transform.position, targetForGoldenJewels, 24*Time.deltaTime);   //third value is the speed at which Jewel moves
            yield return new WaitForEndOfFrame();  
        }

        print("The Golden Jewel has arrived");
        GameObject.Find("FlameController").GetComponent<FlameHidingScript>().FlameOn();  //activates unbonding flame icon
        
        

        JewelMover.JewelsInMotion = false;
        //THE UNBONDING OF ATOMS IS DELAYED UNTIL THE GOLDEN JEWELS ARRIVE!!
            print(Atom1 +" PE increased by" + JouleCostPerAtom);
            Atom1.GetComponent<PotentialEnergy>().PE += JouleCostPerAtom;    //adds potential energy to unbonded atoms!
            Atom2.GetComponent<PotentialEnergy>().PE += JouleCostPerAtom;
            Atom1.GetComponent<PotentialEnergy>().PotentialEnergyAdjust();
            Atom2.GetComponent<PotentialEnergy>().PotentialEnergyAdjust();

        //private void MoveAtomsAndAdjustValleys()
        //THIS FUNCTION IS NOW ACHIEVED IN THE GoldenJewelMover script
        //the line of code below moves the unbonded atoms apart by a reasonable distance, NOW USING RIGIDBODY.MOVEPOSITION!  Rigidbody.MovePosition command is in the UnbondingAtomMoverScript attached to FlameUIElement

        bondDirection = (Atom2.transform.position - Atom1.transform.position); //finds the vector that lines up the two atoms
        Atom2TargetPosition = new Vector2(Atom2.transform.position.x + 0.23f * bondDirection.x, Atom2.transform.position.y + 0.23f * bondDirection.y);  //Allows use of Rigidbody.move command
        Atom2.GetComponent<Rigidbody2D>().MovePosition(Atom2TargetPosition); //transform.position = new Vector2(Atom2.transform.position.x + 0.23f * bondDirection.x, Atom2.transform.position.y + 0.23f * bondDirection.y);
        AtomInMotionRB = Atom2.GetComponent<Rigidbody2D>();
        GameObject.Find("FlameUIElement").GetComponent<UnbondingAtomMoverScript>().AtomInMotionRB = AtomInMotionRB;  //these three lines allow for Rigidbody.MovePosition to work over 12 frames, resulting in a smooth animation of unbonding with groups of atoms!
        GameObject.Find("FlameUIElement").GetComponent<UnbondingAtomMoverScript>().Atom2TargetPosition = Atom2TargetPosition; //the UnbondingAtomMoverScript attached to FlameUIElement uses the Rigidbody.MovePosition command
        GameObject.Find("FlameUIElement").GetComponent<UnbondingAtomMoverScript>().AtomInMotionCountdownTimer = 12;

        Atom1.GetComponent<BondMaker>().valleysRemaining++;   //an empty bonding slot has appeared on Atom1
        Atom2.GetComponent<BondMaker>().valleysRemaining++;    //an empty bonding slot has appeared on Atom2
        SoundFX2.Play();   //NEED A TRANSFORMATION OF JOULES SOUND--KE HAS BECOME PE!!!  MAGIC!!!

        if (GameObject.Find("BondEnergyMatrixAdvanced"))
        {

        }
        else
        {
            GameObject.Find("ConversationDisplay").GetComponent<ConversationTextDisplayScript>().HeatToPEConversion(JouleCost);
        }

        
        //GameObject.Find("JouleHolder").GetComponent<JouleHolderScript>().AdjustJoulesInCorral();

        //GameObject.Find("MoleculeListKeeper").GetComponent<AtomInventory>().GetMoleculeCode();  //check to see if this unbonding event has destroyed a requested product molecule COULD IMPROVE THIS TO BE CALLED ONLY WHEN A COMPLETE MOLECULE HAS BEEN DISRUPTED

        UnbondingScript2.DontBondAgain = 20;  //will delay bonding initiation events until the unbonding is complete.  
        print("DontBondAgain set to 20");

       
        Destroy(gameObject);

        if (TutorialSpeechBubbleScript.TutorialMessageNumber == 16  || TutorialSpeechBubbleScript.TutorialMessageNumber == 22  || TutorialSpeechBubbleScript.TutorialMessageNumber == 27)  //these messages ask user to break the C-H bond
        {
            GameObject.Find("TutorialSpeechBubble").GetComponent<TutorialSpeechBubbleScript>().SendTutorialMessage();
        }

        yield return null;


    }

    //MoveJewelsToDiatomic accomplishes dissociation of original diatomic molecules
    private IEnumerator MoveJewelsToDiatomic(GameObject Diatomic, int JouleCost)
    {        
        while (transform.position != targetForGoldenJewels) 
        {
            transform.position = Vector3.MoveTowards(transform.position, targetForGoldenJewels, 24 * Time.deltaTime);   //third value is the speed at which Jewel moves
            yield return new WaitForEndOfFrame();
        }


        print("The Golden Jewel has arrived");
        GameObject.Find("FlameController").GetComponent<FlameHidingScript>().FlameOn();

        Instantiate(Diatomic.GetComponent<DiatomicScript>().DissociationProduct, new Vector3(Diatomic.transform.position.x - 1.0f, Diatomic.transform.position.y, 0f), Quaternion.identity);  //Dissociation Produce is a public GameObject defined in DiatomicScript (H, Cl or O)
        Instantiate(Diatomic.GetComponent<DiatomicScript>().DissociationProduct, new Vector3(Diatomic.transform.position.x + 1.7f, Diatomic.transform.position.y -1.5f, 0f), Quaternion.Euler(0f, 0f, 180f));
        Destroy(Diatomic);  //the line above spawns the second atom rotated 180  degrees from the first
        SoundFX2.Play();   //Bond breaking soundDestroy(Joule);
                           //print("Case 1 (diatomic) complete");
                           //GameObject.Find("JouleHolder").GetComponent<JouleHolderScript>().AdjustJoulesInCorral();
        
        if (TutorialSpeechBubbleScript.TutorialMessageNumber == 10 || TutorialSpeechBubbleScript.TutorialMessageNumber == 15)
        {
            print("advance tutorial");
            GameObject.Find("TutorialSpeechBubble").GetComponent<TutorialSpeechBubbleScript>().SendTutorialMessage();
        }
        
        else if (GameObject.Find("BondEnergyMatrixAdvanced"))  //if AdvancedBondMatrix in effect, the command to print is sent from UnbondingScript2
        {

        }

        else
        {
            GameObject.Find("ConversationDisplay").GetComponent<ConversationTextDisplayScript>().HeatToPEConversion(JouleCost);
        }

      
        Destroy(gameObject);  //the Unbonding Jewel is now destroyed


        yield return null;


    }

   

}
 