using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteWorldToScreenCoordinates : MonoBehaviour
{
    public GameObject Molecule1;
    //private Transform target;
    Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("m"))  //this will get coordinates of Molecule1!
        {
            print("m");
            GetMoleculeScreenPosition(Molecule1);

        }
    }

    
    
    
    public Vector2 GetMoleculeScreenPosition(GameObject AtomToPlace)
    {
        //target = Molecule1.transform;
        Vector2 screenPos = cam.WorldToScreenPoint(AtomToPlace.transform.position);
        print("MoleculeToPlace is " + screenPos.x + " pixels from the left");
        print("MoleculeToPlace is " + screenPos.y + " pixels from the bottom");
        return screenPos;

    }
}

