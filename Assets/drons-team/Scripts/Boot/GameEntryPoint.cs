using DronsTeam.Config;
using DronsTeam.Core;
using DronsTeam.Tools;
using DronsTeam.UI;
using UnityEngine;

namespace DronsTeam.Boot
{
    // Тут лучше использовать DI Container для прокидывания зависимостей и правильной инициализации
    public class GameEntryPoint : MonoBehaviour
    {
        [Header("Configs")]
        [SerializeField] private DronsConfig _dronsConfig;
        [SerializeField] private FractionConfig _fractionConfig;
        [SerializeField] private ResourcesConfig _resourcesConfig;
        [Header("Links")]
        [SerializeField] private SettingsUIManager _settingsUIManager;
        
        private AddressablesLoader _addressablesLoader;
        private FortSpawner _fortSpawner;

        private void Awake()
        {
            _addressablesLoader = new AddressablesLoader();
            _fortSpawner = new FortSpawner(_fractionConfig, _addressablesLoader);
        }

        private void Start()
        {
            _settingsUIManager.Initialize(_dronsConfig, _resourcesConfig);
            _fortSpawner.Initialize();
        }

        private void OnDestroy()
        {
            _addressablesLoader.Dispose();
        }
    }
}
