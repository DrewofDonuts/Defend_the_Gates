using System;
using Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral
{
    [DisallowMultipleComponent]
    public class SpellHandler : MonoBehaviour, ICastSpell
    {
        [field: SerializeField] public SpellObject ActiveSpell { get; set; }

        [field: Header("Instantiation Point")]
        [field: SerializeField] public Transform CastPoint { get; private set; }
        [SerializeField] Transform omniDirectionalCastPoint;


        public Transform Target { get; set; }
        public Transform Caster { get; set; }

        public void OnSucessfulCast()
        {
            throw new NotImplementedException();
        }

        public Affiliation Affiliation { get; set; }
        public void SetAffiliation(Affiliation _affiliation) => Affiliation = _affiliation;

        public void EndActiveSpell()
        {
            Destroy(ActiveSpell.gameObject);
        }


        public void CastSpell(CharacterAction characterAction, StateMachine stateMachine)
        {
            if (stateMachine.Target != null)
                Target = stateMachine.Target;
            
            Caster = stateMachine.transform;

            if (characterAction.Spell.CastDirection is CastDirection.FourDirectional or CastDirection.EightDirectional)
                CastOmniDirectionalSpell(characterAction);
            else
                CastSingleSpell(characterAction);
        }

        void CastSingleSpell(CharacterAction characterAction)
        {
            var currentObject = characterAction.Spell;
            if (currentObject == null)
            {
                Debug.LogError("No spell object found");
            }

            var currentSpellObject = currentObject.spellObject;

            var currentCastPoint = CheckAndSetCastPoint(characterAction.Spell);

            var spellObject = Instantiate(currentSpellObject, currentCastPoint.position, currentCastPoint.rotation);
            
            if (characterAction.Spell.activeOnHold)
                ActiveSpell  = spellObject;
            
            
            spellObject.InitializeSpellObject(this);

            if (characterAction.Spell.isChildOfCaster)
            {
                spellObject.transform.parent = currentCastPoint;
                spellObject.transform.rotation = currentCastPoint.rotation;

                spellObject.transform.localPosition += characterAction.Spell.Offset;
            }
        }

        public void CastOmniDirectionalSpell(CharacterAction characterAction)
        {
            float[] fourDirectional = { 0f, 90f, 180f, 270f };
            float[] eightDirectional = { 0, 45, 90, 135, 180, 225, 270, 315 };

            float[] angles = characterAction.Spell.CastDirection == CastDirection.FourDirectional
                ? fourDirectional
                : eightDirectional;

            foreach (float angle in angles)
            {
                Quaternion rotation = Quaternion.Euler(0, angle, 0);
                Vector3 rotatedOffset = rotation * characterAction.Spell.offset;

                var spellObject = Instantiate(characterAction.Spell.spellObject,
                    omniDirectionalCastPoint.position + rotatedOffset,
                    omniDirectionalCastPoint.rotation * rotation);

                spellObject.InitializeSpellObject(this);

                if (characterAction.Spell.isChildOfCaster)
                {
                    spellObject.transform.parent = omniDirectionalCastPoint;
                }
            }
        }

        Transform CheckAndSetCastPoint(Spell spell)
        {
            return spell.InstantiateAtCastPoint ? CastPoint : transform;
        }
    }
}