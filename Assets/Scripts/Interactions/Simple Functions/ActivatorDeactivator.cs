using UnityEngine;

namespace Etheral
{
    public class ActivatorDeactivator : MessengerClass
    {
        [SerializeField] MonoBehaviour[] components;
        [SerializeField] GameObject[] gameObjects;

        protected override void HandleReceivingKey()
        {
            EnableOrDisableGameObject();
        }

        void EnableOrDisableGameObject()
        {
            if (components.Length > 0)
            {
                foreach (var component in components)
                {
                    if (component.enabled)
                    {
                        component.enabled = false;
                    }
                    else
                    {
                        component.enabled = true;
                    }
                }
            }

            if (gameObjects.Length > 0)
            {
                foreach (var gameObjectItem in gameObjects)
                {
                    if (gameObjectItem.activeSelf)
                    {
                        gameObjectItem.SetActive(false);
                    }
                    else
                    {
                        gameObjectItem.SetActive(true);
                    }
                }
            }
        }
    }
}