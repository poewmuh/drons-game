using System.Globalization;
using DronsTeam.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DronsTeam.UI
{
    public class DronsCountUI : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private TextMeshProUGUI _valueText;

        public void Initialize(int initialValue)
        {
            _slider.value = initialValue;
            _valueText.text = initialValue.ToString();
            _slider.onValueChanged.AddListener(OnSliderChange);
        }

        private void OnSliderChange(float newValue)
        {
            _valueText.text = newValue.ToString(CultureInfo.InvariantCulture);
            EventBus.Publish(new DroneCountChangedEvent((int)newValue));
        }

        private void OnDestroy()
        {
            _slider.onValueChanged.RemoveListener(OnSliderChange);
        }
    }
}
