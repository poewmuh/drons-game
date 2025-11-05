using System;
using DronsTeam.Config;
using DronsTeam.Core;
using DronsTeam.Tools;
using UnityEngine;

namespace DronsTeam.Boot
{
    public class GameEntryPoint : MonoBehaviour
    {
        [Header("Configs")]
        [SerializeField] private DronsConfig _dronsConfig;
        [SerializeField] private FractionConfig _fractionConfig;
        [SerializeField] private ResourcesConfig _resourcesConfig;
        
        private AddressablesLoader _addressablesLoader;
        private FortSpawner _fortSpawner;

        private void Awake()
        {
            _addressablesLoader = new AddressablesLoader();
            _fortSpawner = new FortSpawner(_fractionConfig, _addressablesLoader);
        }

        private void Start()
        {
            _fortSpawner.Initialize();
        }

        private void OnDestroy()
        {
            _addressablesLoader.Dispose();
        }
    }
}
