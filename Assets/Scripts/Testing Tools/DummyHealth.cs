using System;
using Etheral;
using Interfaces;
using UnityEngine;

public class DummyHealth : MonoBehaviour, ITakeHit, IHaveHealth
{
    public float maxHealth = 10000f;
    public float subtractHealthOnStart = 0f;
    public float currentHealth;
    public ObjectAudio objectAudio;
    public AudioSource audioSource;

    [Header("Stats")]
    public float knockDownDefense;
    public float knockBackDefense;

    void Start()
    {
        currentHealth = maxHealth - subtractHealthOnStart;
    }

    public Affiliation Affiliation { get; set; }
    public void SetAffiliation(Affiliation _affiliation) => Affiliation = _affiliation;

    public Transform Transform { get; set; }

    public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }

    public void Heal(float healthToAdd)
    {
        currentHealth += healthToAdd;
    }

   

    public event Action OnDie;
    public bool isHooked { get; set; }

    public void TakeHit(IDamage damage, float angle = default)
    {
        CheckIfKnockedBack(damage);
        CheckIfKnockedDown(damage);
        currentHealth -= damage.Damage;
        audioSource.PlayOneShot(objectAudio.HitAudio[UnityEngine.Random.Range(0, objectAudio.HitAudio.Length)]);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void TakeDotDamage(float damage)
    {
        throw new NotImplementedException();
    }

    void CheckIfKnockedBack(IDamage damage)
    {
        if (damage.KnockBackForce > knockBackDefense)
        {
            Debug.Log("Knocked back");
        }
    }

    void CheckIfKnockedDown(IDamage damage)
    {
        if (damage.KnockDownForce > knockDownDefense)
        {
            Debug.Log("Knocked down");
        }
    }


    void Die()
    {
        Debug.Log("I'm dead");
    }
}