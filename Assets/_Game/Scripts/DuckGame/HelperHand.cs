using DG.Tweening;
using IKhom.EventBusSystem.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.DuckGame
{
    public class HelperHand : MonoBehaviour
    {
        [Header("Hand Settings")]
        [SerializeField] private Image _handImage;

        [SerializeField] private float _animationDuration = 2f;
        [SerializeField] private float _displayDuration = 3f;
        [SerializeField] private int _repeatCount = 2;

        [Header("Animation Settings")]
        [SerializeField] private float _fadeInDuration = 0.5f;

        [SerializeField] private float _fadeOutDuration = 0.5f;
        [SerializeField] private AnimationCurve _movementCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        private EventBinding<Services.DuckEvents.HelperHandRequestedEvent> _helperHandBinding;
        private EventBinding<Services.DuckEvents.DuckTouchedEvent> _duckTouchedBinding;
        private EventBinding<Services.DuckEvents.DuckStartedDraggingEvent> _duckDragBinding;

        private Sequence _currentAnimation;
        private bool _isShowing = false;

        private void Awake()
        {
            SetupEventBindings();
            HideImmediate();
        }

        private void OnDestroy()
        {
            CleanupEventBindings();
            _currentAnimation?.Kill();
        }

        private void SetupEventBindings()
        {
            _helperHandBinding = new EventBinding<Services.DuckEvents.HelperHandRequestedEvent>(OnHelperHandRequested);
            _duckTouchedBinding = new EventBinding<Services.DuckEvents.DuckTouchedEvent>(OnDuckTouched);
            _duckDragBinding = new EventBinding<Services.DuckEvents.DuckStartedDraggingEvent>(OnDuckStartedDragging);

            EventBus<Services.DuckEvents.HelperHandRequestedEvent>.Register(_helperHandBinding);
            EventBus<Services.DuckEvents.DuckTouchedEvent>.Register(_duckTouchedBinding);
            EventBus<Services.DuckEvents.DuckStartedDraggingEvent>.Register(_duckDragBinding);
        }

        private void CleanupEventBindings()
        {
            EventBus<Services.DuckEvents.HelperHandRequestedEvent>.Deregister(_helperHandBinding);
            EventBus<Services.DuckEvents.DuckTouchedEvent>.Deregister(_duckTouchedBinding);
            EventBus<Services.DuckEvents.DuckStartedDraggingEvent>.Deregister(_duckDragBinding);
        }

        private void OnHelperHandRequested(Services.DuckEvents.HelperHandRequestedEvent eventData)
        {
            if (_isShowing) return;

            ShowHint(eventData.DuckPosition, eventData.TargetBasketPosition);
        }

        private void OnDuckTouched(Services.DuckEvents.DuckTouchedEvent eventData)
        {
            HideHint();
        }

        private void OnDuckStartedDragging(Services.DuckEvents.DuckStartedDraggingEvent eventData)
        {
            HideHint();
        }

        private void ShowHint(Vector3 duckPosition, Vector3 basketPosition)
        {
            if (_isShowing) return;

            _isShowing = true;
            _currentAnimation?.Kill();

            // Position hand at duck position
            transform.position = duckPosition;

            _currentAnimation = DOTween.Sequence();

            // Fade in
            _currentAnimation.Append(_handImage.DOFade(1f, _fadeInDuration));

            // Animate drag motion multiple times
            for (int i = 0; i < _repeatCount; i++)
            {
                _currentAnimation.Append(AnimateDragMotion(duckPosition, basketPosition));
                _currentAnimation.AppendInterval(0.5f); // Small pause between repeats
            }

            // Fade out
            _currentAnimation.Append(_handImage.DOFade(0f, _fadeOutDuration));
            _currentAnimation.OnComplete(() =>
            {
                _isShowing = false;
                HideImmediate();
            });
        }

        private Tween AnimateDragMotion(Vector3 startPos, Vector3 endPos)
        {
            var sequence = DOTween.Sequence();

            // Move to start position
            sequence.Append(transform.DOMove(startPos, 0.2f));

            // Scale down slightly (press effect)
            sequence.Join(transform.DOScale(Vector3.one * 0.9f, 0.2f));

            // Move to end position with curve
            sequence.Append(transform.DOMove(endPos, _animationDuration)
                .SetEase(_movementCurve));

            // Scale back up (release effect)
            sequence.Join(transform.DOScale(Vector3.one, _animationDuration));

            // Small bounce at the end
            sequence.Append(transform.DOPunchScale(Vector3.one * 0.1f, 0.3f, 2));

            return sequence;
        }

        public void HideHint()
        {
            if (!_isShowing) return;

            _currentAnimation?.Kill();

            _handImage.DOFade(0f, _fadeOutDuration * 0.5f)
                .OnComplete(() =>
                {
                    _isShowing = false;
                    HideImmediate();
                });
        }

        private void HideImmediate()
        {
            if (_handImage != null)
            {
                var color = _handImage.color;
                color.a = 0f;
                _handImage.color = color;
            }

            transform.localScale = Vector3.one;
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }
    }
}