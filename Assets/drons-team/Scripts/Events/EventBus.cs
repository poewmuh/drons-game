using System;
using System.Collections.Generic;
using UnityEngine;

namespace DronsTeam.Events
{
    public static class EventBus
    {
        private static Dictionary<Type, List<Delegate>> _eventSubscribers = new ();
        
        public static void Subscribe<T>(Action<T> listener) where T : struct
        {
            var eventType = typeof(T);
            
            if (!_eventSubscribers.ContainsKey(eventType))
            {
                _eventSubscribers[eventType] = new List<Delegate>();
            }
            
            if (!_eventSubscribers[eventType].Contains(listener))
            {
                _eventSubscribers[eventType].Add(listener);
            }
        }

        public static void Unsubscribe<T>(Action<T> listener) where T : struct
        {
            var eventType = typeof(T);

            if (!_eventSubscribers.ContainsKey(eventType)) return;
            _eventSubscribers[eventType].Remove(listener);
        }

        public static void Publish<T>(T eventData) where T : struct
        {
            var eventType = typeof(T);

            if (!_eventSubscribers.TryGetValue(eventType, out var eventSubscriber)) return;
            
            var subscribers = new List<Delegate>(eventSubscriber);
            foreach (Delegate subscriber in subscribers)
            {
                try
                {
                    (subscriber as Action<T>)?.Invoke(eventData);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"[EventBus] Error in {eventType.Name} subscriber: {ex.Message}\n{ex.StackTrace}");
                }
            }
        }

#if UNITY_EDITOR
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStaticData()
        {
            _eventSubscribers.Clear();
        }
#endif
    }
}
