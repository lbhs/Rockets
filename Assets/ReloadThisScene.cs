using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadThisScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReLoadThisScene()
    {
        //Time.timeScale = 0f;
        AtomInventory.NumberOfMoleculesInstantiated = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    

}
