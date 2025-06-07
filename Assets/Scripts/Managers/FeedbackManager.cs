using System.Collections;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Etheral
{
    public class FeedbackManager : MonoBehaviour
    {
        public static FeedbackManager Instance { get; private set; }

        [field: SerializeField] public MMF_Player LightFeedback { get; private set; }
        [field: SerializeField] public MMF_Player MediumFeedback { get; private set; }
        [field: SerializeField] public MMF_Player HeavyFeedback { get; private set; }
        [field: SerializeField] public MMF_Player ConstantRumble { get; private set; }

        PlayerStateMachine player;


        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        IEnumerator Start()
        {
            LightFeedback.Initialization();
            MediumFeedback.Initialization();
            HeavyFeedback.Initialization();

            if (ConstantRumble != null)
                ConstantRumble.Initialization();

            yield return new WaitUntil(() => CharacterManager.Instance.Player != null);
            player = CharacterManager.Instance.Player;
        }


        public void PlayFeedback(string feedbackName)
        {
            Debug.Log("Playing feedback: " + feedbackName);
        }
        
        public void PlayFeedbackBasedOnDistanceFromPlayer(string feedbackName, Vector3 position)
        {
            if (player == null)
            {
                Debug.LogWarning("Player is null");
                return;
            }

            var distance = Vector3.Distance(player.transform.position, position);
            Debug.Log("Distance from player: " + distance);
        }
    }
}