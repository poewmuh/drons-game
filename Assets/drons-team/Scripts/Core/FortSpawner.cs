using DronsTeam.Config;
using DronsTeam.Fort;
using DronsTeam.Tools;
using UnityEngine;

namespace DronsTeam.Core
{
    public class FortSpawner
    {
        private readonly FractionConfig _fractionConfig;
        private readonly AddressablesLoader _addressablesLoader;

        public FortSpawner(FractionConfig fractionConfig, AddressablesLoader addressablesLoader)
        {
            _fractionConfig = fractionConfig;
            _addressablesLoader = addressablesLoader;
        }
        
        public void Initialize()
        {
            foreach (var fractionData in _fractionConfig.GetAllFractionsData())
            {
                SpawnFort(fractionData);
            }
        }

        private void SpawnFort(FractionData data)
        {
            var fortPrefab = _addressablesLoader.LoadImmediate<GameObject>(AddressablesHelper.FORT_KEY);
            var mainFort = Object.Instantiate(fortPrefab).GetComponent<MainFort>();
            
            mainFort.transform.position = data.fractionPos;
            mainFort.Initialize(data.id, data.fractionColor);
        }
    }
}
