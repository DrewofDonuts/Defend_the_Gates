using System.Collections.Generic;
using UnityEngine;

namespace Etheral
{
    public static class CharacterActionConverter
    {
        public static CharacterAction DeepCopy(CharacterAction original)
        {
            if (original == null) return null;

            CharacterAction copy = new CharacterAction
            {
                Name = original.Name,
                Animation = original.Animation,
                IsAttack = original.IsAttack,
                IsAction = original.IsAction,
                EnableLeftWeapon =
                    original.EnableLeftWeapon != null ? (float[])original.EnableLeftWeapon.Clone() : null,
                DisableLeftWeapon = original.DisableLeftWeapon != null
                    ? (float[])original.DisableLeftWeapon.Clone()
                    : null,
                Damage = original.Damage,
                AdjacentDistance = original.AdjacentDistance,
                ActionRange = original.ActionRange,
                DotDamage = original.DotDamage,
                KnockBackForce = original.KnockBackForce,
                KnockDownForce = original.KnockDownForce,
                NextComboStateIndex = original.NextComboStateIndex,
                ComboAttackTime = original.ComboAttackTime,
                IsShieldBreak = original.IsShieldBreak,
                IsExecution = original.IsExecution,
                FeedbackType = original.FeedbackType,
                AudioImpact = original.AudioImpact,
                TimesBeforeSpells = original.TimesBeforeSpells != null
                    ? (float[])original.TimesBeforeSpells.Clone()
                    : null,
                Spell = original.Spell,
                TimeBeforeEffect = original.TimeBeforeEffect,
                Effect = original.Effect,
                TimeBeforeAudio = original.TimeBeforeAudio,
                TimesBeforeAudio = new List<float>(original.TimesBeforeAudio),
                Audio = original.Audio,
                AudioClips = new List<AudioClip>(original.AudioClips),
                TimesBeforeProjectile = original.TimesBeforeProjectile != null
                    ? (float[])original.TimesBeforeProjectile.Clone()
                    : null,
                AimTime = original.AimTime,
                TimeBeforeDrawAudio = original.TimeBeforeDrawAudio,
                CooldownPrefab = original.CooldownPrefab
            };

            return copy;
        }
    }
}