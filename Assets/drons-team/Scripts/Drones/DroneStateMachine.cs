using System.Collections.Generic;
using DronsTeam.Drones.States;

namespace DronsTeam.Drones
{
    public class DroneStateMachine
    {
        private IDroneState _currentState;
        private readonly DroneStateContext _context;
        private readonly Dictionary<DroneStateType, IDroneState> _states;

        public DroneStateMachine(DroneStateContext context)
        {
            _context = context;

            _states = new Dictionary<DroneStateType, IDroneState>
            {
                { DroneStateType.SearchingResource, new SearchingResourceState(_context) },
                { DroneStateType.MovingToResource, new MovingToResourceState(_context) },
                { DroneStateType.Collecting, new CollectingState(_context) },
                { DroneStateType.ReturningToBase, new ReturningToBaseState(_context) },
                { DroneStateType.Depositing, new DepositingState(_context) }
            };
        }

        public void ChangeState(DroneStateType stateType)
        {
            if (!_states.TryGetValue(stateType, out var newState))
                return;

            _currentState?.Exit();
            _currentState = newState;
            _currentState?.Enter();
        }

        public void Update()
        {
            _currentState?.Update();
        }

        public DroneStateContext GetContext()
        {
            return _context;
        }
    }
}

