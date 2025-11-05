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
        private FortsManager _fortsManager;
        private ResourceManager _resourceManager;

        private void Awake()
        {
            _addressablesLoader = new AddressablesLoader();
            _fortsManager = new FortsManager(_fractionConfig, _addressablesLoader);
            _resourceManager = new ResourceManager(_resourcesConfig, _addressablesLoader);
        }

        private void Start()
        {
            _fortsManager.Initialize();
            _resourceManager.Initialize();
            
            _settingsUIManager.Initialize(_dronsConfig, _resourcesConfig);
        }

        private void OnDestroy()
        {
            _addressablesLoader.Dispose();
            _fortsManager.Dispose();
            _resourceManager.Dispose();
        }
    }
}
