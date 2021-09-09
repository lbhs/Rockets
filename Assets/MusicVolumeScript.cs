using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicVolumeScript : MonoBehaviour
{
    public AudioSource MusicForThisScene;
    public float VolumeLevel;
    public Slider VolumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        MusicForThisScene.GetComponent<AudioSource>().volume = VolumeSlider.value;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void VolumeChange()
    {
        print(VolumeSlider.value);
        MusicForThisScene.GetComponent<AudioSource>().volume = VolumeSlider.value; 
    }




}
