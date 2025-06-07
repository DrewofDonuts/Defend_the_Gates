using System.Collections;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

//Hooks to a Character Object and will handle all event triggers for that character

namespace Etheral
{
    public class EventHandler : MonoBehaviour
    {
        //  [field: InlineButton("NewCharacterKey", "New")]
        // [field: SerializeField] public CharacterKey EntityKey { get; private set; }

        [FormerlySerializedAs("Events")] [SerializeField]
        public EtheralEvents[] etheralEvents;

        bool hasDuplicateKeys;
        bool hasEmptyTrigger;


        IEnumerator Start()
        {
            yield return new WaitUntil(() => EventManager.Instance);

            // if (EntityKey != null)
            EventManager.Instance.Register(this);
        }


        public void TriggerEvent(EventKey key)
        {
            if (etheralEvents == null)
            {
                Debug.LogError("EventCharacterTriggers is null!");
                return;
            }

            foreach (var eventToTrigger in etheralEvents.Where(x => x.EventKey == key))
            {
                eventToTrigger.TriggerEnterEvents();
            }
        }

        void OnDisable()
        {
            if (EventManager.Instance == null) return;

            // if (EntityKey != null)
            EventManager.Instance.Unregister(this);
        }


        #region Validation and Debugging
#if UNITY_EDITOR

        bool isCheckKeys;

        void OnValidate()
        {
            if (etheralEvents == null) return;
            foreach (var eventTrigger in etheralEvents)
            {
                //Disable if not want to actually change the name of the event
                eventTrigger.UpdateEventName();
            }

            CheckForDuplicateKeys();
        }

        public void NewCharacterKey()
        {
            // EntityKey = AssetCreator.NewCharacterKey();
        }

        public void CheckForDuplicateKeys()
        {
            if (etheralEvents.Any(x => x.EventKey == null))
            {
                hasEmptyTrigger = true;
                return;
            }

            hasEmptyTrigger = false;


            var duplicateKeys = etheralEvents.GroupBy(x => x.EventKey.Value)
                .Where(g => g.Count() > 1)
                .Select(y => y.Key)
                .ToList();

            hasDuplicateKeys = duplicateKeys.Count > 0;
        }

        [ShowIf("hasDuplicateKeys")]
        [Button("There are Duplicates!", ButtonSizes.Large), GUIColor(1f, .25f, .25f)]
        void DuplicateKeys()
        {
            etheralEvents = etheralEvents.GroupBy(x => x.EventKey.Value)
                .Select(group => group.First()).ToArray();

            OnValidate();
        }

        [ShowIf("hasEmptyTrigger")]
        [Button("There's an Empty Trigger!", ButtonSizes.Large), GUIColor(1f, .25f, .25f)]
        void EmptyTrigger()
        {
        }

#endif
        #endregion
    }
}