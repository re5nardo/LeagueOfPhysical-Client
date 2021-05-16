
namespace DebugCommandHandler
{
    public class SpeedFactorHandler
    {
        public static void Handle(float value1, float value2)
        {
            DebugCommandPubSubService.Publish("SpeedFactor", new object[] { value1, value2 });
        }
    }
}
