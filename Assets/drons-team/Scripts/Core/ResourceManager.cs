using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DronsTeam.Config;
using DronsTeam.Events;
using DronsTeam.Resources;
using DronsTeam.Tools;
using UnityEngine;
using UnityEngine.Pool;
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

        private readonly List<Loot> _activeLoot = new();
        private readonly ObjectPool<Loot> _lootPool; 

        private float _spawnInterval;
        
        public ResourceManager(ResourcesConfig resourcesConfig, AddressablesLoader loader)
        {
            _addressablesLoader = loader;
            _config = resourcesConfig;
            _spawnInterval = _config.SpawnInterval;

            _lootPool = new ObjectPool<Loot>(
                createFunc : CreateLoot,
                actionOnGet: OnGetLoot,
                actionOnRelease: OnReleaseLoot,
                defaultCapacity: 10,
                maxSize: 20);
            
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
                    _lootPool.Get();
                }
            }
        }

        private Loot CreateLoot()
        {
            return Object.Instantiate(_lootPrefab).GetComponent<Loot>();
        }

        private void OnGetLoot(Loot loot)
        {
            loot.gameObject.SetActive(true);
            loot.Initialize(GetRandomLootPos());
            _activeLoot.Add(loot);
        }

        private void OnReleaseLoot(Loot loot)
        {
            loot.gameObject.SetActive(false);
            _activeLoot.Remove(loot);
        }

        private Vector3 GetRandomLootPos()
        {
            var randomX = Random.Range(_config.MinSpawnRadius.x, _config.MaxSpawnRadius.x);
            var randomY = Random.Range(_config.MinSpawnRadius.y, _config.MaxSpawnRadius.y);
            var randomZ = Random.Range(_config.MinSpawnRadius.z, _config.MaxSpawnRadius.z);
            return _config.SpawnCenter + new Vector3(randomX, randomY, randomZ);
        }

        private void OnSpawnRateChanged(ResourceSpawnRateChangedEvent evnt)
        {
            _tokenSource.Cancel();
            _tokenSource = new CancellationTokenSource();
            _spawnInterval = evnt.NewRate;
            SpawnLootAfterDelay(_tokenSource.Token).Forget();
        }

        public List<Loot> GetActiveLoot()
        {
            return _activeLoot;
        }

        public void ReleaseLoot(Loot loot)
        {
            _lootPool.Release(loot);
        }

        public void Dispose()
        {
            _tokenSource?.Cancel();
            _tokenSource?.Dispose();
            EventBus.Unsubscribe<ResourceSpawnRateChangedEvent>(OnSpawnRateChanged);
        }
    }
}
