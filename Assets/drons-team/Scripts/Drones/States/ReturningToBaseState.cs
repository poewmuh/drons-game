using UnityEngine;

namespace DronsTeam.Drones.States
{
    public class ReturningToBaseState : IDroneState
    {
        private readonly DroneStateContext _context;

        public ReturningToBaseState(DroneStateContext context)
        {
            _context = context;
        }

        public void Enter() { }

        public void Update()
        {
            var currentPos = _context.Transform.position;
            var depositPos = _context.HomeFort.DepositPoint;

            var direction = (depositPos - currentPos).normalized;

            var droneAvoidance = _context.AvoidanceService.CalculateAvoidance(_context.Drone);
            direction = (direction + droneAvoidance).normalized;

            var newPos = currentPos + direction * _context.Speed * Time.deltaTime;
            _context.Rigidbody.MovePosition(newPos);
            _context.Drone.UpdatePath(currentPos, depositPos);

            if (Vector3.Distance(currentPos, depositPos) < 0.5f)
            {
                _context.StateMachine.ChangeState(DroneStateType.Depositing);
            }
        }

        public void Exit() { }
    }
}
