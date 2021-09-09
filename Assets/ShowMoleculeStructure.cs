using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowMoleculeStructure : MonoBehaviour  //this script is attached to MoleculeCheckBoxButtons.  The checking of codes is done in the AtomInventory script attached to the MoleculeListKeeper Game object
{
    public bool MoleculeStructureIsShown;
    public Sprite ThisMoleculeStructure;
    public Sprite Transparent;
    public GameObject TargetMoleculeDisplay;


    // Start is called before the first frame update
    void Start()
    {
        MoleculeStructureIsShown = false;
        TargetMoleculeDisplay.GetComponent<Image>().sprite = Transparent;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowTheMolecularStructure()
    {

        if (MoleculeStructureIsShown == false)
        {
            TargetMoleculeDisplay.GetComponent<Image>().sprite = ThisMoleculeStructure;
            MoleculeStructureIsShown = true;
        }

        else
        {
            TargetMoleculeDisplay.GetComponent<Image>().sprite = Transparent;
            MoleculeStructureIsShown = false;
        }
        
        
    }

    public void HideMolecularStructure()
    {
        TargetMoleculeDisplay.GetComponent<Image>().sprite = Transparent;
        MoleculeStructureIsShown = false;
    }

}
