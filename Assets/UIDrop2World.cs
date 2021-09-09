using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIDrop2World : MonoBehaviour, IDropHandler
{
    //THIS SCRIPT IS ATTACHED TO THE FLAME JEWEL (JOULE) UI OBJECT (used for unbonding)
    
    //  Variable Definitions
    private Vector3 prefabWorldPosition; // Position that the prefab spawns in.
    public GameObject PrefabToSpawn;
    


    public void OnDrop(PointerEventData eventData)
    {
        
        RectTransform panel = transform as RectTransform;

        //the point where the unbonding jewels should be spawned
        prefabWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        prefabWorldPosition.z = 0;
        Instantiate(PrefabToSpawn, prefabWorldPosition, Quaternion.identity);
        UnbondingScript2.WaitABit = 8;  //this makes the unbonding Joule remain on screen for 8 Updates (about 0.16 sec)

        //TUTORIAL INTERACTIONS
        if(TutorialSpeechBubbleScript.TutorialMessageNumber == 7  || TutorialSpeechBubbleScript.TutorialMessageNumber == 12)
        {
            GameObject.Find("TutorialSpeechBubble").GetComponent<TutorialSpeechBubbleScript>().SendTutorialMessage();  //advances the tutorial
        }



        

    }

}
