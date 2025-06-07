using Etheral;

namespace Interfaces
{
/*
 * IGetTriggered interface is used to get the trigger that triggers IGetTriggered inheritors
 */
    public interface IGetTriggered
    {
        public ITrigger Trigger { get; set; }
    }
}