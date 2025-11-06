using DronsTeam.Events;

namespace DronsTeam.Drones.States
{
    public class DepositingState : IDroneState
    {
        private readonly DroneStateContext _context;

        public DepositingState(DroneStateContext context)
        {
            _context = context;
        }

        public void Enter()
        {
            _context.Drone.ClearPath();

            EventBus.Publish(new ResourceCollectedEvent(_context.Drone.FractionId));

            // TODO: Trigger visual effects (particles, flash, scale)
            _context.StateMachine.ChangeState(DroneStateType.SearchingResource);
        }

        public void Update() { }

        public void Exit() { }
    }
}
