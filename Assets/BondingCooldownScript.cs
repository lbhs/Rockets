using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BondingCooldownScript : MonoBehaviour
{
    public int BondingCooldownTimer;
    public bool PolyvalentBondInitiated;


    // Start is called before the first frame update
    void Start()
    {
        PolyvalentBondInitiated = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (BondingCooldownTimer > 0)
        {
            BondingCooldownTimer--;
        }
    }
}
