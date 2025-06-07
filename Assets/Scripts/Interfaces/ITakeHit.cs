using System;
using Interfaces;

namespace Etheral
{
    public interface ITakeHit : IAffiliate
    {
        public event Action OnDie;
        bool isHooked { get; set; }
        public void TakeHit(IDamage damage, float angle = default);
        public void TakeDotDamage(float damage);
    }
}