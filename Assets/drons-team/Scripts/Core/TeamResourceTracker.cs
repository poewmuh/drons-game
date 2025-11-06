using System;
using System.Collections.Generic;
using DronsTeam.Events;

namespace DronsTeam.Core
{
    public class TeamResourceTracker : IDisposable
    {
        private readonly Dictionary<int, int> _teamResources = new();

        public TeamResourceTracker()
        {
            EventBus.Subscribe<ResourceCollectedEvent>(OnResourceCollected);
        }

        public void RegisterTeam(int fractionId)
        {
            _teamResources.TryAdd(fractionId, 0);
        }

        private void OnResourceCollected(ResourceCollectedEvent evt)
        {
            if (_teamResources.ContainsKey(evt.FractionId))
            {
                _teamResources[evt.FractionId]++;
            }
        }

        public int GetTeamResources(int fractionId)
        {
            return _teamResources.GetValueOrDefault(fractionId, 0);
        }

        public Dictionary<int, int> GetAllTeamResources()
        {
            return new Dictionary<int, int>(_teamResources);
        }

        public void Dispose()
        {
            EventBus.Unsubscribe<ResourceCollectedEvent>(OnResourceCollected);
        }
    }
}
