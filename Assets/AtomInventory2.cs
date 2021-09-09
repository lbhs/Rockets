using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomInventory2 : MonoBehaviour
{
    public static List<GameObject>[] MoleculeList;
    public int[] bonusPts = new int[8];  //points for completing molecules of different sizes
    private int i;
    private int j;

    //This script is attached to GameObject MoleculeListKeeper

    private void Awake()  //needed to do this on AWAKE because START happened too late--UnchildTheAtoms needs this list to be active prior to Start!!
    {
        print("awake AtomInventory");
        MoleculeList = new List<GameObject>[13];  //limits player to 12 total molecules bc index starts at 1 (seems plenty)
    }

    // Start is called before the first frame update
    void Start()
    {
        print("start AtomInventory");
        
        //IF USING MOLECULE PRE-FABS, ASSIGNMENT OF MOLECULE ID'S AND MoleculeList[x] is done in "UnchildTheAtoms" script!!

        //can't do "new List" because it erases the information already stored in the UnchildTheAtoms script!

        //MoleculeList = new List<GameObject>[13];  //limits player to 12 total molecules bc index starts at 1 (seems plenty)

        /*
        BondMaker[] AtomsWithBondMakerScript = FindObjectsOfType(typeof(BondMaker)) as BondMaker[];  
        //this finds the atoms already in play when the scene starts!  Only has meaning in scenes like Combustion or Photosynthesis
        print(AtomsWithBondMakerScript.Length);

        for (j = 0; j < 13; j++)
        {
            MoleculeList[j] = new List<GameObject>();   //this erases information recorded in "UnchildTheAtoms" script
        }
        

        //DON'T WANT TO USE THE FUNCTION BELOW BECAUSE WE NOW HAVE MOLECULE PRE-FABS THAT WORK ON THE "UnchildTheAtoms" script!
        for (i = 0; i < AtomsWithBondMakerScript.Length; i++) //each (GameObject atom in AtomsWithBondMakerScript)
        {
            //print("atom#" + i);
            GameObject Atom = AtomsWithBondMakerScript[i].gameObject;
            print(Atom);
            j = Atom.GetComponent<BondMaker>().MoleculeID;  //UnchildTheAtoms script will assign MoleculeID values to atoms in Prefab molecules(e.g. CH4 molecule)
            MoleculeList[j].Add(Atom);
            print(Atom + "added to MoleculeList " + j);
            
        }*/
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))  //this will print the molecule lists!
        {
                                   
            for (i = 1; i < 12; i++)
            {
                print("Molecule " +i);
                
                if(MoleculeList[i] != null)  //attempts to access an empty (null) molecule list throws an error message
                {
                    foreach (GameObject atom in MoleculeList[i]) //GameObject.Find("MoleculeListKeeper").GetComponent<//AtomInventory>().MoleculeList[Index])
                    {
                        print(atom.name);
                    }
                }
                
            }

            
        }

       

    }
}
