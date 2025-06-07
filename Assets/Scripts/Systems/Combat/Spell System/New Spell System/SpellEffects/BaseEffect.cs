using System;
using Interfaces;
using UnityEngine;


namespace Etheral
{
    [Serializable]
    public abstract class BaseEffect 
    {
        protected SpellObject spellObject;
        protected Transform caster;
        protected Transform target;
        protected Affiliation affiliation;
        public bool disable;

        public virtual void Initialize(SpellObject _spellObject, ICastSpell iCastSpell)
        {
            spellObject = _spellObject;
            caster = iCastSpell.Caster;
            affiliation = iCastSpell.Affiliation;

            if (iCastSpell.Target != null)
                target = iCastSpell.Target;
        }

        public virtual void Tick(float deltaTime) { }
        public virtual void ApplyOnTriggerEnter(Collider other) { }
        public virtual void ApplyOnCollision(Collision collision) { }
        public virtual void ApplyOnStay(Collider other) { }
        public virtual void ApplyOnExit(Collider collision) { }


        public virtual bool DoNotHitSelf(Collider other)
        {
            if (caster == null) return false;
            if (other.transform.root == caster.transform.root) return true;
            return false;
        }

        public virtual bool IsAffiliationSameAsCaster(Affiliation _affiliation)
        {
            if (affiliation == _affiliation) return true;

            return false;
        }
    }
}