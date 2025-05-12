using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Game.Scripts.DuckGame
{
    public class Duck : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private float _scaleTransitionSpeed;


        public void OnPointerDown(PointerEventData eventData)
        {
            transform.DOScale(1.2f, _scaleTransitionSpeed);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            transform.DOScale(0, _scaleTransitionSpeed);
        }
    }
}