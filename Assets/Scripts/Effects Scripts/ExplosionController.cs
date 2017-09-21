using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controls what happens when an explosion particle system is enabled after an enemy dies.

public class ExplosionController : MonoBehaviour {

    //declare reference to particle system.
    ParticleSystem thisExplosion;

    //Get reference to particle system.
    void Awake()
    {
        thisExplosion = gameObject.GetComponent<ParticleSystem>();
    }

    //Start the explosion when the explosion is enabled.
    void OnEnable()
    {
        StartCoroutine(explode());
    }

    //Play the explosion and disable this object when it is finished to be used again later.
    IEnumerator explode()
    {
        thisExplosion.Play();
        yield return new WaitUntil(() => thisExplosion.isPlaying == false);
        gameObject.SetActive(false);
    }
}
