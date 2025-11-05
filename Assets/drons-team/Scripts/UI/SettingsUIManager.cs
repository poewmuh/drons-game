using DronsTeam.Config;
using UnityEngine;

namespace DronsTeam.UI
{
    public class SettingsUIManager : MonoBehaviour
    {
        [SerializeField] private DronsCountUI _dronsCountUI;
        [SerializeField] private DronsSpeedUI _dronsSpeedUI;
        [SerializeField] private ResourcesSpeedUI _resourcesSpeedUI;
        [SerializeField] private DebugDrawUI _debugDrawUI;

        public void Initialize(DronsConfig dronsConfig, ResourcesConfig resourcesConfig)
        {
            _dronsCountUI.Initialize(dronsConfig.DronsCount);
            _dronsSpeedUI.Initialize(dronsConfig.DronsSpeed);
            _debugDrawUI.Initialize(dronsConfig.DebugPath);
            _resourcesSpeedUI.Initialize(resourcesConfig.ResourcesPerSpawn);
        }
    }
}
