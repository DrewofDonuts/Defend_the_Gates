using System.Collections.Generic;
using System.Linq;
using PixelCrushers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral
{
    public class EventManager : MonoBehaviour
    {
        static EventManager _instance;
        public static EventManager Instance => _instance;

        public List<EventHandler> eventHandlers = new();
        public List<WaypointsHandler> waypointGroups = new();
        
        void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        public void Register<T>(T register)
        {
            if (typeof(T) == typeof(EventHandler))
            {
                eventHandlers.Add(register as EventHandler);
            }

            if (typeof(T) == typeof(WaypointsHandler))
            {
                waypointGroups.Add(register as WaypointsHandler);
            }
        }

        public void Unregister<T>(T unregister)
        {
            if (typeof(T) == typeof(EventHandler))
            {
                eventHandlers.Remove(unregister as EventHandler);
            }

            if (typeof(T) == typeof(WaypointsHandler))
            {
                waypointGroups.Remove(unregister as WaypointsHandler);
            }
        }


        public void TriggerDialogue(EventKey eventKey)
        {
            DialogueSceneManager.Instance.InitializeDialogueAsset(eventKey);
        }

        public void TriggerEvent(EventKey _eventKey)
        {
            if (_eventKey.IsDialogue)
                TriggerDialogue(_eventKey);
            else if (_eventKey.IsCinematic)
                CinematicManager.Instance.TriggerCinematicEvent(_eventKey);
            
            foreach (var _eventHandler in eventHandlers)
            {
                // foreach (var _event in _eventHandler.etheralEvents.Where(x => x.EventKey.Value == _eventKey.Value))
                foreach (var _event in _eventHandler.etheralEvents.Where(x =>
                             x.EventKey != null && x.EventKey.Value == _eventKey.Value))
                {
                    _eventHandler.TriggerEvent(_eventKey);
                }
            }
        }
        public WaypointsHandler GetWaypointGroup(StringObject waypointGroupKey)
        {
            return waypointGroups.FirstOrDefault(x => x.EventKey.Value == waypointGroupKey.Value);

            // return waypointGroups.First(x => x.EventKey.Value == waypointGroupKey.Value);
        }
    }
}