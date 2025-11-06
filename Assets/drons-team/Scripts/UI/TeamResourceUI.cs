using System.Linq;
using DG.Tweening;
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
        [Header("Animation Settings")]
        [SerializeField] private float _scaleDuration = 0.3f;
        [SerializeField] private float _scaleMultiplier = 1.3f;

        private TeamResourceTracker _resourceTracker;

        private int _currentTeam1Resources = -1;
        private int _currentTeam2Resources = -1;
        private int _team1Id;
        private int _team2Id;

        private Sequence _sequenceTeam1;
        private Sequence _sequenceTeam2;

        public void Initialize(TeamResourceTracker resourceTracker)
        {
            _resourceTracker = resourceTracker;

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

            if (_currentTeam1Resources != team1Resources)
            {
                AnimateScale(_team1ResourceText, ref _sequenceTeam1);
                _team1ResourceText.text = team1Resources.ToString();
            }

            if (_currentTeam2Resources != team2Resources)
            {
                AnimateScale(_team2ResourceText, ref _sequenceTeam2);
                _team2ResourceText.text = team2Resources.ToString();
            }

            _currentTeam1Resources = team1Resources;
            _currentTeam2Resources = team2Resources;

            UpdateSlider(team1Resources, team2Resources);
        }

        private void AnimateScale(TMP_Text textField, ref Sequence sequence)
        {
            sequence = DOTween.Sequence();
            textField.transform.localScale = Vector3.one;
            sequence
                .Append(textField.transform.DOScale(_scaleMultiplier, _scaleDuration * 0.5f))
                .Append(textField.transform.DOScale(1f, _scaleDuration * 0.5f));
        }

        private void UpdateSlider(int team1Resources, int team2Resources)
        {
            var totalResources = team1Resources + team2Resources;
            var targetValue = 0f;

            if (totalResources == 0)
            {
                targetValue = 0.5f;
            }
            else
            {
                targetValue = (float)team1Resources / totalResources;
            }

            _balanceSlider.DOValue(targetValue, _scaleDuration);
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe<ResourceCollectedEvent>(OnResourceCollected);

            _sequenceTeam1.Kill(true);
            _sequenceTeam2.Kill(true);
        }
    }
}
