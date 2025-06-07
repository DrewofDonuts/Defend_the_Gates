using UnityEngine;

namespace Etheral
{
    //Used to send keys to the MessageHandler from UnityEvents, including Dialogue System
    public class KeySenderFromUnityEvents : MonoBehaviour
    {
        public void SendKey(string key)
        {
            EtheralMessageSystem.SendKey(this, key);
        }
    }
}