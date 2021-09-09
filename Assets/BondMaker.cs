using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BondMaker : MonoBehaviour
{
    //This script makes a bond (fixed joint) when triggered by simultaneous collision of "peaks" and "valleys" Double Handshake
    RelativeJoint2D joint;
    RelativeJoint2D joint1;
    RelativeJoint2D joint2;

    public bool bonded;     //when bonded, atom no longer rotates
    public int colliderCount;  //the trigger value--need double handshake for a bond to form
    private int otherColliderCount;  //trigger on the other collider--completes double handshake
    public int valleysRemaining;  //number of bonding slots to fill:  H = 1, O = 2, N = 3, C = 4, etc.
    private int totalValleysRem;  //calculated each bonding event--when reaches zero, molecule is complete! All bonding slots filled
    public int bondArrayID;  //H = 0, C = 1, O = 2, Cl = 3, C with double bond = 4, O with double bond = 5 
    private int[,] bondArray;  //temporary storage of the bondEnergyArray stored in game object BondEnergyMatrix
    private int BondEnergy;  //value in joules of the bond that has just been formed
    private GameObject BondingPartner;  //the atom to which this atom has bonded
    private int i;  //counting integer in "for" loop
    public static int Index = 1;  //temporary variable used to assign Molecule ID values
    public int MoleculeID;
    private int BondingPartnerMoleculeID;
    private List<GameObject>TempAtomList;
    private List<GameObject> NewMoleculeList;
    private int BonusPts;           //Bonus Pt value for completed molecule
    private GameObject MCToken;
    public AudioSource SoundFX;     //Bond Formed Sound
    private AudioSource SoundFX3;   //Molecule Completion sound
    public bool Monovalent;
    public GameObject BadgeRecipient;
    public static bool MoleculeJustCompleted;
    public GameObject[] BadgeToPin = new GameObject[7];
    private float BadgeRotation;
    public GameObject CyclicToken;
    private GameObject MovingJewel;  //PE jewels move towards Joule Corral
    public GameObject[] MovingJewels = new GameObject[5];  //Images of One to Four Jewels fly, depending on Bond Energy.  Images are stored in the BondEnergyMatrix GameObject MovingJewelArrays script
    public static float TimeSinceLastBond;
    public static float TimeLastBondFormed;
    public float DuplicateBondPreventionTimer;
    private GameObject BondEnergyMatrix;
    private Vector2 VectorOffset;
    public float TotalPEInThisMolecule;


    // Start is called before the first frame update
    void Start()
    {
        //bonded = false;
        TempAtomList = new List<GameObject>();
        NewMoleculeList = new List<GameObject>();
        SoundFX = GameObject.Find("BondMadeSound").GetComponent<AudioSource>();
        SoundFX3 = GameObject.Find("MoleculeCompleteSound").GetComponent<AudioSource>();
        BondEnergyMatrix = GameObject.Find("BondEnergyMatrix");

        for (i=0; i<5; i++)
        {
            MovingJewels[i] = GameObject.Find("BondEnergyMatrix").GetComponent<MovingJewelArrays>().MovingJewelIconsBlue[i];

        }
        

    }

    private void OnCollisionEnter2D(Collision2D collision)  //Double Bonds are formed via COLLISION2D.  Single Bonds are formed via Trigger2D
    {

        
        //print(UnbondingScript2.DontBondAgain);
        //print("this collider = " + collision.collider);
        //print("other collider = " + collision.otherCollider);

        if (collision.collider.tag == "BondingTriggerDB" && collision.otherCollider.tag == "BondingTriggerDB")
        {
            BondingPartner = collision.collider.transform.root.gameObject;  //add bonding partner to array of molecule's atoms
            VectorOffset = collision.collider.GetComponent<VectorOffset>().BondingOffsetVector;

            if(BondEnergyMatrix.GetComponent<BondingCooldownScript>().PolyvalentBondInitiated == true)
            {
                print("second joint added now");
                joint2 = BondingPartner.AddComponent<RelativeJoint2D>();            //BondingPartner can only be O or C 
                joint2.connectedBody = gameObject.GetComponent<Rigidbody2D>();    //THIS CAN BE USED TO TRACE CONTACTS
                joint2.autoConfigureOffset = false;              //if this bool is true, the joint won't hold when object is dragged!
                joint2.enableCollision = false;                         //so no additional joints will be created (avoid infinite loop)
                joint2.linearOffset = VectorOffset;       //this Vector2 is defined by the InternalBondingSite of the BondingPartner atom
                print("Joint2 Offset " + joint2.linearOffset);
                BondEnergyMatrix.GetComponent<BondingCooldownScript>().PolyvalentBondInitiated = false;    //the second joint has been applied and now we can exit the bond forming sequence
                return;
            }

            else if (UnbondingScript2.DontBondAgain > 0)
            {
                print("DontBondAgain timer value = " + UnbondingScript2.DontBondAgain);
                print("RETURN");   //can't initiate a new bond when DontBondAgain is > 0, but the second joint can still be applied!
                return;
            }


            else if (BondEnergyMatrix.GetComponent<BondingCooldownScript>().BondingCooldownTimer == 0)  //This is the first bond formation--second bond 
            {
                print("Double Bonding Event initiated");
                //VectorOffset = new Vector2(1.5f, -1.5f); //collision.collider.GetComponent<VectorOffset>().BondingOffsetVector;
                print("BondingPartner = " + BondingPartner);
                BondingPartnerMoleculeID = BondingPartner.GetComponent<BondMaker>().MoleculeID;   //see what the Bonding partner's MoleculeID is
                BondEnergyMatrix.GetComponent<BondingCooldownScript>().PolyvalentBondInitiated = true;  //redundancy with line 584, which sets this variable to true if both bonding partners are polyvalent
                CalculateBondEnergies(true, BondingPartner);    //for Double Bonds, the VectorOffset script is attached to the BondingTriggerDB GameObject     
                MakeTheBonds();


            }

           

        }

        //print("this collider tag " + collision.collider.tag);
        //print("other collider tag = " + collision.otherCollider.tag);
    }

    void OnTriggerEnter2D(Collider2D collider)  //triggered by a BondingTrigger colliding with another collider 
    {
        //print("TriggerEnter");

        //ADDED THIS SECTION USING INTERNALBONDINGSITES AND BONDINGCOOLDOWNTIMER--need to add the BondingCooldownTimer to BondingCooldownScript script

        if (collider.tag == "InternalBondingSite" && BondEnergyMatrix.GetComponent<BondingCooldownScript>().PolyvalentBondInitiated == true)
        {
            
            print("collider = " + collider);

            VectorOffset = collider.GetComponent<VectorOffset>().BondingOffsetVector;
            BondingPartner = collider.transform.root.gameObject;  //add bonding partner to array of molecule's atoms
            print("BondingPartner = " + BondingPartner);
            
            print("second joint applied now");
            joint2 = BondingPartner.AddComponent<RelativeJoint2D>();            //BondingPartner can only be O or C 
            joint2.connectedBody = gameObject.GetComponent<Rigidbody2D>();    //THIS CAN BE USED TO TRACE CONTACTS

            joint2.autoConfigureOffset = false;              //if this bool is true, the joint won't hold when object is dragged!
            joint2.enableCollision = false;                         //so no additional joints will be created (avoid infinite loop)
            joint2.linearOffset = VectorOffset;       //this Vector2 is defined by the InternalBondingSite of the BondingPartner atom
            print("Joint2 Offset " + joint2.linearOffset);
            BondEnergyMatrix.GetComponent<BondingCooldownScript>().PolyvalentBondInitiated = false;  //after 2nd joint has been applied, the bonding is complete
            

        }


        else if (UnbondingScript2.DontBondAgain > 0)  //THIS IS A PROBLEM BECAUSE DONTBONDAGAIN IS > 0 WHILE JEWELS ARE IN MOTION. . .
        {
            //print("RETURN because DontBondAgain is > 0");
            return;
        }

       
        
        else if (collider.tag == "InternalBondingSite" && BondEnergyMatrix.GetComponent<BondingCooldownScript>().BondingCooldownTimer == 0)    //Cooldown timer is set to 20 when a bonding event is initiated.  20 fixed updates will take it back to zero
        {
            print("Single Bond initiated");
            VectorOffset = collider.GetComponent<VectorOffset>().BondingOffsetVector;
            BondingPartner = collider.transform.root.gameObject;  //add bonding partner to array of molecule's atoms
            BondingPartnerMoleculeID = BondingPartner.GetComponent<BondMaker>().MoleculeID;   //see what the Bonding partner's MoleculeID is
            CalculateBondEnergies(false, BondingPartner);  //Boolean is TRUE if double bond is being formed, FALSE for single bonds
            MakeTheBonds();
        }
    }


    //DELETE THE REFERENCES TO PEAKS AND PEAKDB
    //if (collider.tag == "Peak" || collider.tag == "PeakDB")     //only "Peaks" can make bonds with "Valleys"
    //{
    //    colliderCount = 1;  //this marker indicates that this atom has received a bonding trigger, but need confirmation from the other atom prior to bond formation
    //    //print("colliderCount =" + colliderCount);
    //    otherColliderCount = collider.transform.root.gameObject.GetComponent<BondMaker>().colliderCount;
    //    //print("otherColliderCount =" + otherColliderCount);

    //    if (otherColliderCount == 1)  //this means that the other atom has triggered simultaneously = requirement for bond formation
    //     {
    //        BondingPartner = collider.transform.root.gameObject;  //add bonding partner to array of molecule's atoms
    //        BondingPartnerMoleculeID = BondingPartner.GetComponent<BondMaker>().MoleculeID;   //see what the Bonding partner's MoleculeID is

    //        /*if(GameObject.Find("TutorialMarker").GetComponent<TutorialScript>().Tutorial == true && DieScript.totalRolls == 3)
    //        {
    //            if(gameObject.tag == BondingPartner.tag)
    //            {
    //                print("disabled H-H bonding on this turn");
    //                return;  //disable H-H bonding on turn 3 of the Tutorial.  Force player to bond H to C.
    //            }
    //        }*/

    //        if (MoleculeID == 0  && BondingPartnerMoleculeID == 0)  //New molecule has begun!  Only occurs when both MoleculeID = zero
    //        {
    //            print("0-0, started a new molecule");
    //            //NewMoleculeList.Clear();  //DANGEROUS BECAUSE IT COULD CLEAR A MOLECULELIST THAT SHOULDN'T BE EMPTIED!!!
    //            for (i=1;i<13;i++)   //Slots 1-12 in the array are used to store Molecules (atoms in the molecule)
    //            {
    //                if (AtomInventory.MoleculeList[i]== null || AtomInventory.MoleculeList[i].Count==0)
    //                {
    //                    Index = i;      //Index finds the lowest empty MoleculeList slot
    //                    print("lowest open slot = " + i);
    //                    break;          //to abort the loop after the first empty slot is found
    //                }
    //            }
    //            MoleculeID = Index;   //Index is used to assign an unused MoleculeID value (max MoleculeID = 12)
    //            BondingPartner.GetComponent<BondMaker>().MoleculeID = Index;    //assign the same MoleculeID to both bonding atoms
    //            AtomInventory.MoleculeList[Index] = new List<GameObject>();  // Initialize NewMoleculeList!  IMPORTANT--otherwise Unity says "instance does not exist"
    //            AtomInventory.MoleculeList[Index].Add(gameObject);  //put this atom in the list
    //            AtomInventory.MoleculeList[Index].Add(BondingPartner);  //put bonding partner in the list
    //        }

    //        else if (MoleculeID == 0 && BondingPartnerMoleculeID > 0)  //this means the Bonding Partner already has a MoleculeID
    //        {
    //            print("gameObject" + gameObject + " took on Bonding Partner ID");
    //            MoleculeID = BondingPartnerMoleculeID;  //newly bonded atom takes on the MoleculeID of its partner
    //        }

    //        else if (MoleculeID > 0 && BondingPartnerMoleculeID == 0)    //if this bonding GameObject already has a MoleculeID (it was bonded earlier)
    //        {
    //            print("Bonding Partner took on ID of" + gameObject);
    //            BondingPartner.GetComponent<BondMaker>().MoleculeID = MoleculeID;  //BondingPartner takes on this atom's MoleculeID
    //        }


    //        else //if (MoleculeID > 0 && BondingPartnerMoleculeID > 0)  //&& MoleculeID != BondingPartnerMoleculeID
    //        {
    //            print("Merging Molecule Lists");
    //            print("MoleculeID of this gameObject = " + MoleculeID);
    //            print("BondingPartnerMoleculeID =" + BondingPartnerMoleculeID);
    //            if(MoleculeID == BondingPartnerMoleculeID)
    //            {
    //                print("Cyclic molecule formed!");
    //                AtomInventory.MoleculeList[MoleculeID].Add(CyclicToken);
    //                //form the bond
    //                //skip molecule ID assignment
    //                //decrement valleys remaining
    //                //bond energy calculation
    //            }


    //            else       //Merging lists--all atoms take on MoleculeID of this Molecule--then BondingPartnerMoleculeID is emptied
    //            { 
    //                foreach (GameObject atom in AtomInventory.MoleculeList[BondingPartnerMoleculeID])  
    //                {
    //                    AtomInventory.MoleculeList[MoleculeID].Add(atom);          //add each atom in Bonding Partner List to this MoleculeList[ID]
    //                    atom.GetComponent<BondMaker>().MoleculeID = MoleculeID;              //change the MoleculeID of each atom that is moved to new list
    //                }
    //                print("emptied Bonding Partner Molecule List");
    //                AtomInventory.MoleculeList[BondingPartnerMoleculeID].Clear();  //makes the list empty, but not "null"
    //            }
    //        }


    //        //NEXT SECTION ADDS RELATIVEJOINT2D TO THE APPROPRIATE ATOMS--TWO JOINTS CREATED WHEN POLYVALENT ATOMS BOND
    //        if (gameObject.tag == "Hydrogen" || gameObject.tag == "Chlorine")          //joints preferentially localized on hydrogen/Cl atoms--easier to unbond
    //        {
    //            print("Joint added to this Monovalent atom");     //Monovalent atoms get only one joint
    //            joint = gameObject.AddComponent<RelativeJoint2D>();                 //joint links to centers of bonding atoms                                                               
    //            joint.connectedBody = BondingPartner.GetComponent<Rigidbody2D>();     //parent of the "BondingTrigger"
    //            joint.autoConfigureOffset = false;              //if this bool is true, the joint won't hold when object is dragged!
    //            joint.enableCollision = false;                         //so no additional joints will be created (avoid infinite loop)
    //            joint.linearOffset = new Vector2(1.73f, 0);
    //            joint.angularOffset = 0;
    //        }

    //        else if (BondingPartner.GetComponent<BondMaker>().Monovalent)
    //        {
    //            print("joint added to monovalent bonding partner");                //the other atom might be monovalent--just one bond formed
    //            joint = BondingPartner.AddComponent<RelativeJoint2D>();            //bondingpartner could be h, o, c, cl. . .
    //            joint.connectedBody = gameObject.GetComponent<Rigidbody2D>();
    //            joint.autoConfigureOffset = false;              //if this bool is true, the joint won't hold when object is dragged!
    //            joint.enableCollision = false;                         //so no additional joints will be created (avoid infinite loop)
    //            joint.linearOffset = new Vector2(1.73f, 0);

    //        }

    //        else if (!gameObject.GetComponent<BondMaker>().Monovalent && !BondingPartner.GetComponent<BondMaker>().Monovalent)    //if neither atom is Monovalent, add joints to both atoms!
    //        {
    //            print("Joints added to polyvalent Bonding Partner AND to this atom");
    //            joint1 = BondingPartner.AddComponent<RelativeJoint2D>();            //BondingPartner can only be O or C 
    //            joint1.connectedBody = gameObject.GetComponent<Rigidbody2D>();    //THIS CAN BE USED TO TRACE CONTACTS

    //            joint1.autoConfigureOffset = false;              //if this bool is true, the joint won't hold when object is dragged!
    //            joint1.enableCollision = false;                         //so no additional joints will be created (avoid infinite loop)
    //            joint1.linearOffset = VectorOffset;       //this Vector2 is defined by the InternalBondingSite of the BondingPartner atom
    //                                                      //print("Joint1 Offset " + joint1.linearOffset);

    //        }

    ////NEXT SECTION ADDS FIXEDJOINT2D TO THE APPROPRIATE ATOMS--TWO JOINTS CREATED WHEN POLYVALENT ATOMS BOND
    //if (gameObject.tag == "Hydrogen" || gameObject.tag == "Chlorine")          //joints preferentially localized on hydrogen/Cl atoms--easier to unbond
    //{
    //    print("Joint added to Monovalent atom");     //Monovalent atoms get only one joint
    //    joint = gameObject.AddComponent<FixedJoint2D>();                 //joint links to centers of bonding atoms                                                               
    //    joint.connectedBody = BondingPartner.GetComponent<Rigidbody2D>();     //parent of the "Peak"
    //    joint.autoConfigureConnectedAnchor = false;              //if this bool is true, the joint won't hold when object is dragged!
    //    joint.enableCollision = false;                         //so no additional joints will be created (avoid infinite loop)
    //}
    //else if (BondingPartner.GetComponent<BondMaker>().Monovalent)
    //{
    //    print("Joint added to monovalent Bonding Partner");                //the other atom might be monovalent--just one bond formed
    //    joint = BondingPartner.AddComponent<FixedJoint2D>();            //BondingPartner could be H, O, C, Cl. . .
    //    joint.connectedBody = gameObject.GetComponent<Rigidbody2D>();
    //    joint.autoConfigureConnectedAnchor = false;              //if this bool is true, the joint won't hold when object is dragged!
    //    joint.enableCollision = false;                         //so no additional joints will be created (avoid infinite loop)
    //}

    //else if (!gameObject.GetComponent<BondMaker>().Monovalent && !BondingPartner.GetComponent<BondMaker>().Monovalent)    //if neither atom is Monovalent, add joints to both atoms!
    //{
    //    print("Joints added to polyvalent Bonding Partner AND to this atom");
    //    joint1 = BondingPartner.AddComponent<FixedJoint2D>();            //BondingPartner can only be O or C 
    //    joint1.connectedBody = gameObject.GetComponent<Rigidbody2D>();    //THIS CAN BE USED TO TRACE CONTACTS
    //    joint2 = gameObject.AddComponent<FixedJoint2D>();          //Adding double joints helps contact tracing in the Unbonding script!
    //    joint2.connectedBody = BondingPartner.GetComponent<Rigidbody2D>();  //joints on both bonding atoms!!
    //    joint1.autoConfigureConnectedAnchor = false;              //if this bool is true, the joint won't hold when object is dragged!
    //    joint1.enableCollision = false;                         //so no additional joints will be created (avoid infinite loop)
    //    joint2.autoConfigureConnectedAnchor = false;
    //    joint2.enableCollision = false;
    //}


    //        //This section is used to put atoms in appropriate MoleculeLists using MoleculeID values
    //        TempAtomList = AtomInventory.MoleculeList[MoleculeID];      //TempAtomList gets the stored list from MoleculeList Array

    //        if (TempAtomList.Contains(gameObject))    
    //        {
    //            print(gameObject + " already in list");  //avoid duplication of GameObjects in the list
    //        }

    //        else
    //        {
    //            print("Added to TempAtomList" + gameObject);
    //            TempAtomList.Add(gameObject);  //add this gameObject to the list that will be stored in MoleculeList[] under MoleculeListKeeper
    //            AtomInventory.MoleculeList[MoleculeID] = TempAtomList;  //pushes the TempAtomList into this molecule's ListKeeper Slot
    //        }

    //        if (TempAtomList.Contains(BondingPartner))   
    //        { 
    //            print("BP already in list");  //no duplication of atoms desired!
    //        }

    //        else
    //        {
    //            print("Added " +BondingPartner + "to TempAtomList");
    //            TempAtomList.Add(BondingPartner);       //add BondingPartner to Atom List for storage in MoleculeList[]  
    //            AtomInventory.MoleculeList[MoleculeID] = TempAtomList;  //TempAtomList pushed to MoleculeList[] array
    //        }


    //        //maintenance of bonding states and valley counts
    //        bonded = true;          //bonded state disables atom rotation
    //        valleysRemaining--;         //decrement number of bonding spots to fill on this atom
    //        collider.transform.root.gameObject.GetComponent<BondMaker>().valleysRemaining--;    //decrease bonding slots on BondingPartner
    //        collider.transform.root.gameObject.GetComponent<BondMaker>().bonded = true;        //set BondingPartner to bonded state
    //        SoundFX.Play();



    //        //this section of code finds the Bond Energy value in the 2D bondArray --needs identity of the two atoms making the bond (order is irrelevant)

    //        bondArray = GameObject.Find("BondEnergyMatrix").GetComponent<BondEnergyValues>().bondEnergyArray; //accesses the array of Bond Energies

    //        if (collider.tag == "PeakDB")  //if double bond, need to use BondArrayID 4 (for double bonded carbon) or 5 (for double bonded oxygen)
    //        {
    //            BondEnergy = bondArray[gameObject.GetComponent<BondMaker>().bondArrayID+3, collider.transform.root.gameObject.GetComponent<BondMaker>().bondArrayID+3];
    //        }
    //        else
    //        {
    //            BondEnergy = bondArray[gameObject.GetComponent<BondMaker>().bondArrayID, collider.transform.root.gameObject.GetComponent<BondMaker>().bondArrayID];
    //        }

    //        print("BondEnergy =" +BondEnergy);  
    //        DisplayCanvasScript.JoulesOfHeat += BondEnergy;        //this updates the total joule count from all bonds that the player has formed so far

    //        if (gameObject.GetComponent<PotentialEnergy>().useJewelPrefab == true)
    //        {
    //            gameObject.GetComponent<PotentialEnergy>().PE -= BondEnergy / 2f;   //PE decreases when bond forms--half of the Joules taken from each atom
    //            BondingPartner.GetComponent<PotentialEnergy>().PE -= BondEnergy / 2f;

    //            gameObject.GetComponent<PotentialEnergy>().PotentialEnergyAdjust();  //This function displays PE Jewels on the atoms
    //            BondingPartner.GetComponent<PotentialEnergy>().PotentialEnergyAdjust();



    //            //MOVE IMAGES OF JOULES FROM ATOMS (PE) TO JOULE CORRAL (HEAT)
    //            MovingJewel = Instantiate(MovingJewels[BondEnergy]) as GameObject;  //Either 1 Jewel, 2 Jewels, 3 Jewels or 4 Jewels 
    //            MovingJewel.transform.position = gameObject.transform.position;  //starting point for the flying joules is this atom's position
    //            GameObject.FindWithTag("MovingJewel").GetComponent<JewelMover>().MovingJewel(BondEnergy);  //this is the function that makes Jewels fly to Joule Corral

    //            //JewelMover Script does the following:
    //            //Set target location (center of the Joule Corral)
    //            //Set velocity so that the jewel moves to the target location  (done using MoveTowards function)
    //            //When jewel reaches target location, instantiate red joules--done using JSpawn (JSpawn is in JouleHolderScript)
    //        }



    //        //this next section counts the number of unfilled bonding slots ("valleys") to determine if molecule is complete!
    //        print("TempAtomList atom count" + TempAtomList.Count);
    //        for (i = 0; i < TempAtomList.Count; i++)   
    //        {
    //            print(TempAtomList[i]);
    //            totalValleysRem += TempAtomList[i].GetComponent<BondMaker>().valleysRemaining;  //TempAtomList is the list of all atoms belonging to the molecule formed by this bonding event
    //        }

    //        //totalValleysRem = counts up the empty slots on all the atoms in the molecule   
    //        print("total valleys remaining =" + totalValleysRem);
    //        if(totalValleysRem == 0)
    //        {
    //            print("molecule complete!!!!");         //i indicates the number of atoms in the molecule
    //            SoundFX3.Play();
    //            if (TempAtomList.Count>6)         //BonusPts max out at 6-atoms in a molecule
    //                { i = 6; }   
    //            BonusPts = GameObject.Find("MoleculeListKeeper").GetComponent<AtomInventory>().bonusPts[i];  //access the BonusPt Array
    //            print("point value of this molecule =" + BonusPts);     //+ GameObject.Find("MoleculeListKeeper").GetComponent<AtomInventory>().bonusPts[i]);
    //            DisplayCanvasScript.NetEnergyProfit += BonusPts;          //update NetEnergyProfit static variable
    //            MCToken = GameObject.Find("MoleculeListKeeper").GetComponent<MoleculeCompletionPtArray>().MoleculeCompletionToken[i];
    //            AtomInventory.MoleculeList[MoleculeID].Add(MCToken);  //adds a MoleculeCompletionToken to the MoleculeList Array
    //            MoleculeJustCompleted = true;
    //            GameObject.Find("MoleculeListKeeper").GetComponent<AtomInventory>().GetMoleculeCode();

    //            //HERE'S WHERE MCTOKENS GET ATTACHED TO MOLECULES
    //            print("i =" + i);
    //            if(i == 2)   ///applies to newly formed diatomic molecules (including HCl)
    //            {
    //                BadgeRecipient = gameObject;
    //            }

    //            else if (i > 2)  //the molecule has a carbon or oxygen center
    //            {
    //                foreach (GameObject atom in TempAtomList)
    //                {
    //                    if (atom.tag == "Oxygen")
    //                    {
    //                        BadgeRecipient = atom;
    //                        print("BadgeRecipient =" + BadgeRecipient);
    //                    }

    //                    if (atom.tag == "Carbon"  || atom.tag == "Carbon2xDB")  //carbon takes precedence over oxygen
    //                    {
    //                        BadgeRecipient = atom;
    //                        print("BadgeRecipient =" + BadgeRecipient);
    //                        break;
    //                    }
    //                }
    //            }  

    //            //print("applying badge now");

    //            GameObject NewBadge = Instantiate(MCToken, BadgeRecipient.transform);  //MCToken is now a Sprite!  BadgeRecipient.transform = the parent
    //            print("Instantiated Badge #" + i);
    //            NewBadge.transform.localPosition = new Vector3(-1.2f, 1f, 0);  //positions badge relative to the BadgeRecipient parent
    //            BadgeRotation = BadgeRecipient.transform.rotation.eulerAngles.z;//Get the Z-component of eulerAngles!!!
    //            NewBadge.transform.Rotate(0, 0, -BadgeRotation);  //undoes the THE z-component of parent Euler Angles!

    //        }
    //    }
    //}


    //ADDED THE FUNCTION "MakeTheBonds()"
    public void MakeTheBonds()
    {
        print("MakeTheBonds function called");
        //print("gameObject " + gameObject);
        //print("BondingPartner " + BondingPartner);
        //print("gameObject MolID " + MoleculeID);
        //print("BondingPartner MolID " + BondingPartnerMoleculeID);

        //SECTION 1 = ASSIGN MOLECULE ID VALUES TO EACH ATOM
        if (MoleculeID == 0 && BondingPartnerMoleculeID == 0)  //New molecule has begun!  Only occurs when both MoleculeID = zero
        {
            print("0-0, started a new molecule");
            //NewMoleculeList.Clear();  //DANGEROUS BECAUSE IT COULD CLEAR A MOLECULELIST THAT SHOULDN'T BE EMPTIED!!!
            for (i = 1; i < 13; i++)   //Slots 1-12 in the array are used to store Molecules (atoms in the molecule)
            {
                if (AtomInventory.MoleculeList[i] == null || AtomInventory.MoleculeList[i].Count == 0)
                {
                    Index = i;      //Index finds the lowest empty MoleculeList slot
                    print("lowest open slot = " + i);
                    break;          //to abort the loop after the first empty slot is found
                }
            }
            MoleculeID = Index;   //Index is used to assign an unused MoleculeID value (max MoleculeID = 12)
            BondingPartner.GetComponent<BondMaker>().MoleculeID = Index;    //assign the same MoleculeID to both bonding atoms
            AtomInventory.MoleculeList[Index] = new List<GameObject>();  // Initialize NewMoleculeList!  IMPORTANT--otherwise Unity says "instance does not exist"
            AtomInventory.MoleculeList[Index].Add(gameObject);  //put this atom in the list
            AtomInventory.MoleculeList[Index].Add(BondingPartner);  //put bonding partner in the list
        }

        else if (MoleculeID == 0 && BondingPartnerMoleculeID > 0)  //this means the Bonding Partner already has a MoleculeID
        {
            print("gameObject" + gameObject + " took on Bonding Partner ID");
            MoleculeID = BondingPartnerMoleculeID;  //newly bonded atom takes on the MoleculeID of its partner
        }

        else if (MoleculeID > 0 && BondingPartnerMoleculeID == 0)    //if this bonding GameObject already has a MoleculeID (it was bonded earlier)
        {
            print("Bonding Partner took on ID of" + gameObject);
            BondingPartner.GetComponent<BondMaker>().MoleculeID = MoleculeID;  //BondingPartner takes on this atom's MoleculeID
        }


        else //if (MoleculeID > 0 && BondingPartnerMoleculeID > 0)  //&& MoleculeID != BondingPartnerMoleculeID
        {
            print("Merging Molecule Lists");
            print("MoleculeID of this gameObject = " + MoleculeID);
            print("BondingPartnerMoleculeID =" + BondingPartnerMoleculeID);
            if (MoleculeID == BondingPartnerMoleculeID)
            {
                print("Cyclic molecule formed!");
                //AtomInventory.MoleculeList[MoleculeID].Add(CyclicToken);
                //form the bond
                //skip molecule ID assignment
                //decrement valleys remaining
                //bond energy calculation
            }


            else       //Merging lists--all atoms take on MoleculeID of this Molecule--then BondingPartnerMoleculeID is emptied
            {
                foreach (GameObject atom in AtomInventory.MoleculeList[BondingPartnerMoleculeID])
                {
                    AtomInventory.MoleculeList[MoleculeID].Add(atom);          //add each atom in Bonding Partner List to this MoleculeList[ID]
                    atom.GetComponent<BondMaker>().MoleculeID = MoleculeID;              //change the MoleculeID of each atom that is moved to new list
                }
                print("emptied Bonding Partner Molecule List");
                AtomInventory.MoleculeList[BondingPartnerMoleculeID].Clear();  //makes the list empty, but not "null"
            }
        }

        //SECTION2:  ADDS RELATIVEJOINT2D TO THE APPROPRIATE ATOMS--TWO JOINTS CREATED WHEN POLYVALENT ATOMS BOND
        if (gameObject.tag == "Hydrogen" || gameObject.tag == "Chlorine")          //joints preferentially localized on hydrogen/Cl atoms--easier to unbond
        {
            print("Joint added to this Monovalent atom");     //Monovalent atoms get only one joint
            joint = gameObject.AddComponent<RelativeJoint2D>();                 //joint links to centers of bonding atoms                                                               
            joint.connectedBody = BondingPartner.GetComponent<Rigidbody2D>();     //parent of the "BondingTrigger"
            joint.autoConfigureOffset = false;              //if this bool is true, the joint won't hold when object is dragged!
            joint.enableCollision = false;                         //so no additional joints will be created (avoid infinite loop)
            joint.linearOffset = new Vector2(1.73f, 0);
            joint.angularOffset = 0;
        }

        else if (BondingPartner.GetComponent<BondMaker>().Monovalent)
        {
            print("joint added to monovalent bonding partner");                //the other atom might be monovalent--just one bond formed
            joint = BondingPartner.AddComponent<RelativeJoint2D>();            //bondingpartner could be h, o, c, cl. . .
            joint.connectedBody = gameObject.GetComponent<Rigidbody2D>();
            joint.autoConfigureOffset = false;              //if this bool is true, the joint won't hold when object is dragged!
            joint.enableCollision = false;                         //so no additional joints will be created (avoid infinite loop)
            joint.linearOffset = new Vector2(1.73f, 0);

        }

        else if (!gameObject.GetComponent<BondMaker>().Monovalent && !BondingPartner.GetComponent<BondMaker>().Monovalent)    //if neither atom is Monovalent, add joints to both atoms!
        {
            print("Joints added to polyvalent Bonding Partner AND to this atom");
            joint1 = BondingPartner.AddComponent<RelativeJoint2D>();            //BondingPartner can only be O or C 
            joint1.connectedBody = gameObject.GetComponent<Rigidbody2D>();    //THIS CAN BE USED TO TRACE CONTACTS

            joint1.autoConfigureOffset = false;              //if this bool is true, the joint won't hold when object is dragged!
            joint1.enableCollision = false;                         //so no additional joints will be created (avoid infinite loop)
            joint1.linearOffset = VectorOffset;       //this Vector2 is defined by the InternalBondingSite of the BondingPartner atom
            //print("Joint1 Offset " + joint1.linearOffset);
            BondEnergyMatrix.GetComponent<BondingCooldownScript>().PolyvalentBondInitiated = true;


        }

        //MoleculeNamesForCheckBox.TimeLastBondWasFormed = Time.time;


        //SECTION 3:  put atoms in appropriate MoleculeLists using MoleculeID values
        TempAtomList = AtomInventory.MoleculeList[MoleculeID];      //TempAtomList gets the stored list from MoleculeList Array

        if (TempAtomList.Contains(gameObject))
        {
            //print(gameObject + " already in list");  //avoid duplication of GameObjects in the list
        }

        else
        {
            print("Added to TempAtomList" + gameObject);
            TempAtomList.Add(gameObject);  //add this gameObject to the list that will be stored in MoleculeList[] under MoleculeListKeeper
            AtomInventory.MoleculeList[MoleculeID] = TempAtomList;  //pushes the TempAtomList into this molecule's ListKeeper Slot
        }

        if (TempAtomList.Contains(BondingPartner))
        {
            //print("BP already in list");  //no duplication of atoms desired!
        }

        else
        {
            print("Added " + BondingPartner + "to TempAtomList");
            TempAtomList.Add(BondingPartner);       //add BondingPartner to Atom List for storage in MoleculeList[]  
            AtomInventory.MoleculeList[MoleculeID] = TempAtomList;  //TempAtomList pushed to MoleculeList[] array
        }


        //SECTION 4:  maintenance of bonding states and valley counts
        bonded = true;          //bonded state disables atom rotation
        valleysRemaining--;         //decrement number of bonding spots to fill on this atom
        BondingPartner.GetComponent<BondMaker>().valleysRemaining--;    //decrease bonding slots on BondingPartner
        BondingPartner.GetComponent<BondMaker>().bonded = true;        //set BondingPartner to bonded state
        SoundFX.Play();
        BondEnergyMatrix.GetComponent<BondingCooldownScript>().BondingCooldownTimer = 20;

       

        //SECTION 5:  counts the number of unfilled bonding slots ("valleys") to determine if molecule is complete!
        //print("TempAtomList atom count" + TempAtomList.Count);

        //totalValleysRem = counts up the empty slots on all the atoms in the molecule   
        totalValleysRem = 0;  //reset the open bonding slot count so that the tallying of Valleys Remaining starts at zero    
        for (i = 0; i < TempAtomList.Count; i++)
        {
            //print(TempAtomList[i]);
            totalValleysRem += TempAtomList[i].GetComponent<BondMaker>().valleysRemaining;  //TempAtomList is the list of all atoms belonging to the molecule formed by this bonding event
            //print("Valleys remaining = " + totalValleysRem);
        }


        //print("total valleys remaining =" + totalValleysRem);
        if (totalValleysRem == 0)
        {
            print("molecule complete!!!!");         //i indicates the number of atoms in the molecule
            SoundFX3.Play();


            //if (TempAtomList.Count > 10)         //For 2-D Model Kit, max is 10+ atoms atoms in a molecule
            //{
            //    i = 10;
            //}

            //MCToken = GameObject.Find("MoleculeListKeeper").GetComponent<MoleculeCompletionPtArray>().MoleculeCompletionToken[i];
            //AtomInventory.MoleculeList[MoleculeID].Add(MCToken);  //adds a MoleculeCompletionToken to the MoleculeList Array
            //MoleculeJustCompleted = true;

            //SECTION 6:  CALCULATE TOTAL PE IN THE MOLECULE--THE BADGE DISPLAYS TOTAL PE IN THE MOLECULE!!!
            TotalPEInThisMolecule = 0;

            foreach (var atom in TempAtomList)
            {
                //print(atom.name + atom.GetComponent<PotentialEnergy>().PE);
                TotalPEInThisMolecule += atom.GetComponent<PotentialEnergy>().PE;
                //print("TotalPE = " + TotalPEInThisMolecule);
            }



            if (TotalPEInThisMolecule > 10)         //PE badges max out at 10+
            {
                TotalPEInThisMolecule = 10;
            }

            MCToken = GameObject.Find("MoleculeListKeeper").GetComponent<MoleculeCompletionPtArray>().MoleculeCompletionToken[Mathf.RoundToInt(TotalPEInThisMolecule)];
            AtomInventory.MoleculeList[MoleculeID].Add(MCToken);  //adds a MoleculeCompletionToken to the MoleculeList Array
            MoleculeJustCompleted = true;
            

            //SECTION 7:  HERE'S WHERE MCTOKENS GET ATTACHED TO MOLECULES
            print("i =" + i);
            if (i == 2)   ///applies to newly formed diatomic molecules (including HCl)
            {
                BadgeRecipient = gameObject;
            }

            else if (i > 2)  //the molecule has a carbon or oxygen center
            {
                foreach (GameObject atom in TempAtomList)
                {
                    if (atom.tag == "Oxygen")
                    {
                        BadgeRecipient = atom;
                        print("BadgeRecipient =" + BadgeRecipient);
                    }

                    if (atom.tag == "Nitrogen")  //nitrogen receives badge over oxygen
                    {
                        BadgeRecipient = atom;
                        print("BadgeRecipient =" + BadgeRecipient);
                    }

                    if (atom.tag == "Carbon")  //carbon takes precedence over oxygen or nitrogen
                    {
                        BadgeRecipient = atom;
                        print("BadgeRecipient =" + BadgeRecipient);
                        break;
                    }
                }
            }

            //print("applying badge now");
            GameObject NewBadge = Instantiate(MCToken, BadgeRecipient.transform);  //MCToken is now a Sprite!  BadgeRecipient.transform = the parent
            //print("Instantiated Badge #" + i);
            NewBadge.transform.localPosition = new Vector3(-1.2f, 1f, 0);  //positions badge relative to the BadgeRecipient parent
            BadgeRotation = BadgeRecipient.transform.rotation.eulerAngles.z;//Get the Z-component of eulerAngles!!!
            NewBadge.transform.Rotate(0, 0, -BadgeRotation);  //undoes the THE z-component of parent Euler Angles!

            GameObject.Find("MoleculeListKeeper").GetComponent<AtomInventory>().GetMoleculeCode(); //Upon completion of a molecule, check to see if it matches an expected MoleculeCode

            

        }

        


        //}
    }

    //SECTION 8:  ADDED THE FUNCTION "CalculateBondEnergies(bool DB, GameObject Atom2)"
    public void CalculateBondEnergies(bool DB, GameObject Atom2)
    {
        //this section of code finds the Bond Energy value in the 2D bondArray --needs identity of the two atoms making the bond (order is irrelevant)
        print("CalculatingBondEnergies");

        bondArray = GameObject.Find("BondEnergyMatrix").GetComponent<BondEnergyValues>().bondEnergyArray; //accesses the array of Bond Energies

        if (DB)  //if double bond, need to use BondArrayID 4 (for double bonded carbon) or 5 (for double bonded oxygen)
        {
            BondEnergy = bondArray[gameObject.GetComponent<BondMaker>().bondArrayID + 3, Atom2.GetComponent<BondMaker>().bondArrayID + 3];
        }
        else
        {
            BondEnergy = bondArray[gameObject.GetComponent<BondMaker>().bondArrayID, Atom2.GetComponent<BondMaker>().bondArrayID];  //set Atom2 to collider.transform.root.gameObject
        }

        print("BondEnergy =" + BondEnergy);

        //DISPLAYCANVAS IS THE ONLY PLACE WHERE ADVANCED BOND ENERGIES ARE MEANINGFUL.  NEED TO CHECK TO SEE WHETHER ADVANCED BOND ENERGIES ARE USED IN THIS SCENE. . .

        if(GameObject.Find("BondEnergyMatrixAdvanced"))
        {
            //print("gameObject = " + gameObject);
            //print("Atom2 = " + Atom2);
            //print("kJ per mole");
            int[,] AdvBondEnergyMatrix = GameObject.Find("BondEnergyMatrixAdvanced").GetComponent<kJperMoleBondEnergyScript>().AdvancedBondEnergyMatrix;
            int AdvancedBondEnergy;
            if (DB)  //if double bond, need to use BondArrayID 4 (for double bonded carbon) or 5 (for double bonded oxygen)
            {
                AdvancedBondEnergy = AdvBondEnergyMatrix[gameObject.GetComponent<BondMaker>().bondArrayID + 3, Atom2.GetComponent<BondMaker>().bondArrayID + 3];
            }
            else
            {
                AdvancedBondEnergy = AdvBondEnergyMatrix[gameObject.GetComponent<BondMaker>().bondArrayID, Atom2.GetComponent<BondMaker>().bondArrayID];  //set Atom2 to collider.transform.root.gameObject
            }
            
           
            GameObject.Find("ConversationDisplay").GetComponent<ConversationTextDisplayScript>().PEtoHeatConversion(AdvancedBondEnergy);  //displays the energy change in a text box

            DisplayCanvasScript.HeatReleasedWhenBondsForm += AdvancedBondEnergy;
        }

        else
        {
            DisplayCanvasScript.HeatReleasedWhenBondsForm += BondEnergy;        //this updates the total joule count from all bonds that the player has formed so far
        }
       

        if (gameObject.GetComponent<PotentialEnergy>().useJewelPrefab == true)
        {
            gameObject.GetComponent<PotentialEnergy>().PE -= BondEnergy / 2f;   //PE decreases when bond forms--half of the Joules taken from each atom
            BondingPartner.GetComponent<PotentialEnergy>().PE -= BondEnergy / 2f;

            gameObject.GetComponent<PotentialEnergy>().PotentialEnergyAdjust();  //This function displays PE Jewels on the atoms
            BondingPartner.GetComponent<PotentialEnergy>().PotentialEnergyAdjust();



            //MOVE IMAGES OF JOULES FROM ATOMS (PE) TO JOULE CORRAL (HEAT)
            MovingJewel = Instantiate(MovingJewels[BondEnergy]) as GameObject;  //Either 1 Jewel, 2 Jewels, 3 Jewels or 4 Jewels 
            MovingJewel.transform.position = gameObject.transform.position;  //starting point for the flying joules is this atom's position
            GameObject.FindWithTag("MovingJewel").GetComponent<JewelMover>().MovingJewel(BondEnergy);  //this is the function that makes Jewels fly to Joule Corral
            DisplayCanvasScript.JoulesOfHeat += BondEnergy;        //this updates the total joule count in the JouleCorral


            //JewelMover Script does the following:
            //Set target location (center of the Joule Corral)
            //Set velocity so that the jewel moves to the target location  (done using MoveTowards function)
            //When jewel reaches target location, instantiate red joules--done using JSpawn (JSpawn is in JouleHolderScript)
        }


    }


    // Update is called once per frame
    void FixedUpdate()
    {
        
        //if (UnbondingScript2.DontBondAgain > 0)   //this variable is set to 20 when unbonding event occurs  THIS NEEDS TO BE CALCULATED ON A SINGLE GAME OBJECT (FlameUIElement UnbondingAtomMoverScript)
        //{
        //   UnbondingScript2.DontBondAgain--;  //this is the delay "timer" so that unbonding and bonding don't occur simultaneously
        //   //print(UnbondingScript2.DontBondAgain);
        //} 
        
    }
}
