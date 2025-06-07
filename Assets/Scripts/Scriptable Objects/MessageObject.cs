using UnityEngine;

namespace Etheral
{
    public abstract class MessageObject  : ScriptableObject, IMessage
    {
        public abstract string KeyToSend { get; }
         public abstract string KeyToReceive { get; }
    }
}