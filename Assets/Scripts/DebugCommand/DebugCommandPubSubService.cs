using System.Collections.Generic;
using System;
using GameFramework;

public class DebugCommandPubSubService
{
    private static SimplePubSubService<string, object[]> simplePubSubService = new SimplePubSubService<string, object[]>();

    public static void Publish(string key, object[] value)
    {
        simplePubSubService.Publish(key, value);
    }

    public static void AddSubscriber(string key, Action<object[]> subscriber)
    {
        simplePubSubService.AddSubscriber(key, subscriber);
    }

    public static void RemoveSubscriber(string key, Action<object[]> subscriber)
    {
        simplePubSubService.RemoveSubscriber(key, subscriber);
    }
}
