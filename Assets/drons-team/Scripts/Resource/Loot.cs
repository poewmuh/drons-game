using DronsTeam.Resources;
using UnityEngine;

namespace DefaultNamespace
{
    public class Loot : MonoBehaviour
    {
        private LootStatus _status;

        public void Initialize(Vector3 position)
        {
            _status = LootStatus.Available;
            transform.position = position;
        }
    }
}
