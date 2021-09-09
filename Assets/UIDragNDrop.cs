using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UIDragNDrop : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public bool returnToZero = false; //default value is false
    public GameObject PrefabToSpawn;
    public bool UseingMe;
	Vector3 prefabWorldPosition;
    public static int BondableAtomsTakenSoFar;
    public TMP_Text ConversationTextBox;
    
    
    //THIS SCRIPT APPLIES TO THE ATOMS (OR MOLECULES) DRAGGED INTO THE SCENE FROM THE TOP MENU.  Attached to the "Image" GameObjects that are part of RollPanelSingle or RollPanelDouble

    void Start()
	{
		GameObject.Find("UI").GetComponent<Animator>().SetBool("Exiting", false);
	}

    public void OnDrag(PointerEventData eventData)
    {
		//if(!GameObject.Find("UI").GetComponent<Animator>().GetBool("Exiting"))
		{
			transform.position = Input.mousePosition;
			UseingMe = true;
		}
    }

    public void OnEndDrag(PointerEventData eventData)
    {
		prefabWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		prefabWorldPosition.z = 0;
        print("end of drag");


        


        if (ableToSpawn()) //&& !GameObject.Find("UI").GetComponent<Animator>().GetBool("Exiting"))
		{
//            if(GameObject.Find("MoleculeLimitCounter") && AtomInventory.NumberOfMoleculesInstantiated < GameObject.Find("MoleculeLimitCounter").GetComponent<LimitNumberOfMoleculesInstantiated>().MaximumNumberOfMoleculesToInstantiate{ 
//              }
            bool CanRemovePiece = true;  //THIS USED TO SAY "FALSE"
            
            

            if (CanRemovePiece)  //this means that there is inventory of this piece available--only meaningful in the CrownJoules game
            {
                InstantiateRealAtom();   //see below     
                


                //if (Conversation_for_CH4_Cl2.messageStatus == 1)
                //{
                //    //GameObject.Find("ConversationDisplay").GetComponent<ConversationTextDisplayScript>().CallForEa();
                //}

                //if (Conversation_for_CH4_Cl2.messageStatus == 2)
                //{
                //    GameObject.Find("ConversationDisplay").GetComponent<ConversationTextDisplayScript>().BreakCl2Bond();
                //    Conversation_for_CH4_Cl2.messageStatus = 3;
                //}

                //if (Conversation_for_CH4_Cl2.messageStatus == 4)
                //{
                //    GameObject.Find("ConversationDisplay").GetComponent<ConversationTextDisplayScript>().BreakCl2Bond();
                //    Conversation_for_CH4_Cl2.messageStatus = 5;
                //}

                //TUTORIAL INSTRUCTIONS ARE OFTEN LINKED TO DRAGGING OUT AN ATOM OR MOLECULE
                if (TutorialSpeechBubbleScript.TutorialMessageNumber == 5)  //Message #5 is Drag out an oxygen molecule (message counter starts at 0)
                {
                    if (gameObject.tag == "OxygenUI") //&&transform.position is within the red circle)
                    {
                        
                        GameObject.Find("TutorialSpeechBubble").GetComponent<TutorialSpeechBubbleScript>().SendTutorialMessage();  //advances the tutorial
                        //InstantiateRealAtom();
                        

                    }

                    else  //if not an oxygen atom. . .
                    {
                        transform.localPosition = Vector3.zero;  //sends the UI image back to its starting place
                    }

                }

                

                GameObject.Find("UI").GetComponent<Animator>().SetTrigger("Exit");
				GameObject.Find("UI").GetComponent<Animator>().SetBool("Exiting", true);
			}
			else  //if cannot remove piece--this should never be active in this project:  only in CrownJoules can this be false
			{
				transform.localPosition = Vector3.zero;
                
            }
		}
		else
		{
			transform.localPosition = Vector3.zero;

        }
    }
	
	public bool ableToSpawn()
	{
        if (GameObject.Find("MoleculeLimitCounter") && AtomInventory.NumberOfMoleculesInstantiated >= GameObject.Find("MoleculeLimitCounter").GetComponent<LimitNumberOfMoleculesInstantiated>().MaximumNumberOfMoleculesToInstantiate)
        {   //this is valid only in the H2 + O2 rocket scene
            print("max number of molecules already in play");
            ConversationTextBox.GetComponent<ConversationTextDisplayScript>().ReachedMaximumNumberOfMolecules();
            return false;  
        }

        PointerEventData pointer = new PointerEventData(EventSystem.current);
		pointer.position = Input.mousePosition;
		List<RaycastResult> raycastResults = new List<RaycastResult>();
		EventSystem.current.RaycastAll(pointer, raycastResults);
		if(raycastResults.Count > 0)
		{
			foreach(var go in raycastResults)
			{
				if(go.gameObject.transform.parent.gameObject.name == "RollPannelSingle" || go.gameObject.transform.parent.gameObject.name == "RollPannelDouble")
				{
                    print("collided with roll panel");
                    return false;
                    
				}
			}
		}
		
		GameObject dummyObject = Instantiate(PrefabToSpawn, prefabWorldPosition, Quaternion.identity);
        //For Molecules based on an empty game object (Unchild the Atoms), NEED TO GIVE THE EMPTY GAMEOBJECT A RIGIDBODY (for line #142) and a COLLIDER so that the raycast hits the dummy Object

        int accuracy = 5; //1 is pixel perfect accuracy but causes stutter, 5 is a great performance but could allow minor overlap
		int range = Screen.height/2;
		
		for(int x = (int)Input.mousePosition.x - range; x < (int)Input.mousePosition.x + range; x+=accuracy)
		{
			for(int y = (int)Input.mousePosition.y - range; y < (int)Input.mousePosition.y + range; y+=accuracy)
			{                
				RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenPointToRay(new Vector3(x, y, 0)).origin, Vector2.zero);
				//Debug.DrawRay(Camera.main.ScreenPointToRay(new Vector3(x, y, 0)).origin, transform.TransformDirection(Vector3.forward) * 100, Color.green, 10f, false);
				if(hits.Length > 1)
				{
					foreach(var go in hits)
					{                        
                        if (go.rigidbody.gameObject == dummyObject)
						{
                            print("dummy object detected");
							foreach(var go2 in hits)
							{
                                print(dummyObject);
                                if (!go2.transform.IsChildOf(dummyObject.transform))  //for Molecules, the dummyObject will have child atoms that will count as raycast hits!  This line ignores those raycast hits
                                {

                                    if (go2.rigidbody.gameObject != dummyObject && (go2.rigidbody.gameObject.GetComponent<DragIt>() != null || go2.rigidbody.gameObject.tag == "Diatomic"))
                                    {
                                        print(go2.rigidbody.gameObject);
                                        Destroy(dummyObject);
                                        Debug.DrawRay(Camera.main.ScreenPointToRay(new Vector3(x, y, 0)).origin, transform.TransformDirection(Vector3.forward) * 100, Color.green, 10f, false);
                                        GameObject.Find("ConversationDisplay").GetComponent<ConversationTextDisplayScript>().noStack();
                                        return false;
                                    }
                                }
							}
						}
					}
				}
			}
		}
		Destroy(dummyObject);
		return true;
	}

    private void InstantiateRealAtom()
    {
        //prefabWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //prefabWorldPosition.z = 0;
        GameObject NewlyInstantiatedGameObject = Instantiate(PrefabToSpawn, prefabWorldPosition, Quaternion.identity);
        if (returnToZero == true)
        {
            transform.localPosition = Vector3.zero;
        }
        UseingMe = false;

        if (TutorialSpeechBubbleScript.TutorialMessageNumber == 5)  //Message #5 is Drag out an oxygen molecule (message counter starts at 0)  
        {
            NewlyInstantiatedGameObject.transform.position = new Vector2(2.26f, 2.1f);  //this line of code will place the O2 molecule where it belongs
        }
    }

}