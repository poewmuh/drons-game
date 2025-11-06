using System.Collections.Generic;
using DronsTeam.Drones;
using UnityEngine;

namespace DronsTeam.Core
{
    public class DroneAvoidanceService
    {
        private const float UPDATE_INTERVAL = 0.1f;
        private const float AVOIDANCE_RADIUS = 4f;
        private const float SQR_AVOIDANCE_RADIUS = AVOIDANCE_RADIUS * AVOIDANCE_RADIUS;

        private readonly Dictionary<Drone, (Vector3 avoidance, float lastUpdateTime)> _avoidanceCache = new();
        private readonly IReadOnlyList<Drone> _activeDrones;

        public DroneAvoidanceService(IReadOnlyList<Drone> activeDrones)
        {
            _activeDrones = activeDrones;
        }

        public Vector3 CalculateAvoidance(Drone drone)
        {
            if (_avoidanceCache.TryGetValue(drone, out var cached))
            {
                if (Time.time - cached.lastUpdateTime < UPDATE_INTERVAL)
                {
                    return cached.avoidance;
                }
            }

            var avoidance = Vector3.zero;
            var nearbyCount = 0;
            var dronePos = drone.Position;

            foreach (var otherDrone in _activeDrones)
            {
                if (otherDrone == drone || otherDrone == null)
                    continue;

                var offset = dronePos - otherDrone.Position;
                var sqrDistance = offset.sqrMagnitude;

                if (sqrDistance < SQR_AVOIDANCE_RADIUS && sqrDistance > 0.0001f)
                {
                    var distance = Mathf.Sqrt(sqrDistance);
                    var awayDir = offset / distance;
                    var strength = 1f - (distance / AVOIDANCE_RADIUS);
                    avoidance += awayDir * strength;
                    nearbyCount++;
                }
            }

            if (nearbyCount > 0)
            {
                avoidance /= nearbyCount;
            }

            _avoidanceCache[drone] = (avoidance, Time.time);

            return avoidance;
        }

        public void ClearCache(Drone drone)
        {
            _avoidanceCache.Remove(drone);
        }
    }
}
