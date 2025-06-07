using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral
{
    public class AudioEffect : BaseEffect
    {
        public AudioClip audioClip;
        public float delayBeforeAudio = .1f;
        public float pitchMin = 1f;
        public float pitchMax = 1f;
        public bool isLooping;

        float audioTimer;
        bool isAudioPlayed;


        public override void Initialize(SpellObject _spellObject, ICastSpell iCastSpell)
        {
            base.Initialize(_spellObject, iCastSpell);
            _spellObject.StartCoroutine(PlayAfterDelay());
        }

        void PlayAudio()
        {
            if (spellObject.enabled == false) return;

            if (audioClip != null)
            {
                spellObject.AudioSource.loop = isLooping;
                AudioProcessor.PlaySingleOneShot(spellObject.AudioSource, audioClip, AudioType.spell, pitchMin,
                    pitchMax);
            }
        }

        IEnumerator PlayAfterDelay()
        {
            yield return new WaitForSeconds(delayBeforeAudio);
            PlayAudio();
        }


        // public override void Tick(float deltaTime)
        // {
        //     if (!isAudioPlayed)
        //     {
        //         audioTimer += deltaTime;
        //         if (audioTimer >= delayBeforeAudio)
        //         {
        //             PlayAudio();
        //             isAudioPlayed = true;
        //         }
        //     }
        // }
    }
}