using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kJperMoleBondEnergyScript : MonoBehaviour
{
    public int[,] AdvancedBondEnergyMatrix;

    // Start is called before the first frame update
    void Start()
    {
        //H = 0, C = 1, O = 2, Cl = 3, C with double bond = 4, O with double bond = 5
        AdvancedBondEnergyMatrix = new int[6, 6];  //using bond energies from Zumdahl
        AdvancedBondEnergyMatrix[0, 0] = 432;
        AdvancedBondEnergyMatrix[0, 1] = 413;
        AdvancedBondEnergyMatrix[0, 2] = 467;
        AdvancedBondEnergyMatrix[0, 3] = 427;
        AdvancedBondEnergyMatrix[1, 0] = 413;
        AdvancedBondEnergyMatrix[1, 1] = 347;
        AdvancedBondEnergyMatrix[1, 2] = 358;
        AdvancedBondEnergyMatrix[1, 3] = 339;
        AdvancedBondEnergyMatrix[2, 0] = 467;
        AdvancedBondEnergyMatrix[2, 1] = 358;
        AdvancedBondEnergyMatrix[2, 2] = 146;
        AdvancedBondEnergyMatrix[2, 3] = 203;
        AdvancedBondEnergyMatrix[3, 0] = 427;
        AdvancedBondEnergyMatrix[3, 1] = 339;
        AdvancedBondEnergyMatrix[3, 2] = 203;
        AdvancedBondEnergyMatrix[3, 3] = 239;
        AdvancedBondEnergyMatrix[4, 4] = 614;   //   4 = Carbon double bond
        AdvancedBondEnergyMatrix[4, 5] = 799;  //   5 = oxygen double bond
        AdvancedBondEnergyMatrix[5, 4] = 799;
        AdvancedBondEnergyMatrix[5, 5] = 495;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
