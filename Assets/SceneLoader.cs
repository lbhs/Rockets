using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public Animator FadeAnimator;
    public float FadeTime;
    public void LoadScene(int SceneToLoad)
    {
        StartCoroutine(LoadSceneTimer(SceneToLoad));
        //Reset all the static variables!
        


        //SceneManager.LoadScene(2);
    }



    IEnumerator LoadSceneTimer(int SceneToLoad)
    {
        //FadeAnimator.SetTrigger("Start");   No Fade desired in Rocket Project
        yield return new WaitForSeconds(FadeTime);
        SceneManager.LoadScene(SceneToLoad);
    }
}
