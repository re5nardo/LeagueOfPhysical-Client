
namespace DebugCommandHandler
{
    public class SpeedFactorHandler
    {
        public static void Handle(float value1, float value2)
        {
            AppMessageBroker.Publish(new DebugCommandMessage.SpeedFactor(value1, value2));
        }
    }
}
