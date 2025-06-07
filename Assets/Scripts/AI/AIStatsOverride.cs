using System;
using UnityEngine;


[Obsolete("Use StatHandler instead")]
public class AIStatsOverride : MonoBehaviour
{
    
    [SerializeField] bool overrideDetectionRange;
    [SerializeField] bool overrideAttackRange;
    [SerializeField] bool overrideRunSpeed;
    [SerializeField] bool overrideWalkSpeed;
    
    [SerializeField] float detectionRangeOverride;
    [SerializeField] float attackRangeOverride;
    [SerializeField] float runSpeedOverride;
    [SerializeField] float walkSpeedOverride;
    
    
    public bool OverrideDetectionRange() => overrideDetectionRange;
    public bool OverrideAttackRange() => overrideAttackRange;
    public bool OverrideRunSpeed() => overrideRunSpeed;
    public bool OverrideWalkSpeed() => overrideWalkSpeed;
    
    public float GetDetectionRange() => detectionRangeOverride;
    public float GetAttackRange() => attackRangeOverride;
    public float GetRunSpeed() => runSpeedOverride;
    public float GetWalkSpeed() => walkSpeedOverride;
    
    
}
