using System;
using UnityEngine;

public interface IAttack
{
    public event Action<int, int, int, Transform, LayerMask> AddedWeaponStats;
}
