namespace Etheral
{
    public class AttackToken
    {
        public string TokenName { get; private set; }


        public AttackToken(string tokenName)
        {
            TokenName = tokenName;
        }
    }
}