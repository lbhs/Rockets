using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveThisArrow : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveTheArrow1()
    {
        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(300, -600);
        gameObject.transform.eulerAngles = new Vector3(0, 0, -35);
        //gameObject.GetComponent<RectTransform>().localScale = new Vector2(2.7f, 1f);
        gameObject.GetComponent<Image>().color = new Color32(255, 190, 6, 255);
        //GameObject.Find("ConversationDisplay").GetComponent<ConversationTextDisplayScript>().CallForEa();
    }

    public void MoveTheArrow2()
    {
        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(20, -510);
        gameObject.transform.eulerAngles = new Vector3(0, 0, 148);
        gameObject.GetComponent<RectTransform>().localScale = new Vector2(5.3f, 0.7f);
        GameObject.Find("ConversationDisplay").GetComponent<Text>().text = "Use the unbonding flame to break bonds so you can make new molecules!";
    }

    public void MoveTheArrow3()
    {
        Destroy(gameObject); //.GetComponent<RectTransform>().anchoredPosition = new Vector2(2000, -510);
        GameObject.Find("ConversationDisplay").GetComponent<Text>().text = "Try to make an HCl molecule!";
        GameObject.Find("ConversationDisplay").GetComponent<Text>().color = Color.white;
    }


}
