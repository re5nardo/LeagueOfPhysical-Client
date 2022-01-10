using GameFramework;

namespace DebugCommandHandler
{
    public class TransformDelayFactorHandler
    {
        public static void Handle(float value)
        {
            AppMessageBroker.Publish(new DebugCommandMessage.TransformDelayFactor(value));
        }
    }
}
