using DronsTeam.Events;
using TMPro;
using UnityEngine;

namespace DronsTeam.UI
{
    public class ResourcesSpeedUI : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _inputField;

        public void Initialize(int initialValue)
        {
            _inputField.text = initialValue.ToString();
            _inputField.onValueChanged.AddListener(OnInputChange);
        }

        private void OnInputChange(string newValueString)
        {
            if (float.TryParse(newValueString, out var newValue) && newValue > 0)
            {
                EventBus.Publish(new ResourceSpawnRateChangedEvent(newValue));   
            }
            else
            {
                Debug.LogError($"[ResourcesSpeedUI] Failed to parse resource spawn rate from {newValueString}");
            }
        }

        private void OnDestroy()
        {
            _inputField.onValueChanged.RemoveListener(OnInputChange);
        }
    }
}
