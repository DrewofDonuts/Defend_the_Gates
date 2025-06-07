using System;
using UnityEngine;

namespace Etheral
{
    public class SceneObjectiveHandler : MonoBehaviour
    {
        public Objective[] Objectives;


        public void AddToObjective(EventKey key, int amount)
        {
            foreach (var objective in Objectives)
            {
                if (objective.ObjectiveKey == key)
                {
                    objective.AddToCounter(amount);
                }
            }
        }

        public bool CheckIfObjectiveComplete(EventKey key)
        {
            foreach (var objective in Objectives)
            {
                if (objective.ObjectiveKey == key)
                {
                    return objective.CheckIfComplete();
                }
            }
            return false;
        }
    }


    [Serializable]
    public class Objective
    {
        public string ObjectiveName;
        public EventKey ObjectiveKey;
        public int TotalNeeded;
        public int CurrentCounter;
        public bool IsComplete;

        public void AddToCounter(int amount)
        {
            CurrentCounter += amount;
            if (CurrentCounter >= TotalNeeded)
            {
                IsComplete = true;
            }
        }

        public bool CheckIfComplete()
        {
            return IsComplete;
        }
    }
}