using DronsTeam.Resources;
using System.Linq;
using UnityEngine;

namespace DronsTeam.Drones.States
{
    public class SearchingResourceState : IDroneState
    {
        private readonly DroneStateContext _context;

        public SearchingResourceState(DroneStateContext context)
        {
            _context = context;
        }

        public void Enter()
        {
            _context.Drone.ClearPath();
            _context.TargetLoot = null;

            var assignedLoot = FindAndReserveNearestResource();

            if (assignedLoot != null)
            {
                _context.TargetLoot = assignedLoot;
                _context.StateMachine.ChangeState(DroneStateType.MovingToResource);
            }
        }

        public void Update()
        {
            if (_context.TargetLoot != null) return;

            var assignedLoot = FindAndReserveNearestResource();

            if (assignedLoot != null)
            {
                _context.TargetLoot = assignedLoot;
                _context.StateMachine.ChangeState(DroneStateType.MovingToResource);
            }
        }

        private Loot FindAndReserveNearestResource()
        {
            var availableLoot = _context.ResourceManager.GetActiveLoot()
                .Where(l => l.Status == LootStatus.Available)
                .OrderBy(l => Vector3.Distance(_context.Drone.Position, l.Position))
                .ToList();

            if (availableLoot.Count > 0)
            {
                var loot = availableLoot[0];
                if (loot.TryReserve())
                {
                    return loot;
                }
            }

            return null;
        }

        public void Exit() { }
    }
}
