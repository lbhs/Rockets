using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BondMakerCopy : MonoBehaviour
{
    //This script makes a bond (Relative joint) when triggered by simultaneous collision of "peaks" and "valleys" Double Handshake
    //the script is attached to each atom

    //    RelativeJoint2D joint;
    //    RelativeJoint2D joint1;
    //    RelativeJoint2D joint2;

    //    public bool bonded;     //when bonded, atom no longer rotates
    //    public int colliderCount;  //the trigger value--need double handshake for a bond to form
    //    private int otherColliderCount;  //trigger on the other collider--completes double handshake
    //    public int valleysRemaining;  //number of bonding slots to fill:  H = 1, O = 2, N = 3, C = 4, etc.
    //    private int totalValleysRem;  //calculated each bonding event--when reaches zero, molecule is complete! All bonding slots filled
    //    public int bondArrayID;  //H = 0, C = 1, O = 2, Cl = 3, C with double bond = 4, O with double bond = 5 
    //    private int[,] bondArray;  //temporary storage of the bondEnergyArray stored in game object BondEnergyMatrix
    //    private int BondEnergy;  //value in joules of the bond that has just been formed
    //    private GameObject BondingPartner;  //the atom to which this atom has bonded
    //    private int i;  //counting integer in "for" loop
    //    public static int Index = 1;  //temporary variable used to assign Molecule ID values
    //    public int MoleculeID;
    //    private int BondingPartnerMoleculeID;
    //    private List<GameObject> TempAtomList;
    //    private List<GameObject> NewMoleculeList;
    //    private int BonusPts;           //Bonus Pt value for completed molecule
    //    private GameObject MCToken;
    //    public AudioSource SoundFX;     //Bond Formed Sound
    //    private AudioSource SoundFX3;   //Molecule Completion sound
    //    public bool Monovalent;
    //    public GameObject BadgeRecipient;
    //    public static bool MoleculeJustCompleted;
    //    public GameObject[] BadgeToPin = new GameObject[7];
    //    private float BadgeRotation;
    //    public GameObject CyclicToken;
    //    private GameObject MovingJewel;  //PE jewels move towards Joule Corral
    //    public GameObject[] MovingJewels = new GameObject[5];  //Images of One to Four Jewels fly, depending on Bond Energy
    //    public static float TimeSinceLastBond;
    //    public static float TimeLastBondFormed;
    //    public float DuplicateBondPreventionTimer;
    //    private GameObject BondEnergyMatrix;
    //    private Vector2 VectorOffset;

    //    //public GameObject CheckBox;



    //    // Start is called before the first frame update
    //    void Start()
    //    {
    //        //bonded = false;   THIS SHOULD BE DELETED WHENEVER SCENE STARTS WITH INTACT MOLECULES!!!!
    //        TempAtomList = new List<GameObject>();
    //        NewMoleculeList = new List<GameObject>();
    //        SoundFX = GameObject.Find("BondMadeSound").GetComponent<AudioSource>();
    //        SoundFX3 = GameObject.Find("MoleculeCompleteSound").GetComponent<AudioSource>();
    //        BondEnergyMatrix = GameObject.Find("BondEnergyMatrix");

    //        for (i = 0; i < 5; i++)
    //        {
    //            //MovingJewels[i] = GameObject.Find("BondEnergyMatrix").GetComponent<MovingJewelArrays>().MovingJewelIconsBlue[i];

    //        }


    //    }

    //    private void OnCollisionEnter2D(Collision2D collision)
    //    {

    //        if (UnbondingScript2.DontBondAgain > 0)
    //        {
    //            print("RETURN");
    //            return;
    //        }

    //        print(UnbondingScript2.DontBondAgain);
    //        print("this collider = " + collision.collider);
    //        print("other collider = " + collision.otherCollider);

    //        if (collision.collider.tag == "BondingTriggerDB" && collision.otherCollider.tag == "BondingTriggerDB")
    //        {
    //            BondingPartner = collision.collider.transform.root.gameObject;  //add bonding partner to array of molecule's atoms
    //            VectorOffset = collision.collider.GetComponent<VectorOffset>().BondingOffsetVector;

    //            if (BondEnergyMatrix.GetComponent<BondEnergyValues>().BondingCooldownTimer == 0)
    //            {
    //                print("Double Bonding Event initiated");
    //                //VectorOffset = new Vector2(1.5f, -1.5f); //collision.collider.GetComponent<VectorOffset>().BondingOffsetVector;
    //                print("BondingPartner = " + BondingPartner);
    //                BondingPartnerMoleculeID = BondingPartner.GetComponent<BondMaker>().MoleculeID;   //see what the Bonding partner's MoleculeID is
    //                                                                                                  //for Double Bonds, the VectorOffset script is attached to the BondingTriggerDB GameObject
    //                MakeTheBonds();
    //            }

    //            else
    //            {
    //                joint2 = BondingPartner.AddComponent<RelativeJoint2D>();            //BondingPartner can only be O or C 
    //                joint2.connectedBody = gameObject.GetComponent<Rigidbody2D>();    //THIS CAN BE USED TO TRACE CONTACTS
    //                joint2.autoConfigureOffset = false;              //if this bool is true, the joint won't hold when object is dragged!
    //                joint2.enableCollision = false;                         //so no additional joints will be created (avoid infinite loop)
    //                joint2.linearOffset = VectorOffset;       //this Vector2 is defined by the InternalBondingSite of the BondingPartner atom
    //                print("Joint2 Offset " + joint2.linearOffset);
    //            }

    //        }

    //        //print("this collider tag " + collision.collider.tag);
    //        //print("other collider tag = " + collision.otherCollider.tag);
    //    }



    //    void OnTriggerEnter2D(Collider2D collider)  //triggered by an object colliding with the CircleCollider2D named "Bonding Trigger".  "collider" is the Other gameObject's collider that triggered this Collider2D
    //    {
    //        print("TriggerEnter, BCT = " + BondEnergyMatrix.GetComponent<BondEnergyValues>().BondingCooldownTimer);
    //        if (UnbondingScript2.DontBondAgain > 0)
    //        {
    //            //print("RETURN");
    //            return;
    //        }

    //        if (collider.tag == "InternalBondingSite" && BondEnergyMatrix.GetComponent<BondEnergyValues>().BondingCooldownTimer > 0)  //this function makes a second bond between two multivalent atoms that have just initiated bonding
    //        {
    //            //print("Second Internal Bonding Site trigger detected");
    //            //print("Collider = " + collider.name);

    //            VectorOffset = collider.GetComponent<VectorOffset>().BondingOffsetVector;
    //            BondingPartner = collider.transform.root.gameObject;  //add bonding partner to array of molecule's atoms
    //            //BondingPartnerMoleculeID = BondingPartner.GetComponent<BondMaker>().MoleculeID;   //see what the Bonding partner's MoleculeID is

    //            if (Monovalent || BondingPartner.GetComponent<BondMaker>().Monovalent)  //Monovalent atoms don't require a second joint
    //            {
    //                print("no second joint added");
    //                return;
    //            }
    //            else
    //            {
    //                joint2 = BondingPartner.AddComponent<RelativeJoint2D>();            //BondingPartner can only be O or C 
    //                joint2.connectedBody = gameObject.GetComponent<Rigidbody2D>();    //THIS CAN BE USED TO TRACE CONTACTS

    //                joint2.autoConfigureOffset = false;              //if this bool is true, the joint won't hold when object is dragged!
    //                joint2.enableCollision = false;                         //so no additional joints will be created (avoid infinite loop)
    //                joint2.linearOffset = VectorOffset;       //this Vector2 is defined by the InternalBondingSite of the BondingPartner atom
    //                print("Joint2 Offset " + joint2.linearOffset);
    //            }

    //        }

    //        if (collider.tag == "InternalBondingSite" && BondEnergyMatrix.GetComponent<BondEnergyValues>().BondingCooldownTimer == 0)    //Cooldown timer is set to 20 when a bonding event is initiated.  20 fixed updates will take it back to zero
    //        {
    //            VectorOffset = collider.GetComponent<VectorOffset>().BondingOffsetVector;
    //            BondingPartner = collider.transform.root.gameObject;  //add bonding partner to array of molecule's atoms
    //            BondingPartnerMoleculeID = BondingPartner.GetComponent<BondMaker>().MoleculeID;   //see what the Bonding partner's MoleculeID is
    //            MakeTheBonds();
    //        }
    //    }





    //    public void MakeTheBonds()
    //    {
    //        print("gameObject " + gameObject);
    //        print("BondingPartner " + BondingPartner);
    //        print("gameObject MolID " + MoleculeID);
    //        print("BondingPartner MolID " + BondingPartnerMoleculeID);

    //        if (MoleculeID == 0 && BondingPartnerMoleculeID == 0)  //New molecule has begun!  Only occurs when both MoleculeID = zero
    //        {
    //            print("0-0, started a new molecule");
    //            //NewMoleculeList.Clear();  //DANGEROUS BECAUSE IT COULD CLEAR A MOLECULELIST THAT SHOULDN'T BE EMPTIED!!!
    //            for (i = 1; i < 13; i++)   //Slots 1-12 in the array are used to store Molecules (atoms in the molecule)
    //            {
    //                if (AtomInventory.MoleculeList[i] == null || AtomInventory.MoleculeList[i].Count == 0)
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
    //            if (MoleculeID == BondingPartnerMoleculeID)
    //            {
    //                print("Cyclic molecule formed!");
    //                //AtomInventory.MoleculeList[MoleculeID].Add(CyclicToken);
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

    //        //MoleculeNamesForCheckBox.TimeLastBondWasFormed = Time.time;


    //        //This section is used to put atoms in appropriate MoleculeLists using MoleculeID values
    //        TempAtomList = AtomInventory.MoleculeList[MoleculeID];      //TempAtomList gets the stored list from MoleculeList Array

    //        if (TempAtomList.Contains(gameObject))
    //        {
    //            //print(gameObject + " already in list");  //avoid duplication of GameObjects in the list
    //        }

    //        else
    //        {
    //            print("Added to TempAtomList" + gameObject);
    //            TempAtomList.Add(gameObject);  //add this gameObject to the list that will be stored in MoleculeList[] under MoleculeListKeeper
    //            AtomInventory.MoleculeList[MoleculeID] = TempAtomList;  //pushes the TempAtomList into this molecule's ListKeeper Slot
    //        }

    //        if (TempAtomList.Contains(BondingPartner))
    //        {
    //            //print("BP already in list");  //no duplication of atoms desired!
    //        }

    //        else
    //        {
    //            print("Added " + BondingPartner + "to TempAtomList");
    //            TempAtomList.Add(BondingPartner);       //add BondingPartner to Atom List for storage in MoleculeList[]  
    //            AtomInventory.MoleculeList[MoleculeID] = TempAtomList;  //TempAtomList pushed to MoleculeList[] array
    //        }


    //        //maintenance of bonding states and valley counts
    //        bonded = true;          //bonded state disables atom rotation
    //        valleysRemaining--;         //decrement number of bonding spots to fill on this atom
    //        BondingPartner.GetComponent<BondMaker>().valleysRemaining--;    //decrease bonding slots on BondingPartner
    //        BondingPartner.GetComponent<BondMaker>().bonded = true;        //set BondingPartner to bonded state
    //        SoundFX.Play();
    //        BondEnergyMatrix.GetComponent<BondEnergyValues>().BondingCooldownTimer = 20;

    //        if (TutorialSpeechBubbleScript.TutorialMessageNumber == 5 || TutorialSpeechBubbleScript.TutorialMessageNumber == 9 || TutorialSpeechBubbleScript.TutorialMessageNumber == 19 || TutorialSpeechBubbleScript.TutorialMessageNumber == 21 || TutorialSpeechBubbleScript.TutorialMessageNumber == 24)
    //        {
    //            GameObject.Find("TutorialSpeechBubble").GetComponent<TutorialSpeechBubbleScript>().SendTutorialMessage();
    //            RotateThis.DisableRotation = false;
    //        }



    //        //this next section counts the number of unfilled bonding slots ("valleys") to determine if molecule is complete!
    //        //print("TempAtomList atom count" + TempAtomList.Count);

    //        //totalValleysRem = counts up the empty slots on all the atoms in the molecule   
    //        totalValleysRem = 0;  //reset the open bonding slot count so that the tallying of Valleys Remaining starts at zero    
    //        for (i = 0; i < TempAtomList.Count; i++)
    //        {
    //            print(TempAtomList[i]);
    //            totalValleysRem += TempAtomList[i].GetComponent<BondMaker>().valleysRemaining;  //TempAtomList is the list of all atoms belonging to the molecule formed by this bonding event
    //        }


    //        //print("total valleys remaining =" + totalValleysRem);
    //        if (totalValleysRem == 0)
    //        {
    //            print("molecule complete!!!!");         //i indicates the number of atoms in the molecule
    //            SoundFX3.Play();
    //            if (TempAtomList.Count > 10)         //For 2-D Model Kit, max is 10+ atoms atoms in a molecule
    //            {
    //                i = 10;
    //            }
    //            //BonusPts = GameObject.Find("MoleculeListKeeper").GetComponent<AtomInventory>().bonusPts[i];  //access the BonusPt Array
    //            //print("point value of this molecule =" + BonusPts);     //+ GameObject.Find("MoleculeListKeeper").GetComponent<AtomInventory>().bonusPts[i]);
    //            //DisplayCanvasScript.BonusPointTotal += BonusPts;          //update BonusPointTotal static variable
    //            MCToken = GameObject.Find("MoleculeListKeeper").GetComponent<MoleculeCompletionPtArray>().MoleculeCompletionToken[i];
    //            AtomInventory.MoleculeList[MoleculeID].Add(MCToken);  //adds a MoleculeCompletionToken to the MoleculeList Array
    //            MoleculeJustCompleted = true;

    //            //HERE'S WHERE MCTOKENS GET ATTACHED TO MOLECULES
    //            print("i =" + i);
    //            if (i == 2)   ///applies to newly formed diatomic molecules (including HCl)
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

    //                    if (atom.tag == "Nitrogen")  //nitrogen receives badge over oxygen
    //                    {
    //                        BadgeRecipient = atom;
    //                        print("BadgeRecipient =" + BadgeRecipient);
    //                    }

    //                    if (atom.tag == "Carbon")  //carbon takes precedence over oxygen or nitrogen
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

    //            //if (GameObject.Find("MoleculeListKeeper").GetComponent<AtomInventory>().ThisIsThe10MoleculeScene == true)
    //            //{
    //            //    GameObject.Find("MoleculeListKeeper").GetComponent<AtomInventory>().TenMoleculeSceneCheckBox(MoleculeID);  //This checks current molecule vs GREEN CHECK BOX code
    //            //                                                                                                               //SEND MOLECULE ID OF THE NEWLY COMPLETED MOLECULE!  THIS IS THE ONLY ONE TO CHECK IN THE 10 MOLECULE SCENE!!!

    //            //}

    //            //else if (TutorialSpeechBubbleScript.TutorialMessageNumber == 26)
    //            //{
    //            //    GameObject.Find("MoleculeCheckBoxButton1").GetComponent<Image>().color = new Color32(107, 245, 6, 255);   //turns checkbox green when bond is formed to complete molecule
    //            //}

    //            //else  //the script below will be active only in the Reaction Simulator where each product has a check box!
    //            //{
    //            //    //GameObject.Find("MoleculeListKeeper").GetComponent<AtomInventory>().GetMoleculeCode(); //Upon completion of a molecule, check to see if it matches an expected MoleculeCode
    //            //}


    //        }
    //        //}
    //    }


    //    public Vector2 GetJoint2Offset()
    //    {
    //        if (BondingPartner.transform.position.y > gameObject.transform.position.y + 1.5f)
    //        {
    //            return new Vector2(0, -1.73f);
    //        }

    //        else if (BondingPartner.transform.position.y < gameObject.transform.position.y - 1.5f)
    //        {
    //            return new Vector2(0, 1.73f);
    //        }

    //        else if (BondingPartner.transform.position.x < gameObject.transform.position.x - 1.5f)
    //        {
    //            return new Vector2(1.73f, 0);
    //        }

    //        else
    //        {
    //            return new Vector2(-1.73f, 0);
    //        }
    //    }




    //    // Update is called once per frame
    //    void FixedUpdate()
    //    {
    //        //totalValleysRem = 0;  //reset the open bonding slot count so that the next collision starts at zero       

    //        //if (UnbondingScript2.DontBondAgain > 0)   //this variable is set to 20 when unbonding event occurs
    //        //{
    //        //    UnbondingScript2.DontBondAgain--;  //this is the delay "timer" so that unbonding and bonding don't occur simultaneously
    //        //    print(UnbondingScript2.DontBondAgain);
    //        //}

    //    }
}
