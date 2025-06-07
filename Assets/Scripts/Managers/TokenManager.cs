using System.Collections.Generic;
using UnityEngine;

namespace Etheral
{
    public class TokenManager : MonoBehaviour
    {
        static TokenManager _instance;
        public static TokenManager Instance => _instance;

        [field: SerializeField] public int MaxTokens { get; private set; } = 3;
        public Queue<AttackToken> tokenQueue;
        public Queue<AIStateMachine> requesterQueue;


        void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
                tokenQueue = new Queue<AttackToken>();
                requesterQueue = new Queue<AIStateMachine>();

                for (int i = 0; i < MaxTokens; i++)
                {
                    //Create tokens with a name
                    tokenQueue.Enqueue(new AttackToken("Token " + (i + 1)));
                }
            }
        }

        public bool RequestToken(AIStateMachine requester)
        {
            //If there are tokens available, assign one to the requester
            if (tokenQueue.Count > 0)
            {
                var token = tokenQueue.Dequeue();
                requester.AssignToken(token);

                return true;
            }

            //If there are no tokens available, add the requester to the queue
            if (!requesterQueue.Contains(requester))
                requesterQueue.Enqueue(requester);
            return false;
        }

        public bool AreThereZeroTokensLeft()
        {
            return tokenQueue.Count == 0;
        }


        public void ReturnToken(AttackToken token)
        {
            // Debug.Log($"Returning Token {token.TokenName}");

            // If there are requesters waiting, assign the token to the next requester
            if (requesterQueue.Count > 0)
            {
                var nextRequester = requesterQueue.Dequeue();

                // Debug.Log($"Returning Token {token.TokenName} to {nextRequester.transform.name}");
                nextRequester.AssignToken(token);
            }
            else
            {
                tokenQueue.Enqueue(token);
            }
        }

        public void DeQueueRequester(EnemyStateMachine requester)
        {
            if (requesterQueue.Contains(requester))
                requesterQueue.Dequeue();
        }
    }
}