namespace Etheral
{
    public class TestEffect : BaseEffect
    {
        public float testValue = 0f;


        public override void Tick(float deltaTime)
        {
            testValue += deltaTime;
        }
    }
}