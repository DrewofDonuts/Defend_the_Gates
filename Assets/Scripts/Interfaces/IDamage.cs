using System;
using Etheral;
using Interfaces;
using UnityEngine;
using UnityEngine.Serialization;


public interface IDamage
{
    public Transform Transform { get; }
    public float Damage { get; }
    public float DotDamage { get; }
    public float KnockBackForce { get; }
    public float KnockDownForce { get; }
    public bool DoesImpact { get; }

    public bool IsShieldBreak { get; }
    public bool IsExecution { get; }
    public int AttackerID { get; }

    public Vector3 Direction { get; }
    public FeedbackType FeedbackType { get; }

    public AudioImpact AudioImpact { get; }

    public void IsBlocked(bool isBlocking);
    public IGetBlocked GetBlocked { get; }
}

[Serializable]
public class DamageData : IDamage
{
    public float damage = 10f;
    public float dotDamage;
    public float knockBackForce;
    public float knockDownForce;
    public bool isShieldBreak;
    public bool isExecution;
    public bool doesImpact = true;
    public FeedbackType feedbackType;
    public AudioImpact audioImpact;
    public int attackerID = -1; // Default to -1 if no attacker ID is set


    public float Damage => damage;
    public float DotDamage => dotDamage;
    public float KnockBackForce => knockBackForce;
    public float KnockDownForce => knockDownForce;
    public bool DoesImpact  => doesImpact;
    public bool IsShieldBreak => isShieldBreak;
    public bool IsExecution => isExecution;
    public int AttackerID => attackerID;
    public Vector3 Direction { get; set; }
    public FeedbackType FeedbackType => feedbackType;
    public AudioImpact AudioImpact => audioImpact;
    public IGetBlocked GetBlocked { get; private set; }
    

    public Transform Transform { get; set; }

    public void SetGetBlocked(IGetBlocked getBlocked)
    {
        GetBlocked = getBlocked;
    }

    public void IsBlocked(bool isBlocking)
    {
        if (GetBlocked == null) return;
        
        if (isBlocking)
            GetBlocked.HandleBlocked();
    }
}