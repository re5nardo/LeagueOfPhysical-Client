
namespace DebugCommandHandler
{
    public class TransformDelayFactorHandler
    {
        public static void Handle(float value)
        {
            DebugCommandPubSubService.Publish("TransformDelayFactor", new object[] { value });
        }
    }
}
