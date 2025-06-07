using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Etheral.Audio
{
    public class FloorAudio : MonoBehaviour
    {
        [field: SerializeField] public SurfaceType SurfaceType { get; private set; }
        [field: SerializeField] public bool IsOverrideSurface { get; private set; }
        [field: ShowIf("IsOverrideSurface")]
        [field: SerializeField] public SurfaceType ExitSurfaceType { get; private set; }

        AudioFXManager instance;

        IEnumerator Start()
        {
            yield return new WaitUntil(() => AudioFXManager.Instance != null);
            instance = AudioFXManager.Instance;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out CharacterAudio characterAudio))
            {
                if (characterAudio.CurrentSurface == SurfaceType) return;
                characterAudio.AudioSelector.DetectAndSetFootstep(SurfaceType);
            }
        }

        void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent(out CharacterAudio characterAudio))
            {
                if (characterAudio.CurrentSurface == SurfaceType) return;
                characterAudio.AudioSelector.DetectAndSetFootstep(SurfaceType);
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out CharacterAudio characterAudio))
            {
                Debug.Log(characterAudio);
                if (IsOverrideSurface)
                {
                    characterAudio.AudioSelector.DetectAndSetFootstep(ExitSurfaceType);
                }
                else
                    characterAudio.AudioSelector.DetectAndSetFootstep(instance.SceneSurfaceDefault);
            }
        }
    }
}