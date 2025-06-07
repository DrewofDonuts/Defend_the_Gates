using System;
using Etheral;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;


public class NonDestructible : TriggerSuccessOrFailureMonoBehavior, ITakeHit
{
    
    public Affiliation Affiliation { get; set; }
    public void SetAffiliation(Affiliation affiliation) => Affiliation = affiliation;

    float currentHealth;
    public float limitBeforeInvokeEvent = 50;
    public event Action OnDie;
    public bool isHooked { get; set; }
    public ObjectAudio objectAudio;
    public AudioSource audioSource;
    public UnityEvent Event;


    public void TakeHit(IDamage damage, float angle)
    {
        Debug.Log("TakeHit");
        currentHealth += damage.Damage;
        HandleHitAudio();

        if (currentHealth >= limitBeforeInvokeEvent)
        {
            OnTriggerEvent();
            currentHealth = 0;
            Event.Invoke();
        }
    }

    public void TakeDotDamage(float damage)
    {
        throw new NotImplementedException();
    }

    void HandleHitAudio()
    {
        if (objectAudio == null)
        {
            Debug.LogError("No ObjectAudio found on " + gameObject.name);
            return;
        }

        AudioProcessor.PlayRandomClips(audioSource, objectAudio.HitAudio, .85f, 1.15f);
    }

}