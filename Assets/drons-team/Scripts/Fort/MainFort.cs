using DronsTeam.Config;
using UnityEngine;

namespace DronsTeam.Fort
{
    public class MainFort : MonoBehaviour
    {
        [SerializeField] private FortMeshHandler _meshHandler;
        
        private FortFractionHandler _fractionHandler;
        
        public void Initialize(FractionData data)
        {
            _fractionHandler = new FortFractionHandler(data.id);
            _meshHandler.Initialize(data.fractionColor);

            transform.position = data.fractionPos;
        }
    }
}
