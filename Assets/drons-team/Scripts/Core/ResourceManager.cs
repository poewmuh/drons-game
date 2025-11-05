using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using DronsTeam.Config;
using DronsTeam.Events;
using DronsTeam.Tools;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace DronsTeam.Core
{
    public class ResourceManager : IDisposable
    {
        private readonly AddressablesLoader _addressablesLoader;
        private readonly ResourcesConfig _config;
        private CancellationTokenSource _tokenSource;
        private GameObject _lootPrefab;

        private float _spawnInterval;
        
        public ResourceManager(ResourcesConfig resourcesConfig, AddressablesLoader loader)
        {
            _addressablesLoader = loader;
            _config = resourcesConfig;
            _spawnInterval = _config.SpawnInterval;
            
            EventBus.Subscribe<ResourceSpawnRateChangedEvent>(OnSpawnRateChanged);
        }

        public void Initialize()
        {
            _tokenSource = new CancellationTokenSource();
            _lootPrefab = _addressablesLoader.LoadImmediate<GameObject>(AddressablesHelper.RESOURCE_KEY);

            SpawnLootAfterDelay(_tokenSource.Token).Forget();
        }
        
        private async UniTaskVoid SpawnLootAfterDelay(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_spawnInterval), cancellationToken: ct);
                for (int i = 0; i < _config.ResourcesPerSpawn; i++)
                {
                    var loot = Object.Instantiate(_lootPrefab).GetComponent<Loot>();
                    
                    loot.Initialize(GetRandomLootPos());
                }
            }
        }

        private Vector3 GetRandomLootPos()
        {
            var randomX = Random.Range(_config.MinSpawnRadius.x, _config.MaxSpawnRadius.x);
            var randomY = Random.Range(_config.MinSpawnRadius.y, _config.MaxSpawnRadius.y);
            var randomZ = Random.Range(_config.MinSpawnRadius.z, _config.MaxSpawnRadius.z);
            var randomSphere = Random.insideUnitSphere;
            return _config.SpawnCenter + new Vector3(randomSphere.x * randomX, randomSphere.y * randomY, randomSphere.z * randomZ);
        }

        private void OnSpawnRateChanged(ResourceSpawnRateChangedEvent evnt)
        {
            _tokenSource.Cancel();
            _tokenSource = new CancellationTokenSource();
            _spawnInterval = evnt.NewRate;
            SpawnLootAfterDelay(_tokenSource.Token).Forget();
        }

        public void Dispose()
        {
            _tokenSource?.Cancel();
            _tokenSource?.Dispose();
            EventBus.Unsubscribe<ResourceSpawnRateChangedEvent>(OnSpawnRateChanged);
        }
    }
}
