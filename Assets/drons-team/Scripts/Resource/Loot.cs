using UnityEngine;

namespace DronsTeam.Resources
{
    public class Loot : MonoBehaviour
    {
        private LootStatus _status;

        public LootStatus Status => _status;
        public Vector3 Position => transform.position;

        public void Initialize(Vector3 position)
        {
            _status = LootStatus.Available;
            transform.position = position;
        }

        public bool TryReserve()
        {
            if (_status != LootStatus.Available)
                return false;

            _status = LootStatus.Collecting;
            return true;
        }

        public void ReleaseReservation()
        {
            if (_status == LootStatus.Collecting)
                _status = LootStatus.Available;
        }

        public void Collect()
        {
            _status = LootStatus.Collected;
        }
    }
}
