using UnityEngine;

namespace DronsTeam.Events
{
    public readonly struct ResourceSpawnEvent
    {
        public readonly Vector3 Position;

        public ResourceSpawnEvent(Vector3 position)
        {
            Position = position;
        }
    }
}
