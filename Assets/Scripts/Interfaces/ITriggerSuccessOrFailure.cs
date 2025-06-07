using System;

namespace Etheral
{
    public interface ITriggerSuccessOrFailure
    {
        public event Action OnTriggerSuccessEvent;
        public event Action OnTriggerFailedEvent;
    }
}