namespace Etheral
{
    public interface IMessage
    {
        public string KeyToSend { get; }
        public string KeyToReceive { get; }
    }
}