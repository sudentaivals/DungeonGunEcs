using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class EcsEventBus
{
    private static readonly IDictionary<GameplayEventType, UnityEvent<int, EventArgs>> Events = new Dictionary<GameplayEventType, UnityEvent<int, EventArgs>>();

    public static void Subscribe(GameplayEventType eventType, UnityAction<int, EventArgs> listener)
    {
        UnityEvent<int, EventArgs> thisEvent;
        if (Events.TryGetValue(eventType, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent<int, EventArgs>();
            thisEvent.AddListener(listener);
            Events.Add(eventType, thisEvent);
        }
    }

    public static void Unsubscribe(GameplayEventType eventType, UnityAction<int, EventArgs> listener)
    {
        UnityEvent<int, EventArgs> thisEvent;
        if (Events.TryGetValue(eventType, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void Publish(GameplayEventType eventType, int sender, EventArgs args)
    {
        UnityEvent<int, EventArgs> thisEvent;
        if (Events.TryGetValue(eventType, out thisEvent))
        {
            thisEvent.Invoke(sender, args);
        }
    }
}
