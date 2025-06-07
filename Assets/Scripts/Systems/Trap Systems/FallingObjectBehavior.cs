using System;
using RayFire;
using UnityEngine;

namespace Etheral
{
    
public class FallingObjectBehavior : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] LayerMask collisionLayer;
    
    [Header("References")]
    [SerializeField] AudioClip fallingSound;
    [SerializeField] AudioClip impactSound;
    [SerializeField] AudioSource audioSource;
    [SerializeField] RayfireRigid rigid;
    [SerializeField] PlayAudio playAudio;
    
    
    
    void OnEnable()
    {
        if (rigid != null)
        {

            // rigid.fading.fadeType = RayFire.FadeType.SimExclude;
            // rigid.fading.fadeTime = 1f;
            // rigid.fading.lifeTime = 3f;
            
            rigid.Initialize();
        }
        
        if (audioSource != null)
        {
            // audioSource.clip = fallingSound;
            // audioSource.Play();
        }
    }
    

    void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & collisionLayer) != 0)
        {
            Debug.Log("Hit something");
            OnCollision();
            EventBusPlayerController.FeedbackIgnoringDistanceFromPlayer(gameObject.name, FeedbackType.Heavy);
            rigid.Demolish();

        }
    }


    void OnCollision()
    {
        Debug.Log("Should play impact sound");
        var audioStuff = Instantiate(playAudio, transform.position, Quaternion.identity);
        audioStuff.SetClipAndSource(impactSound);
        audioStuff.PlayAudioClip(true); 
    }
}
}
