using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace DronsTeam.Core
{
    public class VFXPool : IDisposable
    {
        private readonly GameObject _vfxPrefab;
        private readonly ObjectPool<ParticleSystem> _pool;
        private readonly List<ParticleSystem> _activeEffects = new();

        public VFXPool(GameObject vfxPrefab, int defaultCapacity = 5, int maxSize = 20)
        {
            _vfxPrefab = vfxPrefab;

            _pool = new ObjectPool<ParticleSystem>(
                createFunc: CreateVFX,
                actionOnGet: OnGetVFX,
                actionOnRelease: OnReleaseVFX,
                actionOnDestroy: OnDestroyVFX,
                defaultCapacity: defaultCapacity,
                maxSize: maxSize);
        }

        private ParticleSystem CreateVFX()
        {
            return Object.Instantiate(_vfxPrefab).GetComponent<ParticleSystem>();
        }

        private void OnGetVFX(ParticleSystem vfx)
        {
            vfx.gameObject.SetActive(true);
            _activeEffects.Add(vfx);
        }

        private void OnReleaseVFX(ParticleSystem vfx)
        {
            vfx.gameObject.SetActive(false);
            _activeEffects.Remove(vfx);
        }

        private void OnDestroyVFX(ParticleSystem vfx)
        {
            if (vfx != null && vfx.gameObject != null)
            {
                Object.Destroy(vfx.gameObject);
            }
        }

        public void Play(Vector3 position, Quaternion rotation = default)
        {
            var vfx = _pool.Get();
            vfx.transform.position = position;
            vfx.transform.rotation = rotation == default ? Quaternion.identity : rotation;
            vfx.Play();

            var duration = vfx.main.duration + vfx.main.startLifetime.constantMax;
            ReturnToPoolAfterDelay(vfx, duration).Forget();
        }

        private async UniTaskVoid ReturnToPoolAfterDelay(ParticleSystem vfx, float delay)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay));

            if (vfx != null && vfx.gameObject != null)
            {
                _pool.Release(vfx);
            }
        }

        public void Dispose()
        {
            _pool?.Clear();
            _activeEffects.Clear();
        }
    }
}
