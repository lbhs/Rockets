using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnchildTheAtoms : MonoBehaviour
{
    public List<GameObject> ChildAtoms = new List<GameObject>();   //need to assign these atoms in the Prefab Molecule
    private int i;
    private int Index;

    public float TotalPEInThisMolecule;
    public GameObject MCToken;
    private int j;      //used to count the number of atoms in the molecule
    private GameObject BadgeRecipient;
    private float BadgeRotation;


    //This script is attached to Molecule Prefabs, composed of many atoms that have the appropriate joints, valley counts, and PE counts.  
    //Might want to delete the molecule completion tokens. . .


    // Start is called before the first frame update  UNCHILD THE ATOMS MUST OCCUR ON START, AFTER MOLECULE LISTS ARE SET UP DURING "AWAKE" FUNCTION
    void Start()
    {
        print("start UnchildTheAtoms");
        AtomInventory.NumberOfMoleculesInstantiated++;
        

            for (i = 1; i < 13; i++)   //Slots 1-12 in the array are used to store Molecules (atoms in the molecule)
            {
                if (AtomInventory.MoleculeList[i] == null || AtomInventory.MoleculeList[i].Count == 0)
                {
                    Index = i;      //Index finds the lowest empty MoleculeList slot
                    print("lowest open slot = " + i);
                    break;          //to abort the loop after the first empty slot is found
                }
            }

            AtomInventory.MoleculeList[Index] = new List<GameObject>();

            foreach (GameObject Atom in ChildAtoms)
            {
                //print(Atom);
                
                BadgeRecipient = Atom;
                Atom.transform.parent = null;  //this unchilds each atom in turn
                AtomInventory.MoleculeList[Index].Add(Atom);  //This adds the atoms to a molecule list!  Also adds the MCToken. . .
                Atom.GetComponent<BondMaker>().MoleculeID = Index;  //assign MoleculeID to each atom in the Molecule  
                j++;  //this variable counts the number of atoms in the molecule

                TotalPEInThisMolecule += Atom.GetComponent<PotentialEnergy>().PE;
                AtomInventory.ListOfAllProductAtoms.Add(Atom);  //This is the list of all atoms in the "products".  Can use Checksum to determine if extra atoms were used?
                //Atom.GetComponent<BondMaker>().bonded = true;  //atoms came out unbonded because BondMaker script line 46 set bonded to FALSE!!!  
            }

            print("assigned MoleculeID " + Index);

            MCToken = GameObject.Find("MoleculeListKeeper").GetComponent<MoleculeCompletionPtArray>().MoleculeCompletionToken[Mathf.RoundToInt(TotalPEInThisMolecule)];
            AtomInventory.MoleculeList[Index].Add(MCToken);  //adds a MoleculeCompletionToken to the MoleculeList Array


        //HERE'S WHERE MCTOKENS GET ATTACHED TO MOLECULES
        print("Number of atoms in the molecule = " + j);
        if (j == 2)   ///applies to newly formed diatomic molecules (including HCl)
        {
            //BadgeRecipient = Atom;
        }

        else if (j > 2)  //the molecule has a carbon or oxygen center
        {
            foreach (GameObject atom in ChildAtoms)
            {
                if (atom.tag == "Oxygen")
                {
                    BadgeRecipient = atom;
                    print("BadgeRecipient =" + BadgeRecipient);
                }

                if (atom.tag == "Carbon" || atom.tag == "Carbon2xDB")  //carbon takes precedence over oxygen
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

        Destroy(gameObject);   //this destroys the empty GameObject used as the Prefab Molecule (all the actual atoms are children of the empty)

    }

    // Update is called once per frame
    void Update()
    {
       
    }
    
    //IEnumerator Wait()
    //{
    //    yield return new WaitForSecondsRealtime(5f);
    //    GetComponent<BondMaker>().enabled = true;
    //}
}

