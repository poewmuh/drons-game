using DronsTeam.Config;
using UnityEngine;

namespace DronsTeam.Fort
{
    public class MainFort : MonoBehaviour
    {
        [SerializeField] private FortMeshHandler _meshHandler;
        [SerializeField] private float _spawnOffset = 5f;
        [SerializeField] private float _depositOffset = 3f;

        private FortFractionHandler _fractionHandler;
        public int FractionId => _fractionHandler.FractionId;
        public Vector3 SpawnPoint => transform.position + Vector3.up * _spawnOffset;
        public Vector3 DepositPoint => transform.position + Vector3.up * _depositOffset;

        public void Initialize(FractionData data)
        {
            _fractionHandler = new FortFractionHandler(data.id);
            _meshHandler.Initialize(data.fractionColor);

            transform.position = data.fractionPos;
        }
    }
}
