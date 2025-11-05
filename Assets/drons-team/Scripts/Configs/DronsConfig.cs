using UnityEngine;

namespace DronsTeam.Config
{
    [CreateAssetMenu(fileName = "DronsConfig", menuName = "Data/DronsConfig")]
    public class DronsConfig : ScriptableObject
    {
        [SerializeField, Range(1,5)] private int _dronsCount = 3;
        [SerializeField, Range(1,10)] private int _dronsSpeed = 5;
        [SerializeField] private bool _debugPath = false;
        [SerializeField] private Color _debugPathColor = Color.white;
        [SerializeField, Range(0,5)] private float _collectionTime = 2f;
        
        public int DronsCount => _dronsCount;
        public int DronsSpeed => _dronsSpeed;
        public bool DebugPath => _debugPath;
        public Color DebugPathColor => _debugPathColor;
        public float CollectionTime => _collectionTime;
    }
}
