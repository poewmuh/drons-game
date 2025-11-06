using System;
using DronsTeam.Tools;
using UnityEngine;

namespace DronsTeam.Core
{
    public class VFXManager : IDisposable
    {
        private readonly AddressablesLoader _addressablesLoader;
        private VFXPool _deliveryVFXPool;

        public VFXManager(AddressablesLoader loader)
        {
            _addressablesLoader = loader;
        }

        public void Initialize()
        {
            var deliveryVFXPrefab = _addressablesLoader.LoadImmediate<GameObject>(AddressablesHelper.DELIVERY_VFX_KEY);
            _deliveryVFXPool = new VFXPool(deliveryVFXPrefab, defaultCapacity: 5, maxSize: 20);
        }

        public void PlayDeliveryEffect(Vector3 position)
        {
            _deliveryVFXPool?.Play(position);
        }

        public void Dispose()
        {
            _deliveryVFXPool?.Dispose();
        }
    }
}
