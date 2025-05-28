using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Game.Scripts.DuckGame
{
    public class Basket : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private DuckColor _paintColor;
        [SerializeField] private Image _basketImage;
        [SerializeField] private Image _highlightImage;
        [SerializeField] private float _highlightDuration = 0.3f;
        [SerializeField] private float _successScaleDuration = 0.5f;
        
        private Color _originalColor;
        private Color _highlightColor;
        private bool _isHighlighted = false;
        
        public DuckColor PaintColor => _paintColor;
        
        private void Awake()
        {
            if (_basketImage != null)
            {
                _originalColor = _basketImage.color;
                SetBasketColor();
            }
            
            if (_highlightImage != null)
            {
                _highlightColor = Color.white;
                _highlightImage.color = new Color(_highlightColor.r, _highlightColor.g, _highlightColor.b, 0f);
            }
        }
        
        private void SetBasketColor()
        {
            if (_basketImage == null) return;
            
            var baseColor = _paintColor switch
            {
                DuckColor.Yellow => Color.yellow,
                DuckColor.White => Color.white,
                DuckColor.Blue => Color.blue,
                _ => Color.white
            };
            
            // Make basket color slightly darker than duck color
            _basketImage.color = baseColor * 0.8f;
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            // Only highlight if a duck is being dragged
            var draggedObject = eventData.pointerDrag;
            if (draggedObject != null && draggedObject.GetComponent<Duck>() != null)
            {
                var duck = draggedObject.GetComponent<Duck>();
                if (!duck.IsPlaced)
                {
                    ShowHighlight(duck.Color == _paintColor);
                }
            }
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            HideHighlight();
        }
        
        private void ShowHighlight(bool isCorrectColor)
        {
            if (_isHighlighted) return;
            
            _isHighlighted = true;
            
            if (_highlightImage != null)
            {
                var targetColor = isCorrectColor ? Color.green : Color.red;
                targetColor.a = 0.5f;
                
                _highlightImage.DOColor(targetColor, _highlightDuration);
            }
            
            // Slight scale up
            transform.DOScale(transform.localScale * 1.1f, _highlightDuration);
        }
        
        private void HideHighlight()
        {
            if (!_isHighlighted) return;
            
            _isHighlighted = false;
            
            if (_highlightImage != null)
            {
                var transparentColor = _highlightColor;
                transparentColor.a = 0f;
                _highlightImage.DOColor(transparentColor, _highlightDuration);
            }
            
            // Scale back to normal
            transform.DOScale(Vector3.one, _highlightDuration);
        }
        
        public void PlaySuccessAnimation()
        {
            // Success animation when duck is placed correctly
            var sequence = DOTween.Sequence();
            sequence.Append(transform.DOScale(Vector3.one * 1.2f, _successScaleDuration * 0.5f))
                   .Append(transform.DOScale(Vector3.one, _successScaleDuration * 0.5f))
                   .SetEase(Ease.OutBounce);
        }
        
        public void Initialize(DuckColor color, Vector3 position)
        {
            _paintColor = color;
            transform.position = position;
            SetBasketColor();
        }
    }
}