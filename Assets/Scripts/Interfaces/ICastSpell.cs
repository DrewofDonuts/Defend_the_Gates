using UnityEngine;

namespace Etheral
{
    public interface ICastSpell : IAffiliate
    {
        public Transform Target { get; }
        public Transform Caster { get; }
        public void OnSucessfulCast();
    }
}