using System;
using System.Collections.Generic;

public static class EventBus
{
    private static readonly Dictionary<Type, Delegate> eventHub = new Dictionary<Type, Delegate>();

    public static void Subcribe<T>(Action<T> listener) where T : struct
    { 
        Type eventType = typeof(T);

        if (!eventHub.ContainsKey(eventType))
        {
            eventHub[eventType] = null;
        }

        eventHub[eventType] = (Action<T>)eventHub[eventType] + listener;
    }

    public static void Unsubcribe<T>(Action<T> listener) where T : struct
    { 
        Type eventType = typeof(T);

        if (eventHub.ContainsKey(eventType)) 
        {
            eventHub[eventType] = (Action<T>)eventHub[eventType] - listener;

            if (eventHub[eventType] == null)
            {
                eventHub.Remove(eventType);
            }
        }
    }

    public static void Publish<T>(T eventData) where T : struct
    { 
        Type eventType = typeof(T);
        if (eventHub.ContainsKey(eventType)) 
        {
            Delegate d = eventHub[eventType];
            if (d != null)
            {
                (d as Action<T>)?.Invoke(eventData);
            }
        }
    }
}
