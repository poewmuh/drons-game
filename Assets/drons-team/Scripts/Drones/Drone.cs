using DronsTeam.Fort;
using UnityEngine;

namespace DronsTeam.Drones
{
    public class Drone : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private Rigidbody _rigidbody;
        
        private MainFort _homeFort;
        private DroneStateMachine _stateMachine;
        public Vector3 Position => transform.position;
        public int FractionId => _homeFort.FractionId;
        public Rigidbody Rigidbody => _rigidbody;

        public void Initialize(MainFort homeFort, DroneStateMachine stateMachine, Color debugPathColor)
        {
            _homeFort = homeFort;
            _stateMachine = stateMachine;

            transform.position = homeFort.SpawnPoint;

            SetupLineRenderer(debugPathColor);
        }

        private void SetupLineRenderer(Color color)
        {
            _lineRenderer.startWidth = 0.1f;
            _lineRenderer.endWidth = 0.1f;
            _lineRenderer.startColor = color;
            _lineRenderer.endColor = color;
        }

        public void SetDebugPathEnabled(bool enabled)
        {
            if (_lineRenderer != null)
                _lineRenderer.enabled = enabled;
        }

        public void SetSpeed(float speed)
        {
            if (_stateMachine != null)
            {
                _stateMachine.GetContext().Speed = speed;
            }
        }

        public void UpdatePath(Vector3 start, Vector3 end)
        {
            if (!_lineRenderer.enabled) return;

            _lineRenderer.positionCount = 2;
            _lineRenderer.SetPosition(0, start);
            _lineRenderer.SetPosition(1, end);
        }

        public void ClearPath()
        {
            _lineRenderer.positionCount = 0;
        }

        private void Update()
        {
            _stateMachine?.Update();
        }
    }
}
