using UnityEngine;

namespace DronsTeam.Fort
{
    public class FortMeshHandler : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _meshRenderer;
        
        private MaterialPropertyBlock _materialPropertyBlock;

        public void Initialize(Color fractionColor)
        {
            _materialPropertyBlock = new MaterialPropertyBlock();
            _materialPropertyBlock.SetColor("_BaseColor", fractionColor);
            _meshRenderer.SetPropertyBlock(_materialPropertyBlock);
        }
    }
}
