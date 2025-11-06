using System;
using DronsTeam.Config;
using DronsTeam.Events;
using DronsTeam.Tools;

namespace DronsTeam.Core
{
    public class DroneManager : IDisposable
    {
        private readonly FortsManager _fortsManager;
        private readonly ResourceManager _resourceManager;
        private readonly DroneSpawner _spawner;
        private readonly DroneAvoidanceService _avoidanceService;

        private float _currentSpeed;

        public DroneManager(DronsConfig config, AddressablesLoader loader, FortsManager fortsManager,
            ResourceManager resourceManager)
        {
            _fortsManager = fortsManager;
            _resourceManager = resourceManager;

            _currentSpeed = config.DronsSpeed * 10;

            _spawner = new DroneSpawner(config, loader);
            _avoidanceService = new DroneAvoidanceService(_spawner.ActiveDrones);

            EventBus.Subscribe<DroneCountChangedEvent>(OnDroneCountChanged);
            EventBus.Subscribe<DroneSpeedChangedEvent>(OnDroneSpeedChanged);
            EventBus.Subscribe<PathVisualizationToggleEvent>(OnPathVisualizationToggle);
        }

        public void Initialize()
        {
            _spawner.Initialize();

            var forts = _fortsManager.GetAllForts();
            _spawner.SpawnDronesForForts(forts, _avoidanceService, _resourceManager, _currentSpeed);
        }

        private void OnDroneCountChanged(DroneCountChangedEvent evt)
        {
            var forts = _fortsManager.GetAllForts();
            if (forts.Count == 0) return;

            var currentCount = _spawner.ActiveDrones.Count;
            var currentPerFaction = forts.Count > 0 ? currentCount / forts.Count : 0;

            if (evt.NewCount > currentPerFaction)
            {
                var toAddPerFaction = evt.NewCount - currentPerFaction;
                _spawner.AddDronesForForts(forts, toAddPerFaction, _avoidanceService, _resourceManager, _currentSpeed);
            }
            else if (evt.NewCount < currentPerFaction)
            {
                var toRemovePerFaction = currentPerFaction - evt.NewCount;
                _spawner.RemoveDronesForForts(forts, toRemovePerFaction);
            }
        }

        private void OnDroneSpeedChanged(DroneSpeedChangedEvent evt)
        {
            _currentSpeed = evt.NewSpeed * 10;
            foreach (var drone in _spawner.ActiveDrones)
            {
                drone.SetSpeed(_currentSpeed);
            }
        }

        private void OnPathVisualizationToggle(PathVisualizationToggleEvent evt)
        {
            foreach (var drone in _spawner.ActiveDrones)
            {
                drone.SetDebugPathEnabled(evt.IsEnabled);
            }
        }

        public void Dispose()
        {
            EventBus.Unsubscribe<DroneCountChangedEvent>(OnDroneCountChanged);
            EventBus.Unsubscribe<DroneSpeedChangedEvent>(OnDroneSpeedChanged);
            EventBus.Unsubscribe<PathVisualizationToggleEvent>(OnPathVisualizationToggle);

            _spawner?.Dispose();
        }
    }
}
