using System.Collections.Generic;
using DG.Tweening;
using IKhom.EventBusSystem.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.DuckGame
{
    public class DuckGameUI : MonoBehaviour
    {
        [Header("Progress Display")]
        [SerializeField] private TextMeshProUGUI _progressText;

        [SerializeField] private Transform _progressDotsContainer;
        [SerializeField] private GameObject _progressDotPrefab;
        [SerializeField] private Color _completedDotColor = Color.green;
        [SerializeField] private Color _incompleteDotColor = Color.gray;

        [Header("Animation Settings")]
        [SerializeField] private float _dotAnimationDuration = 0.5f;

        [SerializeField] private float _textAnimationDuration = 0.3f;

        private List<Image> _progressDots = new();
        private int _totalRounds = 5;
        private int _currentRound = 1;

        // Event bindings
        private EventBinding<Services.DuckEvents.RoundCompleteEvent> _roundCompleteBinding;
        private EventBinding<Services.DuckEvents.BasketsSpawnedEvent> _basketsSpawnedBinding;

        private void Awake()
        {
            SetupEventBindings();
        }

        private void OnDestroy()
        {
            CleanupEventBindings();
        }

        private void SetupEventBindings()
        {
            _roundCompleteBinding = new EventBinding<Services.DuckEvents.RoundCompleteEvent>(OnRoundComplete);
            _basketsSpawnedBinding = new EventBinding<Services.DuckEvents.BasketsSpawnedEvent>(OnBasketsSpawned);

            EventBus<Services.DuckEvents.RoundCompleteEvent>.Register(_roundCompleteBinding);
            EventBus<Services.DuckEvents.BasketsSpawnedEvent>.Register(_basketsSpawnedBinding);
        }

        private void CleanupEventBindings()
        {
            EventBus<Services.DuckEvents.RoundCompleteEvent>.Deregister(_roundCompleteBinding);
            EventBus<Services.DuckEvents.BasketsSpawnedEvent>.Deregister(_basketsSpawnedBinding);
        }

        public void Initialize(int totalRounds)
        {
            _totalRounds = totalRounds;
            _currentRound = 1;

            CreateProgressDots();
            UpdateProgressDisplay();
        }

        private void CreateProgressDots()
        {
            // Clear existing dots
            foreach (var dot in _progressDots)
            {
                if (dot != null) Destroy(dot.gameObject);
            }

            _progressDots.Clear();

            // Create new dots
            for (int i = 0; i < _totalRounds; i++)
            {
                var dotObj = Instantiate(_progressDotPrefab, _progressDotsContainer);
                var dotImage = dotObj.GetComponent<Image>();

                if (dotImage != null)
                {
                    dotImage.color = _incompleteDotColor;
                    _progressDots.Add(dotImage);
                }
            }
        }

        private void UpdateProgressDisplay()
        {
            // Update text
            if (_progressText != null)
            {
                _progressText.text = $"{_currentRound}/{_totalRounds}";

                // Animate text
                _progressText.transform.DOPunchScale(Vector3.one * 0.2f, _textAnimationDuration, 2);
            }
        }

        private void OnRoundComplete(Services.DuckEvents.RoundCompleteEvent eventData)
        {
            _currentRound = eventData.CompletedRound + 1; // Next round

            // Animate completed dot
            if (eventData.CompletedRound - 1 < _progressDots.Count)
            {
                var dotToComplete = _progressDots[eventData.CompletedRound - 1];
                AnimateCompletedDot(dotToComplete);
            }

            UpdateProgressDisplay();
        }

        private void OnBasketsSpawned(Services.DuckEvents.BasketsSpawnedEvent eventData)
        {
            _currentRound = eventData.RoundNumber;
            UpdateProgressDisplay();
        }

        private void AnimateCompletedDot(Image dot)
        {
            if (dot == null) return;

            var sequence = DOTween.Sequence();

            // Scale up, change color, scale back
            sequence.Append(dot.transform.DOScale(Vector3.one * 1.5f, _dotAnimationDuration * 0.5f))
                .Join(dot.DOColor(_completedDotColor, _dotAnimationDuration * 0.5f))
                .Append(dot.transform.DOScale(Vector3.one, _dotAnimationDuration * 0.5f))
                .SetEase(Ease.OutBounce);
        }

        public void ShowGameComplete()
        {
            // Special animation when all rounds are complete
            if (_progressText != null)
            {
                _progressText.text = "Complete!";
                _progressText.transform.DOScale(Vector3.one * 1.5f, 0.5f)
                    .SetEase(Ease.OutBounce);
            }

            // Animate all dots in sequence
            for (int i = 0; i < _progressDots.Count; i++)
            {
                var dot = _progressDots[i];
                DOVirtual.DelayedCall(i * 0.1f, () => { dot.transform.DOPunchScale(Vector3.one * 0.3f, 0.3f, 2); });
            }
        }
    }
}