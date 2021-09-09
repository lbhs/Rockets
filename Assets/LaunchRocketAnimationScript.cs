using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchRocketAnimationScript : MonoBehaviour
{
    public Animator BadRocketAnimator;
    public Animator BetterRocketAnimator;
    public Animator BestRocketAnimator;

    public Animator RocketAnimator;

    public AudioSource LaunchSound;
    public AudioSource CrowdResponseBad;
    public AudioSource CrowdResponseBetter;
    public AudioSource CrowdResponseBest;
    public AudioSource GroundContact;


    public void LaunchRocketAnimation()
    {
        RocketAnimator = GameObject.FindGameObjectWithTag("RocketForFlight").GetComponent<Animator>();

        RocketAnimator.SetTrigger("LaunchThisRocket");

        LaunchSound.Play();

        StartCoroutine(DelayedSoundEffects());
    }


    private IEnumerator DelayedSoundEffects()  //this is a co-routine, can run in parallel with other scripts/functions
    {
        yield return new WaitForSeconds(3);

        if (DisplayCanvasScript.NetEnergyProfit > 8)
        {
            CrowdResponseBest.Play();
        }
        else if (DisplayCanvasScript.NetEnergyProfit > 5)
        {
            CrowdResponseBetter.Play();
        }
        else
        {
            CrowdResponseBad.Play();
        }

        yield break;
    }


}
