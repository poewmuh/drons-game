using DronsTeam.Core;
using DronsTeam.Fort;
using DronsTeam.Resources;
using UnityEngine;

namespace DronsTeam.Drones
{
    public class DroneStateContext
    {
        public Drone Drone { get; }
        public MainFort HomeFort { get; }
        public Transform Transform { get; }
        public Rigidbody Rigidbody { get; }
        public DroneAvoidanceService AvoidanceService { get; }
        public ResourceManager ResourceManager { get; }
        public VFXManager VFXManager { get; }
        public DroneStateMachine StateMachine { get; set; }
        public float Speed { get; set; }
        public float CollectionTime { get; }

        public Loot TargetLoot { get; set; }
        public float CollectionTimer { get; set; }

        public DroneStateContext(Drone drone, MainFort homeFort, Transform transform, Rigidbody rigidbody,
            DroneAvoidanceService avoidanceService, ResourceManager resourceManager, VFXManager vfxManager,
            float speed, float collectionTime)
        {
            Drone = drone;
            HomeFort = homeFort;
            Transform = transform;
            Rigidbody = rigidbody;
            AvoidanceService = avoidanceService;
            ResourceManager = resourceManager;
            VFXManager = vfxManager;
            Speed = speed;
            CollectionTime = collectionTime;
        }
    }
}
