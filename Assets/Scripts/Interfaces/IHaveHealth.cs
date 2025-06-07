using System;
using UnityEngine;

namespace Etheral
{
    public interface IHaveHealth : IAffiliate
    {
        public float MaxHealth { get; }
        public float CurrentHealth { get; set; }


        public void Heal(float healthToAdd);
    }
}