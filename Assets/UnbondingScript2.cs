﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnbondingScript2 : MonoBehaviour  //THIS SCRIPT IS ATTACHED TO THE UNBONDING JOULE GAMEOBJECT!
{
    private GameObject Atom1;      //Atom1 and Atom2 are the two atoms to unbond
    private GameObject Atom2;
    private int DotCount;       //This is used to make sure two dots are collided with--if only 1 dot, no bond to break!
    private GameObject Joule;    //the UI Jewel becomes a Colliding Joule to break bonds
    private Vector2 bondDirection;   //atoms unbond by moving Atom2 along the axis of the original bond
    private int MolIDValue;       //This variable gets the proper atom list from MoleculeList[].  Also used to push the atom list back in
    private GameObject SwapAtom;   //it is convenient to swap hydrogen to be Atom 2--this temporary variable allows for swapping Atom1 and Atom2
    private int JouleCost;     //the bond strength for the bond that is broken
    private int[,] bondArray;  //this array retrieves data from the master Bond Energy Array (attached to BondEnergyMatrix gameObject)
    RelativeJoint2D jointToBreak;   //this variable allows examination of each bond (joint) in the molecule (one at a time)
    List<RelativeJoint2D> jointsOnThisAtom;    //not using this--using the JointArray instead
    public RelativeJoint2D[] JointArray = new RelativeJoint2D[5];   //this stores the joints found on a given carbon atom (could be up to 4 joints)
    private int i; //used for indexing an array or list
    private int Index;  //used for assigning MoleculeID
    public int NewMoleculeID;  //used for a new carbon group that has been formed when C-C bond breaks
    private List<GameObject> TempAtomList;   //this list stores up the atoms that have broken away from the original molecule
    List<Rigidbody2D> BondingPartnerList;  //This is a Rigidbody list--when tracing bonds, need to use Rigidbody (joints connect RB's)
    private GameObject Diatomic;  //diatomics drawn from the drop down animation are instantiated as single game objects (not 2 atoms)
    private Vector3 DiatomicPosition;  //allows for proper placing of the individual atoms upon dissociation of diatomic icon
    private AudioSource SoundFX2;   //unbonding sound
    //private GameObject JouleToDestroy;    //??
    private GameObject[] JoulesInCorral;  //Joules of Heat in the Joule corral
    public static int DontBondAgain;  //used to make it impossible to create a new bond upon unbonding this atom
    public static int WaitABit;  //this variable counts down over 8 frames--when zero, bonding is re-enabled
    private int MolID1;   //used to check whether this "bond" isn't really a bond--could just be atoms juxtaposed (esp in cyclic molecules)
    private int MolID2;   //if this is a proper molecule, MolID1 = MolID2 because the bonded atoms are part of the same molecule list
    public GameObject[] GoldenJewelsToMove;   //These are icons of the jewels of heat that move from the corral to break a bond
    private Vector3 GoldenJewelStartingLocation;  //finds the center of joule corral
    private GameObject GoldenJewel;  //temporary label for the GoldenJewels that move
    //public Vector2 Atom2TargetPosition;
    //private Rigidbody2D AtomInMotionRB;




    // Start is called before the first frame update
    void Start()
    {
        DotCount = 0;                //resets DotCount
        Joule = gameObject;
        JointArray = new RelativeJoint2D[5];
        TempAtomList = new List<GameObject>();
        BondingPartnerList = new List<Rigidbody2D>();
        SoundFX2 = GameObject.Find("BondBrokenSound").GetComponent<AudioSource>();
        for (i = 0; i < 5; i++)
        {
            GoldenJewelsToMove[i] = GameObject.Find("BondEnergyMatrix").GetComponent<MovingJewelArrays>().MovingJewelIconsRed[i];
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {

        if (collider.tag == "UnbondingTrigger" || collider.tag == "UnbondingTriggerDB" || collider.tag == "Diatomic")

        {
            print("DotCount =" + DotCount);
            print("root object =" + collider.transform.root.gameObject);
            //DIATOMIC UNBONDING IS THE FIRST CASE
            if (collider.transform.root.gameObject.tag == "Diatomic")  //This section breaks a diatomic element into two atoms
            {
                print("dissociate diatomic!");
                Diatomic = collider.transform.root.gameObject;

                if (Diatomic.GetComponent<DiatomicScript>().BondDissociationEnergy > DisplayCanvasScript.JoulesOfHeat)
                {
                    GameObject.Find("ConversationDisplay").GetComponent<ConversationTextDisplayScript>().Denied();
                    //prints "You don't have enough joules to break this bond";
                    return;
                }
                else
                { //This starts the diatomic dissociation process.  Calls GoldenJewelMover script that is attached to the GoldenJewelsToMove prefab             
                    JouleCost = Diatomic.GetComponent<DiatomicScript>().BondDissociationEnergy;

                    DisplayCanvasScript.JoulesOfHeat -= JouleCost;  //BDE is a public defined in DiatomicScript
                    DisplayCanvasScript.HeatInvestedToBreakBonds += JouleCost;  

                    //DisplayCanvasScript.NetEnergyProfit -= 10;   //diatomic molecule = 10 bonus pts, need to subtract when molecule is destroyed
                    DiatomicPosition = Diatomic.transform.position;

                    JoulesInCorral = GameObject.FindGameObjectsWithTag("JouleInCorral");   //fill array with all the joules in the corral
                    
                    for (i = 0; i < JouleCost; i++)     //for as many joules as it takes to break the bond
                    {
                        Destroy(JoulesInCorral[i]);     //this "erases" red joules from the corral
                    }
                    GameObject.Find("JouleHolder").GetComponent<JouleHolderScript>().AdjustJoulesInCorral();

                    

                    GoldenJewelStartingLocation = GameObject.Find("JouleHolder").transform.position;  //Golden Jewels fly from center of corral 
                    GoldenJewelStartingLocation = new Vector3(GoldenJewelStartingLocation.x, GoldenJewelStartingLocation.y, 0);
                    GoldenJewel = Instantiate(GoldenJewelsToMove[JouleCost]) as GameObject;  //Either 1 Jewel, 2 Jewels, 3 Jewels or 4 Jewels 
                    GoldenJewel.transform.position = GoldenJewelStartingLocation;
                    GameObject.FindWithTag("MovingGoldenJewel").GetComponent<GoldenJewelMover>().DiatomicDissociation(Diatomic, JouleCost);
                    //The line above starts the Golden Jewels Moving to dissociate the diatomic--unbonding event handled in GoldenJewelMover script

                }

            }

            if (DotCount == 0)     //this is the first trigger
            {
                DotCount = 1;
                Atom1 = collider.transform.root.gameObject;
                print("Atom1 =" + Atom1);
            }

            else if (DotCount == 1)  //indicates we have a second trigger (two unbonding triggers)
            {
                Atom2 = collider.transform.root.gameObject;
                print("Atom2 = " + Atom2);

                //if (GameObject.Find("TutorialMarker").GetComponent<TutorialScript>().Tutorial == true && DieScript.totalRolls == 3)
                //{
                //    print("abort this bond breaking event in the tutorial version");
                //    return;  //disable dissociation of C=O bond on turn 3 of tutorial--only H-H dissociation is allowed
                //}

                //else if (GameObject.Find("TutorialMarker").GetComponent<TutorialScript>().Tutorial == true && DieScript.totalRolls == 4)
                //{
                //    if (Atom1.tag == "Oxygen" || Atom2.tag == "Oxygen")
                //    {
                //        print("abort this bond breaking event in the tutorial version");
                //        return;  //disable dissociation of C=O bond on turn 3 of tutorial--only C-H dissociation is allowed
                //    }

                //}



                MolID1 = Atom1.GetComponent<BondMaker>().MoleculeID;
                MolID2 = Atom2.GetComponent<BondMaker>().MoleculeID;

                if (MolID1 != MolID2)   //this means that two atoms that are of different molecules are juxtaposed so that they look like they are bonded
                {
                    GameObject.Find("ConversationDisplay").GetComponent<ConversationTextDisplayScript>().NoBondToBreak();
                    return;
                }

                if (MolID1 == 0 || MolID2 == 0)  //means one of these atoms is not bonded to anything
                {
                    GameObject.Find("ConversationDisplay").GetComponent<ConversationTextDisplayScript>().NoBondToBreak();
                    return;
                }

                if (Atom1.GetComponent<BondMaker>().Monovalent)  //If there is a Monovalent atom, put it in Atom2 slot--simplifies later case work 
                {
                    SwapAtom = Atom2;
                    Atom2 = Atom1;
                    Atom1 = SwapAtom;
                    print("Atom1 and Atom2 swapped");
                }


                if (Atom1.GetComponent<BondMaker>().Monovalent)  //If there is a Monovalent atom, put it in Atom2 slot--simplifies later case work 
                {
                    SwapAtom = Atom2;
                    Atom2 = Atom1;
                    Atom1 = SwapAtom;
                    print("Atom1 and Atom2 swapped");
                }

                if (Atom1 != Atom2)
                {
                    print(Atom1 + "and " + Atom2 + " unbond");

                    //THIS IS THE CALCULATE JOULE COST FUNCTION--ALL BOND BREAKING CASES 2, 3 & 4 USE THIS!
                    bondArray = GameObject.Find("BondEnergyMatrix").GetComponent<BondEnergyValues>().bondEnergyArray;
                    if (collider.tag == "UnbondingTriggerDB")  //if double bond, need to use BondArrayID 4 (for double bonded carbon) or 5 (for double bonded oxygen)
                    {
                        JouleCost = bondArray[Atom1.GetComponent<BondMaker>().bondArrayID + 3, Atom2.GetComponent<BondMaker>().bondArrayID + 3];
                    }
                    else
                    {
                        JouleCost = bondArray[Atom1.GetComponent<BondMaker>().bondArrayID, Atom2.GetComponent<BondMaker>().bondArrayID];
                    }


                    print("JouleCost =" + JouleCost);  //JouleCost comes from the bondArray[], which is a copy of the Master Array attached to BondEnergyMatrix GameObject
                    if (JouleCost > DisplayCanvasScript.JoulesOfHeat)
                    {
                        GameObject.Find("ConversationDisplay").GetComponent<ConversationTextDisplayScript>().Denied(); //prints "You don't have enough joules to break this bond";
                        return;
                    }


                    if (GameObject.Find("BondEnergyMatrixAdvanced"))
                    {
                        
                        int[,] AdvBondEnergyMatrix = GameObject.Find("BondEnergyMatrixAdvanced").GetComponent<kJperMoleBondEnergyScript>().AdvancedBondEnergyMatrix;
                        int AdvancedBondEnergy;
                        if (collider.tag == "UnbondingTriggerDB")  //if double bond, need to use BondArrayID 4 (for double bonded carbon) or 5 (for double bonded oxygen)
                        {
                            AdvancedBondEnergy = AdvBondEnergyMatrix[Atom1.GetComponent<BondMaker>().bondArrayID + 3, Atom2.GetComponent<BondMaker>().bondArrayID + 3];
                        }
                        else
                        {
                            AdvancedBondEnergy = AdvBondEnergyMatrix[Atom1.GetComponent<BondMaker>().bondArrayID, Atom2.GetComponent<BondMaker>().bondArrayID];  //set Atom2 to collider.transform.root.gameObject
                        }

                        GameObject.Find("ConversationDisplay").GetComponent<ConversationTextDisplayScript>().HeatToPEConversion(AdvancedBondEnergy);
                        DisplayCanvasScript.HeatInvestedToBreakBonds += AdvancedBondEnergy;
                    }

                    else
                    {
                        DisplayCanvasScript.HeatInvestedToBreakBonds += JouleCost;
                    }

                    DisplayCanvasScript.JoulesOfHeat -= JouleCost;
                    GameObject.Find("JouleHolder").GetComponent<JouleHolderScript>().AdjustJoulesInCorral();

                    //THE GoldenJewelMover Script Sends Golden Jewels to the unbonding location and adjusts PE values for each unbonded atom
                    GoldenJewelStartingLocation = GameObject.Find("JouleHolder").transform.position;
                    //print(GoldenJewelStartingLocation);
                    GoldenJewelStartingLocation = new Vector3(GoldenJewelStartingLocation.x, GoldenJewelStartingLocation.y, 0);
                    GoldenJewel = Instantiate(GoldenJewelsToMove[JouleCost]) as GameObject;  //Either 1 Jewel, 2 Jewels, 3 Jewels or 4 Jewels 
                    GoldenJewel.transform.position = GoldenJewelStartingLocation;  //starting point for the flying joules is center of Joule Corral
                    GameObject.FindWithTag("MovingGoldenJewel").GetComponent<GoldenJewelMover>().MovingJewel(JouleCost, Atom1, Atom2);


                    JoulesInCorral = GameObject.FindGameObjectsWithTag("JouleInCorral");   //fill array with all the joules in the corral
                    for (i = 0; i < JouleCost; i++)     //for as many joules as it takes to break the bond
                    {
                        Destroy(JoulesInCorral[i]);
                    }
                    //THIS ENDS THE CALCULATE JOULE COST FUNCTION


                    MolIDValue = Atom1.GetComponent<BondMaker>().MoleculeID;   //MolIDValue is used in this script to store MoleculeID

                    MCTokenSearch();  //THIS IS A FUNCTION:  If a MoleculeCompletionToken is in this list, remove it and decrement pt value

                    //CASE2
                    if (AtomInventory.MoleculeList[MolIDValue].Count == 2 || AtomInventory.MoleculeList[MolIDValue].Count == 0)  //CASE 2--ONLY TWO ATOMS TO UNBOND
                    {
                        ClearMoleculeList();  //THIS FUNCTION SETS EACH ATOM TO UNBONDED-0 STATE AND REMOVES JOINT THAT BONDS THEM
                        MoveAtomsAndAdjustValleys();
                        print("CASE2 complete--gonna return now");
                        return;   //this seems to be working!!!  (April 26 11:45 am)
                    }


                    //CASE3
                    else if (Atom2.GetComponent<BondMaker>().Monovalent)   //CASE3--SPLITTING A MONOVALENT FROM A GROUP OF AT LEAST 2 ATOMS
                    {
                        print("CASE3 initiated--dissociate monovalent atom");
                        Destroy(Atom2.GetComponent<RelativeJoint2D>());  //every monovalent has a fixed joint
                        Atom2.GetComponent<BondMaker>().MoleculeID = 0;    //Reset monovalent Atom2 MoleculeID to zero (unbonded state)
                        Atom2.GetComponent<BondMaker>().bonded = false;   //reset monovalent Atom2 to unbonded state
                        AtomInventory.MoleculeList[MolIDValue].Remove(Atom2);  //take the monovalent out of the MoleculeList
                        MoveAtomsAndAdjustValleys();   //FOR CASE3
                        print("CASE3 completed");
                        return;
                    }


                    //CASE4 No monovalent involved in this unbonding:  At least one cluster of atoms is involved bc CASE2 was not met
                    else

                    {
                        print("CASE4:  carbon-carbon bond is now broken");
                        print(AtomInventory.MoleculeList[MolIDValue].Count + " AtomInvCount");


                        if (AtomInventory.MoleculeList[MolIDValue].Count > 2)  //this means that at least one of the Atoms has bonding partners--do contact tracing!
                        {
                            Rigidbody2D Atom1rb = Atom1.GetComponent<Rigidbody2D>();  //need Rigidbody for the joint scripting!  GameObject name doesn't work!!
                            Rigidbody2D Atom2rb = Atom2.GetComponent<Rigidbody2D>();   //Atom2rb is the Rigidbody equivalent of Atom2


                            if (Atom1.GetComponent<RelativeJoint2D>())   //trying to find the joint that links the two carbons we are unbonding
                            {
                                JointArray = Atom1.GetComponents<RelativeJoint2D>();   //this gets ALL the bonds between this carbon and neighboring carbons/oxygens
                                print("Atom 1 joints:");

                                foreach (RelativeJoint2D jointToBreak in JointArray)  //Search through all the joints on Atom1 to find the one to break!
                                {
                                    //print(jointToBreak.connectedBody);  //just for debugging purposes

                                    if (jointToBreak.connectedBody == Atom2rb)   //this is the connector between the two carbon atoms
                                    {
                                        jointToBreak.connectedBody = null;   //need to do this to avoid Atom1 joining BondingPartnerList--it remembered its ConnectedBody
                                        Destroy(jointToBreak);
                                        print("joint from Atom1 to Atom2 broken");
                                    }
                                }
                            }

                            if (Atom2.GetComponent<RelativeJoint2D>() == null)  //PROBABLY DON'T NEED THIS
                            {
                                BondingPartnerList.Add(Atom2.GetComponent<Rigidbody2D>());  //start the list with Atom2
                            }

                            else        //if Atom2 has bonds, add ALL the attached atoms to BondingPartnerList
                            {
                                BondingPartnerList.Add(Atom2.GetComponent<Rigidbody2D>());  //start the list with Atom2
                                JointArray = Atom2.GetComponents<RelativeJoint2D>();  //searching Atom2 for all BondingPartners

                                foreach (RelativeJoint2D jointToBreak in JointArray)
                                {
                                    if (jointToBreak.connectedBody == Atom1rb)   //Need to destroy the joint between Atom2 and Atom1
                                    {
                                        Destroy(jointToBreak);
                                        print("joint from Atom2 to Atom1 broken");
                                    }

                                    BondingPartnerList.Add(jointToBreak.connectedBody);  //BondingPartnerList collects the rb's at the ends of joints from Atom2 
                                    BondingPartnerList.Remove(Atom1rb);   //this is iterated--not a problem?
                                }
                            }

                            //STILL CASE4--ATOM2 WILL BE MOVED WITH ALL ITS CONNECTED ATOMS 
                            foreach (Rigidbody2D atomRB in BondingPartnerList)   //this list is currently only carbons and oxygens--hydrogens added later
                            {
                                print("Atom2 BondingPartnerList contains " + atomRB);  //just a check to see if the list is complete
                            }

                            for (i = 1; i < 5; i++)  //iterate the search so that distant contacts will be included in the BondingPartnerList
                            {
                                foreach (GameObject atom in AtomInventory.MoleculeList[MolIDValue])  //This is to move the hydrogens attached to carbons
                                {
                                    //THIS SCRIPT NOW LOOKS AT EVERY JOINT ON A POLYVALENT ATOM!!!!
                                    JointArray = atom.GetComponents<RelativeJoint2D>();  //this will get all the joints on "atom"

                                    foreach (RelativeJoint2D joint in JointArray)  //look at the joints one at a time
                                    {
                                        if (BondingPartnerList.Contains(joint.connectedBody))  //Looks for atoms in MoleculeList[] who have joints that target atoms in BondingPartnerList
                                        {
                                            if (BondingPartnerList.Contains(atom.GetComponent<Rigidbody2D>()))
                                            {
                                                print(atom + " is NOT added to BPL bc it's already present");
                                            }
                                            else
                                            {
                                                print(atom + " was added to BPL");   //this will display the subset of atoms that will move with Atom2
                                                BondingPartnerList.Add(atom.GetComponent<Rigidbody2D>());  //this should complete the BondingPartnerList--all atoms to dissociate from the main molecule
                                                BondingPartnerList.Remove(Atom1rb);
                                            }

                                        }

                                    }
                                }
                            }


                            //need an empty MoleculeID to store the dissociated Atom2 cluster  THIS IS A FUNCTION used only when unbonding Carbons/Oxygens
                            for (i = 1; i < 13; i++)   //Slots 1-12 in the array are used to store Molecules (atoms in the molecule)
                            {
                                if (AtomInventory.MoleculeList[i] == null || AtomInventory.MoleculeList[i].Count == 0)
                                {
                                    Index = i;      //Index finds the lowest empty MoleculeList slot
                                    break;          //to abort the loop after the first empty slot is found
                                }
                            }


                            NewMoleculeID = Index;                         //Index shows the empty slot to use for the new MoleculeID 
                            AtomInventory.MoleculeList[MolIDValue].Remove(Atom2);  //take Atom2 out of the original MoleculeID--Atom2 always moves


                            foreach (Rigidbody2D BP in BondingPartnerList)  //BondingPartnerList is a list of all the atoms to move
                            {
                                if (BondingPartnerList.Count == 1)   //if only a single atom being dissociated, set to UNBONDED state
                                {
                                    print("Atom2 reset to Zero");
                                    Atom2.GetComponent<BondMaker>().bonded = false;  //BP.GetComponent<BondMaker>().bonded = false;    //set to unbonded--allows rotation and SwapIt
                                    Atom2.GetComponent<BondMaker>().MoleculeID = 0; //BP.GetComponent<BondMaker>().MoleculeID = 0;    //set molecule ID to zero
                                    //break;
                                }
                                else  //TempAtomList is the list of all atoms to be moved as a cluster.  Each "BP" is an atom in BondingPartnerList
                                {
                                    TempAtomList.Add(BP.gameObject);     //translates RigidbodyList to GameObjectList
                                    AtomInventory.MoleculeList[MolIDValue].Remove(BP.gameObject);   //removes BondingPartners of Atom2 from original MoleculeID
                                    BP.gameObject.GetComponent<BondMaker>().MoleculeID = NewMoleculeID;  //redesignates atom with correct ID}
                                    AtomInventory.MoleculeList[NewMoleculeID] = TempAtomList;  //stores the atoms that have been moved in a new MoleculeID slot
                                }

                            }

                            if (AtomInventory.MoleculeList[MolIDValue].Count == 1)  //check to see if Atom1 is all alone after moving out the Atom2 cluster, if so, MoleculeID = zero
                            {
                                print("Atom1 gets reset to MolID zero");  //originally didn't work for O2 bc MCToken counted as a game object!!! 
                                Atom1.GetComponent<BondMaker>().bonded = false;
                                Atom1.GetComponent<BondMaker>().MoleculeID = 0;
                                AtomInventory.MoleculeList[MolIDValue].Clear();
                            }
                        }

                        MoveAtomsAndAdjustValleys();  //THIS IS FOR CASE4  

                    }

                }
            }
        }

    }

    private void MCTokenSearch()  //If a MoleculeCompletionToken is in this list, remove it and decrement pt value
    {
        foreach (GameObject MCToken in AtomInventory.MoleculeList[MolIDValue])
        {
            //print("searching for MCToken");
            //print(MCToken);

            foreach (Transform child in MCToken.transform)
            {
                if (child.tag == "MCToken")  //searching for an MCToken as child.  Only exists if this was a completed molecule
                {
                    Destroy(child.gameObject);  //this is the MCToken (20, 30, 40, 60 pts)
                    print("child MCToken destroyed");
                    
                }
            }

            if (MCToken.tag == "MCToken")         //Unbonding removes the COMPLETED MOLECULE TOKEN from the MoleculeList 
            {
                print("MC Token Found and Removed");
                AtomInventory.MoleculeList[MolIDValue].Remove(MCToken);    //MC Token removed from list
                DisplayCanvasScript.NetEnergyProfit -= MCToken.GetComponent<MoleculePtValues>().BonusPtValue;  //Value of MCToken subtracted from score
                break;
            }
        }
    }

    private void ClearMoleculeList()  //this is called when only two atoms are in the molecule when unbonding occurs
    {
        //print("hello world");
        AtomInventory.MoleculeList[MolIDValue].Clear();   //Empty the molecule list b/c both atoms become unbonded
        print("cleared MoleculeList");
        Atom1.GetComponent<BondMaker>().MoleculeID = 0;  // Set MoleculeID of Atom1 to zero
        print("Atom1 set to zero");
        Atom2.GetComponent<BondMaker>().MoleculeID = 0;  //Set MoleculeID of atom2 to zero
        print("Atom2 set to zero");
        Atom1.GetComponent<BondMaker>().bonded = false;  //Set bonded state to false for both atoms
        Atom2.GetComponent<BondMaker>().bonded = false;
        if (Atom1.GetComponent<RelativeJoint2D>())    //remove the bond!
        {
            Destroy(Atom1.GetComponent<RelativeJoint2D>());
        }
        if (Atom2.GetComponent<RelativeJoint2D>())
        {
            Destroy(Atom2.GetComponent<RelativeJoint2D>());
        }
    }

    private void MoveAtomsAndAdjustValleys()
    {/*  //THIS FUNCTION IS NOW ACHIEVED IN THE GoldenJewelMover script
        //the line of code below moves the unbonded atoms apart by a reasonable distance
        bondDirection = (Atom2.transform.position - Atom1.transform.position); //finds the vector that lines up the two atoms
        Atom2.transform.position = new Vector2(Atom2.transform.position.x + 0.23f * bondDirection.x, Atom2.transform.position.y + 0.23f * bondDirection.y);
        Atom1.GetComponent<BondMaker>().valleysRemaining++;   //an empty bonding slot has appeared on Atom1
        Atom2.GetComponent<BondMaker>().valleysRemaining++;    //an empty bonding slot has appeared on Atom2
        SoundFX2.Play();   //Plays Unbonding Sound
        DontBondAgain = 20;
        print("DontBondAgain set to 20");*/

        GameObject.Find("MoleculeListKeeper").GetComponent<AtomInventory>().GetMoleculeCode();  //check to see if this unbonding event has destroyed a requested product molecule 
        //NEED TO DO THE MOLECULECODECHECK AFTER THE UNBONDING HAS SORTED THE COMPONENT ATOMS. . .


    }


    // Update is called once per frame
    void Update()
    {
        WaitABit--;

        if (WaitABit < 1)
        {
            Destroy(Joule);
            Atom1 = null;
            Atom2 = null;
            DotCount = 0;
        }

    }

}
