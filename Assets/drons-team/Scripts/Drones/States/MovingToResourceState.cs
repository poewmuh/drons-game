using DronsTeam.Resources;
using UnityEngine;

namespace DronsTeam.Drones.States
{
    public class MovingToResourceState : IDroneState
    {
        private readonly DroneStateContext _context;

        public MovingToResourceState(DroneStateContext context)
        {
            _context = context;
        }

        public void Enter() { }

        public void Update()
        {
            if (_context.TargetLoot == null || _context.TargetLoot.Status == LootStatus.Collected)
            {
                _context.TargetLoot = null;
                _context.StateMachine.ChangeState(DroneStateType.SearchingResource);
                return;
            }

            var currentPos = _context.Transform.position;
            var targetPos = _context.TargetLoot.Position;

            var direction = (targetPos - currentPos).normalized;
            
            var droneAvoidance = _context.AvoidanceService.CalculateAvoidance(_context.Drone);
            direction = (direction + droneAvoidance).normalized;

            var newPos = currentPos + direction * _context.Speed * Time.deltaTime;
            _context.Rigidbody.MovePosition(newPos);
            _context.Drone.UpdatePath(currentPos, targetPos);

            if (Vector3.Distance(currentPos, targetPos) < 0.5f)
            {
                _context.StateMachine.ChangeState(DroneStateType.Collecting);
            }
        }

        public void Exit() { }
    }
}
