using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AtomInventory : MonoBehaviour
{
    public static List<GameObject>[] MoleculeList;  //This is an array of lists!  Each list contains the individual atoms present in the molecule (or soon to be molecule)
    public int[] bonusPts = new int[8];  //points for completing molecules of different sizes
    private int i;
    private int j;
    public static List<GameObject> ListOfAllProductAtoms = new List<GameObject>();  //This list will be used to detect unbonded atoms in the "products"
    public List<GameObject> ListOfUnbondedProductAtoms = new List<GameObject>();  //These will be pointed out to user as a problem to be corrected
    public TMP_Text AnswerResponseTextBox;
    public AudioSource BadSound;
    public AudioSource GoodSound;
    public int[] MoleculeCode = new int[13];
    public List<int> ProductMoleculeCodes;
    public List<bool> ProductMoleculeCheckList;
    public List<string> ProductMoleculeNames;
    public List<Button> ProductCheckButton;
    public int CheckSumForProductMolecules;
    public AudioSource VictorySong;
    public bool UsingMoleculeCodes;
    public static int NumberOfMoleculesInstantiated;
    

    //This script is attached to the MoleculeListKeeper GameObject 



    void Awake()   //needed to do this on AWAKE because START happened too late--UnchildTheAtoms needs this list to be active prior to Start!!
    {

        MoleculeList = new List<GameObject>[13];  //limits player to 12 total molecules bc index starts at 1 (seems plenty)       
        //AnswerResponseTextBox.text = null;
    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))  //this will print the molecule lists!
        {
            GetMoleculeCode();                                   
           
        }

       

    }

    public void DeleteAllMolecules()
    {
        for (i = 1; i < 12; i++)
        {
            print("Molecule " + i);

            if (MoleculeList[i] != null)  //attempts to access an empty (null) molecule list throws an error message
            {
                foreach (GameObject atom in MoleculeList[i]) //GameObject.Find("MoleculeListKeeper").GetComponent<//AtomInventory>().MoleculeList[Index])
                {
                    Destroy(atom);
                }
            }

        }
    }


    public void DetectUnbondedAtoms()
    {
        ListOfUnbondedProductAtoms = new List<GameObject>();
        foreach (GameObject atom in ListOfAllProductAtoms)
        {
            if(atom.GetComponent<BondMaker>().bonded == false)
            {
                ListOfUnbondedProductAtoms.Add(atom);
            }
        }
        foreach (GameObject UnbondedAtom in ListOfUnbondedProductAtoms)
        {
            print(UnbondedAtom + "is not bonded!");
        }

        if(ListOfUnbondedProductAtoms.Count == 0)
        {
            AnswerResponseTextBox.text = "GOOD WORK! You have no unbonded atoms!!";
            AnswerResponseTextBox.color = Color.green;
            GoodSound.Play();
        }
        else
        {
            AnswerResponseTextBox.text = "You have " + ListOfUnbondedProductAtoms.Count + " unbonded atoms.";
            AnswerResponseTextBox.color = Color.red;
            BadSound.Play();
        }
    }

    public void CheckSumForProductAtoms()
    {

    }

    public void DeleteAllProductAtoms()
    {
        foreach (GameObject atom in ListOfAllProductAtoms)
        {
            Destroy(atom);
        }

        ListOfAllProductAtoms = new List<GameObject>();
        MoleculeList = new List<GameObject>[13];

    }

    public void GetMoleculeCode()    //this function calculates the 5-digit "code" for a newly completed molecule.  Called from the BondMaker script  5 digits = H-C-O-Cl-N
    {
        if (UsingMoleculeCodes)
        {
            MoleculeCode = new int[13];
            for (i = 1; i < 13; i++)
            {
                if (MoleculeList[i] != null)  //attempts to access an empty (null) molecule list throws an error message
                {
                    print("Molecule " + i);

                    foreach (GameObject atom in MoleculeList[i]) //GameObject.Find("MoleculeListKeeper").GetComponent<//AtomInventory>().MoleculeList[Index])
                    {
                        //print(atom.name);
                        if (atom.tag == "Hydrogen")
                        {
                            MoleculeCode[i] += 10000;
                        }
                        if (atom.tag == "Carbon")
                        {
                            MoleculeCode[i] += 1000;
                        }
                        if (atom.tag == "Oxygen")
                        {
                            MoleculeCode[i] += 100;
                        }
                        if (atom.tag == "Chlorine")
                        {
                            MoleculeCode[i] += 10;
                        }
                        if (atom.tag == "Nitrogen")
                        {
                            MoleculeCode[i] += 1;
                        }
                    }

                    print("Molecule Code " + i + " = " + MoleculeCode[i]);

                }

            }
            CheckMoleculeCodes();
        }
        

    }

    public void CheckMoleculeCodes()  //this compares the molecule codes from completed molecules to the molecule codes expected in the scene
    {
        print("Number of product molecules = " + ProductMoleculeCodes.Count);  //this is the number of product molecules that should be formed in the scene
        for (i = 0; i < ProductMoleculeCodes.Count; i++) //this checks each expected product molecule
        {
            ProductMoleculeCheckList[i] = false;  //Need to start with a blank slate, will turn true only if a molecule has been built that matches the pre-determined code
            ProductCheckButton[i].GetComponent<Image>().color = Color.white;  //this is a blank slate for the button colors
            //print("white button " + i);

            for (j=1; j<13; j++)  //this checks the list of 12 molecules that may have been created--in reality will be less that 12...
            {
                if(MoleculeCode[j] == ProductMoleculeCodes[i])   //MoleculeCode[j] is a molecule that has been constructed, ProductMoleculeCodes[i] is one of the expected products
                {
                    ProductMoleculeCheckList[i] = true;  //Checks off this molecule
                    print(ProductMoleculeNames[i] + "has been formed");
                    MoleculeCode[j] += 5;  //this "punches" this code so it can't be used again  This works fine, since codes are rechecked every time a new molecule is formed or when a bond is broken
                    ProductCheckButton[i].GetComponent<Image>().color = Color.green;
                    print("green button " + i);

                    if(TutorialSpeechBubbleScript.TutorialMessageNumber == 24 && MoleculeCode[j] == 20105)
                    {
                        GameObject.Find("TutorialSpeechBubble").GetComponent<TutorialSpeechBubbleScript>().SendTutorialMessage();
                    }

                    if(TutorialSpeechBubbleScript.TutorialMessageNumber == 29 && MoleculeCode[j] == 01205)
                    {
                        GameObject.Find("TutorialSpeechBubble").GetComponent<TutorialSpeechBubbleScript>().SendTutorialMessage();
                    }

                    break;
                    
                }

                
            }
        }

        CheckForVictory();  //this will check to see if all the Product molecules have been completed
    }


    public void CheckForVictory()
    {
        for (i = 0; i < ProductMoleculeCodes.Count; i++)
        {
            print("ProductMoleculeCheckList for Molecule" + i + "=" + ProductMoleculeCheckList[i]);  //prints true if molecule has been made, false if not yet made
            print(MoleculeCode[i + 1] + "= MoleculeCode" + (i + 1));
            if(ProductMoleculeCheckList[i] == false)
            {
                return;
            }

        }
        VictorySong.Play();  //if no "false" statement has been encountered, victory has been achieved!  MAYBE AN ERROR HERE BECAUSE USER MAY HAVE SOME EXTRA MOLECULES THAT DON'T MATCH ANY PRODUCT CODES. . .
    }
    

}
