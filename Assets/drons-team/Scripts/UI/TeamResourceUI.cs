using System.Linq;
using DronsTeam.Core;
using DronsTeam.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DronsTeam.UI
{
    public class TeamResourceUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _team1ResourceText;
        [SerializeField] private TMP_Text _team2ResourceText;
        [SerializeField] private Slider _balanceSlider;

        private TeamResourceTracker _resourceTracker;
        private int _team1Id = -1;
        private int _team2Id = -1;

        public void Initialize(TeamResourceTracker resourceTracker)
        {
            _resourceTracker = resourceTracker;

            // Get team IDs from resource tracker
            var teams = _resourceTracker.GetAllTeamResources();
            if (teams.Count >= 2)
            {
                var teamIds = teams.Keys.ToList();
                _team1Id = teamIds[0];
                _team2Id = teamIds[1];
            }

            EventBus.Subscribe<ResourceCollectedEvent>(OnResourceCollected);
            UpdateUI();
        }

        private void OnResourceCollected(ResourceCollectedEvent evt)
        {
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (_resourceTracker == null)
                return;

            var team1Resources = _team1Id >= 0 ? _resourceTracker.GetTeamResources(_team1Id) : 0;
            var team2Resources = _team2Id >= 0 ? _resourceTracker.GetTeamResources(_team2Id) : 0;
            _team1ResourceText.text = team1Resources.ToString();
            _team2ResourceText.text = team2Resources.ToString();

            UpdateSlider(team1Resources, team2Resources);
        }

        private void UpdateSlider(int team1Resources, int team2Resources)
        {
            var totalResources = team1Resources + team2Resources;

            if (totalResources == 0)
            {
                _balanceSlider.value = 0.5f;
            }
            else
            {
                var ratio = (float)team1Resources / totalResources;
                _balanceSlider.value = ratio;
            }
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe<ResourceCollectedEvent>(OnResourceCollected);
        }
    }
}
