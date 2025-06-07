using System.Collections.Generic;
using UnityEngine;

namespace Etheral
{
    public class ExecutionProcessor
    {
        StateMachine stateMachine;
        CharacterAction characterAction;
        EnemyStateMachine enemyStateMachine;
        CharacterAudio characterAudio;

        /// List of VFX to be played
        int enableRightWeaponIndex;

        //List of the Audio to be played
        int timesBeforeAudioIndex;

        List<bool> alreadyPlayedSounds = new();
        List<bool> alreadyPlayedVFX = new();

        public ExecutionProcessor(StateMachine _stateMachine, CharacterAction _characterAction,
            EnemyStateMachine _enemyStateMachine)
        {
            SetupExecutionProcessor(_stateMachine, _characterAction, _enemyStateMachine);
        }

        public void SetupExecutionProcessor(StateMachine _stateMachine, CharacterAction _characterAction,
            EnemyStateMachine _enemyStateMachine)
        {
            stateMachine = _stateMachine;
            characterAction = _characterAction;
            enemyStateMachine = _enemyStateMachine;
            characterAudio = stateMachine.CharacterAudio;

            if (characterAction.EnableRightWeapon.Length > 0)
                InitializeVFXList();

            if (characterAction.TimesBeforeAudio.Count > 0)
                InitializeAudioList();
        }

        void InitializeAudioList()
        {
            for (int i = 0; i < characterAction.TimesBeforeAudio.Count; i++)
            {
                alreadyPlayedSounds.Add(false);
            }
        }

        void InitializeVFXList()
        {
            for (int i = 0; i < characterAction.EnableRightWeapon.Length; i++)
            {
                alreadyPlayedVFX.Add(false);
            }
        }

        public void EnablingAudio(float normalizedValue)
        {
            if (characterAction.TimesBeforeAudio.Count > 0)
            {
                for (int i = 0; i < characterAction.TimesBeforeAudio.Count; i++)
                {
                    if (normalizedValue >= characterAction.TimesBeforeAudio[i] && !alreadyPlayedSounds[i])
                    {
                        stateMachine.CharacterAudio.PlayOneShot(characterAudio.WeaponSource,
                            characterAction.AudioClips[i]);
                        alreadyPlayedSounds[i] = true;
                    }
                }
            }
        }


        public void EnablingVFX(float normalizedValue)
        {
            if (characterAction.EnableRightWeapon.Length > 0)
            {
                for (int i = 0; i < characterAction.EnableRightWeapon.Length; i++)
                {
                    if (normalizedValue >= characterAction.EnableRightWeapon[i] && !alreadyPlayedVFX[i])
                    {
                        alreadyPlayedVFX[i] = true;
                        enemyStateMachine.Health.EnableBlood();
                    }
                }
            }
        }
    }
}