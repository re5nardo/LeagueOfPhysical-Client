
namespace DebugCommandMessage
{
    public struct TransformDelayFactor
    {
        public float factor;

        public TransformDelayFactor(float factor)
        {
            this.factor = factor;
        }
    }

    public struct SpeedFactor
    {
        public float value1;
        public float value2;

        public SpeedFactor(float value1, float value2)
        {
            this.value1 = value1;
            this.value2 = value2;
        }
    }
}
