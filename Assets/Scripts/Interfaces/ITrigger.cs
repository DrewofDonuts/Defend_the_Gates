using System;

namespace Etheral
{
    public interface ITrigger
    {
        public event Action OnTrigger;
        public event Action OnReceive;
        public event Action OnReset;
        public string KeyToSend { get; }
        public string KeyToReceive { get; }
        public void ResetTrigger();
        public void SendKeyAndMessage();
    }
}