using UnityEngine;

namespace DronsTeam.Drones.States
{
    public class CollectingState : IDroneState
    {
        private readonly DroneStateContext _context;

        public CollectingState(DroneStateContext context)
        {
            _context = context;
        }

        public void Enter()
        {
            _context.CollectionTimer = 0f;
            _context.Drone.ClearPath();
        }

        public void Update()
        {
            _context.CollectionTimer += Time.deltaTime;
            if (!(_context.CollectionTimer >= _context.CollectionTime)) return;

            if (_context.TargetLoot != null)
            {
                _context.TargetLoot.Collect();
                _context.ResourceManager.ReleaseLoot(_context.TargetLoot);
                _context.TargetLoot = null;
            }

            _context.StateMachine.ChangeState(DroneStateType.ReturningToBase);
        }

        public void Exit() { }
    }
}
