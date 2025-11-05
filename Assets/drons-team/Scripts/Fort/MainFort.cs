using UnityEngine;

namespace DronsTeam.Fort
{
    public class MainFort : MonoBehaviour
    {
        [SerializeField] private FortMeshHandler _meshHandler;
        
        private FortFractionHandler _fractionHandler;
        
        public void Initialize(int id, Color fractionColor)
        {
            _fractionHandler = new FortFractionHandler(id);
            _meshHandler.Initialize(fractionColor);
        }
    }
}
