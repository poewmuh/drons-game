using System;
using System.Collections.Generic;
using DronsTeam.Config;
using DronsTeam.Drones;
using DronsTeam.Events;
using DronsTeam.Fort;
using DronsTeam.Tools;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace DronsTeam.Core
{
    public class DroneSpawner : IDisposable
    {
        private readonly DronsConfig _config;
        private readonly AddressablesLoader _addressablesLoader;
        private readonly ObjectPool<Drone> _dronePool;
        private readonly List<Drone> _activeDrones = new();

        private GameObject _dronePrefab;
        private int _currentDroneCount;
        private DroneAvoidanceService _avoidanceService;

        public IReadOnlyList<Drone> ActiveDrones => _activeDrones;

        public DroneSpawner(DronsConfig config, AddressablesLoader loader)
        {
            _config = config;
            _addressablesLoader = loader;
            _currentDroneCount = _config.DronsCount;

            _dronePool = new ObjectPool<Drone>(
                createFunc: CreateDrone,
                actionOnGet: OnGetDrone,
                actionOnRelease: OnReleaseDrone,
                defaultCapacity: 10,
                maxSize: 20);

            EventBus.Subscribe<DroneCountChangedEvent>(OnDroneCountChanged);
        }

        public void Initialize()
        {
            _dronePrefab = _addressablesLoader.LoadImmediate<GameObject>(AddressablesHelper.DRON_KEY);
        }

        public void SpawnDronesForForts(List<MainFort> forts, DroneAvoidanceService avoidanceService,
            ResourceManager resourceManager, float speed)
        {
            _avoidanceService = avoidanceService;

            foreach (var fort in forts)
            {
                for (int i = 0; i < _currentDroneCount; i++)
                {
                    var drone = _dronePool.Get();
                    InitializeDrone(drone, fort, avoidanceService, resourceManager, speed);
                }
            }
        }

        public void AddDronesForForts(List<MainFort> forts, int count, DroneAvoidanceService avoidanceService,
            ResourceManager resourceManager, float speed)
        {
            foreach (var fort in forts)
            {
                for (int i = 0; i < count; i++)
                {
                    var drone = _dronePool.Get();
                    InitializeDrone(drone, fort, avoidanceService, resourceManager, speed);
                }
            }
        }

        public void RemoveDronesForForts(List<MainFort> forts, int count)
        {
            foreach (var fort in forts)
            {
                var removed = 0;
                for (int i = _activeDrones.Count - 1; i >= 0 && removed < count; i--)
                {
                    if (_activeDrones[i].FractionId != fort.FractionId) continue;

                    var drone = _activeDrones[i];
                    _dronePool.Release(drone);
                    removed++;
                }
            }
        }

        private void InitializeDrone(Drone drone, MainFort homeFort, DroneAvoidanceService avoidanceService,
            ResourceManager resourceManager, float speed)
        {
            var context = new DroneStateContext(
                drone,
                homeFort,
                drone.transform,
                drone.Rigidbody,
                avoidanceService,
                resourceManager,
                speed,
                _config.CollectionTime
            );

            var stateMachine = new DroneStateMachine(context);
            context.StateMachine = stateMachine;

            drone.Initialize(homeFort, stateMachine, _config.DebugPathColor);
            drone.SetDebugPathEnabled(_config.DebugPath);

            stateMachine.ChangeState(DroneStateType.SearchingResource);
        }

        private Drone CreateDrone()
        {
            return Object.Instantiate(_dronePrefab).GetComponent<Drone>();
        }

        private void OnGetDrone(Drone drone)
        {
            drone.gameObject.SetActive(true);
            _activeDrones.Add(drone);
        }

        private void OnReleaseDrone(Drone drone)
        {
            drone.gameObject.SetActive(false);
            _activeDrones.Remove(drone);
            _avoidanceService?.ClearCache(drone);
        }

        private void OnDroneCountChanged(DroneCountChangedEvent evt)
        {
            _currentDroneCount = evt.NewCount;
        }

        public void Dispose()
        {
            EventBus.Unsubscribe<DroneCountChangedEvent>(OnDroneCountChanged);
            _dronePool?.Clear();
        }
    }
}
