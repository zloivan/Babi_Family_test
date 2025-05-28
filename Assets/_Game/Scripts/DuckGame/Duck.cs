using DG.Tweening;
using IKhom.EventBusSystem.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Game.Scripts.DuckGame
{
    public class Duck : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler,
        IEndDragHandler
    {
        [SerializeField] private Image _duckImage;
        [SerializeField] private float _scaleTransitionSpeed = 0.2f;
        [SerializeField] private float _jumpDuration = 0.5f;
        [SerializeField] private float _jumpHeight = 100f;

        private DuckColor _color;
        private Vector3 _originalPosition;
        private Vector3 _originalScale;
        private bool _isDragging = false;
        private bool _isPlaced = false;
        private Canvas _parentCanvas;
        private GraphicRaycaster _raycaster;

        public DuckColor Color => _color;
        public bool IsPlaced => _isPlaced;

        private void Awake()
        {
            _originalScale = transform.localScale;
            _parentCanvas = GetComponentInParent<Canvas>();
            _raycaster = _parentCanvas.GetComponent<GraphicRaycaster>();
        }

        public void Initialize(DuckColor color, Vector3 startPosition)
        {
            _color = color;
            _originalPosition = startPosition;
            transform.position = startPosition;

            // Set duck color based on enum
            SetDuckColor(color);

            _isPlaced = false;
        }

        private void SetDuckColor(DuckColor color)
        {
            if (_duckImage == null) return;

            _duckImage.color = color switch
            {
                DuckColor.Yellow => UnityEngine.Color.yellow,
                DuckColor.White => UnityEngine.Color.white,
                DuckColor.Blue => UnityEngine.Color.blue,
                _ => UnityEngine.Color.white
            };
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_isPlaced) return;

            // Scale up and fire touch event
            transform.DOScale(_originalScale * 1.1f, _scaleTransitionSpeed);

            EventBus<Services.DuckEvents.DuckTouchedEvent>.Raise(new Services.DuckEvents.DuckTouchedEvent
            {
                DuckColor = _color,
                Position = transform.position
            });
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_isPlaced || _isDragging) return;

            // Scale back to normal if not dragging
            transform.DOScale(_originalScale, _scaleTransitionSpeed);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_isPlaced) return;

            _isDragging = true;

            EventBus<Services.DuckEvents.DuckStartedDraggingEvent>.Raise(
                new Services.DuckEvents.DuckStartedDraggingEvent
                {
                    DuckColor = _color
                });
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_isPlaced || !_isDragging) return;

            // Convert screen position to canvas position
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _parentCanvas.transform as RectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 localPoint);

            transform.position = localPoint;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_isPlaced || !_isDragging) return;

            _isDragging = false;

            // Check what we dropped on
            var dropTarget = GetDropTarget(eventData);

            if (dropTarget != null && dropTarget.GetComponent<Basket>() != null)
            {
                var basket = dropTarget.GetComponent<Basket>();
                if (basket.PaintColor == _color)
                {
                    // Correct placement
                    PlaceInBasket(basket);
                }
                else
                {
                    // Wrong basket
                    ReturnToOriginalPosition();

                    EventBus<Services.DuckEvents.DuckPlacedIncorrectlyEvent>.Raise(
                        new Services.DuckEvents.DuckPlacedIncorrectlyEvent
                        {
                            DuckColor = _color,
                            AttemptedPosition = basket.transform.position
                        });
                }
            }
            else
            {
                // Dropped on nothing
                ReturnToOriginalPosition();
            }
        }

        private GameObject GetDropTarget(PointerEventData eventData)
        {
            var results = new System.Collections.Generic.List<RaycastResult>();
            _raycaster.Raycast(eventData, results);

            foreach (var result in results)
            {
                if (result.gameObject != gameObject && result.gameObject.GetComponent<Basket>() != null)
                {
                    return result.gameObject;
                }
            }

            return null;
        }

        private void PlaceInBasket(Basket basket)
        {
            _isPlaced = true;

            // Jump animation into basket
            var sequence = DOTween.Sequence();
            var targetPosition = basket.transform.position;

            sequence.Append(transform.DOJump(targetPosition, _jumpHeight, 1, _jumpDuration))
                .Join(transform.DOScale(_originalScale * 0.8f, _jumpDuration))
                .OnComplete(() =>
                {
                    EventBus<Services.DuckEvents.DuckPlacedCorrectlyEvent>.Raise(
                        new Services.DuckEvents.DuckPlacedCorrectlyEvent
                        {
                            DuckColor = _color,
                            BasketPosition = basket.transform.position
                        });
                });
        }

        private void ReturnToOriginalPosition()
        {
            var sequence = DOTween.Sequence();
            sequence.Append(transform.DOMove(_originalPosition, 0.3f))
                .Join(transform.DOScale(_originalScale, 0.3f));
        }
    }
}