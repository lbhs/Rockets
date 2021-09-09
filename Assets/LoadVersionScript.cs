using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadVersionScript : MonoBehaviour
{
    public TMP_Dropdown VersionSelect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChooseScene()
    {
        print(VersionSelect.value);
        SceneManager.LoadScene(VersionSelect.value);   //Scene (0) is choose your version, Scene(1) = tutorial,  Scene(2) is CH4 combustion, Scene(3) is C3H8 Combustion, Scene(4) = AP Methane Combustion, Scene (5)= AP Propane Combustion
        //For Chemical Reactions, Scene(1) = H2 + Cl2,  Scene(2) = H2 + O2,   Scene(3) = CO2 + H2
    }





}
