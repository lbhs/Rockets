using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnbondingAtomMoverScript : MonoBehaviour  //This script is attached to the JewelUI GameObject.  It is used to gradually move Rigidbodies during unbonding events.
{
    public Rigidbody2D AtomInMotionRB;
    public Vector2 Atom2TargetPosition;
    public int AtomInMotionCountdownTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    

    private void FixedUpdate()
    {
        if (AtomInMotionRB != null )
        {
            //print("AtomInMotionRB " + AtomInMotionRB);
            AtomInMotionRB.MovePosition(Atom2TargetPosition);
            //print(Atom2TargetPosition);
            
        }

       
        if(AtomInMotionCountdownTimer > 0 )
        {
            AtomInMotionCountdownTimer--;
            if(AtomInMotionCountdownTimer == 0)
            {
                AtomInMotionRB = null;
            }
                
        }

        if (UnbondingScript2.DontBondAgain > 0)   //this variable is set to 20 when unbonding event occurs (UnbondingScript2).  Is also set to 10 when Jewels are in Motion after a bonding event
            //BondMaker script checks to see if "DontBondAgain" has reached zero
        {
            UnbondingScript2.DontBondAgain--;  //this is the delay "timer" so that unbonding and bonding don't occur simultaneously
            //print("DontBondAgain = " +UnbondingScript2.DontBondAgain);
        }



    }
}
