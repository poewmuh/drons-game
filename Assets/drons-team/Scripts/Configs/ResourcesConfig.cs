using UnityEngine;

namespace DronsTeam.Config
{
    [CreateAssetMenu(fileName = "ResourcesConfig", menuName = "Data/ResourcesConfig")]
    public class ResourcesConfig : ScriptableObject
    {
        [SerializeField, Range(0, 100)] private float _spawnInterval = 1;
        [SerializeField, Range(1, 100)] private int _resourcesPerSpawn = 1;
        [SerializeField] private Vector3 _spawnCenter;
        [SerializeField] private Vector3 _minSpawnRadius;
        [SerializeField] private Vector3 _maxSpawnRadius;

        public float SpawnInterval => _spawnInterval;
        public int ResourcesPerSpawn => _resourcesPerSpawn;
        public Vector3 SpawnCenter => _spawnCenter;
        public Vector3 MinSpawnRadius => _minSpawnRadius;
        public Vector3 MaxSpawnRadius => _maxSpawnRadius;
    }
}
