using DronsTeam.Events;
using UnityEngine;
using UnityEngine.UI;

namespace DronsTeam.UI
{
    public class DebugDrawUI : MonoBehaviour
    {
        [SerializeField] private Toggle _toggle;

        public void Initialize(bool isEnabled)
        {
            _toggle.isOn = isEnabled;
            _toggle.onValueChanged.AddListener(OnToggleValueChanged);
        }

        private void OnToggleValueChanged(bool newValue)
        {
            EventBus.Publish(new PathVisualizationToggleEvent(newValue));
        }

        private void OnDestroy()
        {
            _toggle.onValueChanged.RemoveListener(OnToggleValueChanged);
        }
    }
}
