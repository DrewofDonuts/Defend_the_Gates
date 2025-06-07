using System;
using System.Collections.Generic;
using PixelCrushers;
using Sirenix.Utilities;
using UnityEngine;

namespace Etheral
{
    public static class EtheralMessageSystem
    {
        public static event Action<string> OnCondition;
        public static event Action<MusicActions, string, string> OnMusicAction;

        public static Dictionary<object, string> keys = new();

        // public static void SendMessage(object sender, string condition)
        // {
        //     OnCondition?.Invoke(condition);
        // }

        public static void SendKey(object sender, string keyToSend)
        {
            if (!keyToSend.IsNullOrWhitespace())
                OnCondition?.Invoke(keyToSend);
            
            Debug.Log($"Sent key {keyToSend} from {sender}");
        }

        public static void SendQuestMachineMessage(object sender, string message, string parameter)
        {
            if (!message.IsNullOrWhitespace() && !parameter.IsNullOrWhitespace())
            {
                PixelCrushers.MessageSystem.SendMessage(sender, message, parameter);
                Debug.Log($"Sent message {message} with parameter {parameter}");
            }
        }


        public static void CheckForDuplicatesAndRegister(object sender, string condition)
        {
            if (keys.ContainsKey(condition))
            {
                Debug.LogError("Duplicate key detected. Please use a unique key.");
                return;
            }

            keys.Add(sender, condition);
        }

        public static void SendMusicAction(MusicActions action, string songName, string trackName)
        {
            OnMusicAction?.Invoke(action, songName, trackName);
        }
    }
}