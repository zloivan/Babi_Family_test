using System;
using Cysharp.Threading.Tasks;
using IKhom.EventBusSystem.Runtime;
using UnityEngine;

namespace _Game.Scripts.DuckGame.Services
{
    public class DragDropService : IDisposable
    {
        private Camera _uiCamera;
        private Canvas _gameCanvas;
        private bool _isDragging = false;
        private GameObject _draggedObject;
        private Vector3 _dragOffset;

        public bool IsDragging => _isDragging;

        public async UniTask InitializeAsync()
        {
            Debug.Log("[DragDropService] Initializing...");
            
            // Find the main camera and canvas
            _uiCamera = Camera.main;
            _gameCanvas = GameObject.FindFirstObjectByType<Canvas>();
            
            if (_gameCanvas == null)
            {
                Debug.LogError("[DragDropService] No Canvas found in scene!");
            }
        }

        public void Update()
        {
            HandleInput();
        }

        private void HandleInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnPointerDown(Input.mousePosition);
            }
            else if (Input.GetMouseButton(0) && _isDragging)
            {
                OnPointerDrag(Input.mousePosition);
            }
            else if (Input.GetMouseButtonUp(0) && _isDragging)
            {
                OnPointerUp(Input.mousePosition);
            }
        }

        private void OnPointerDown(Vector3 screenPosition)
        {
            var worldPosition = ScreenToCanvasPosition(screenPosition);
            var hitObject = GetObjectAtPosition(worldPosition);
            
            if (hitObject != null && hitObject.GetComponent<Duck>() != null)
            {
                StartDragging(hitObject, worldPosition);
            }
        }

        private void OnPointerDrag(Vector3 screenPosition)
        {
            if (!_isDragging || _draggedObject == null) return;
            
            var worldPosition = ScreenToCanvasPosition(screenPosition);
            _draggedObject.transform.position = worldPosition + _dragOffset;
        }

        private void OnPointerUp(Vector3 screenPosition)
        {
            if (!_isDragging || _draggedObject == null) return;
            
            var worldPosition = ScreenToCanvasPosition(screenPosition);
            var dropTarget = GetBasketAtPosition(worldPosition);
            
            StopDragging(dropTarget);
        }

        private void StartDragging(GameObject obj, Vector3 startPosition)
        {
            _isDragging = true;
            _draggedObject = obj;
            _dragOffset = obj.transform.position - startPosition;
            
            var duck = obj.GetComponent<Duck>();
            if (duck != null)
            {
                duck.OnDragStart();
                
                EventBus<DuckEvents.DuckStartedDraggingEvent>.Raise(new DuckEvents.DuckStartedDraggingEvent
                {
                    DuckColor = duck.Color
                });
            }
        }

        private void StopDragging(GameObject dropTarget)
        {
            var duck = _draggedObject.GetComponent<Duck>();
            if (duck != null)
            {
                duck.OnDragEnd(dropTarget);
            }
            
            _isDragging = false;
            _draggedObject = null;
            _dragOffset = Vector3.zero;
        }

        private Vector3 ScreenToCanvasPosition(Vector3 screenPosition)
        {
            if (_gameCanvas == null) return screenPosition;
            
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _gameCanvas.transform as RectTransform,
                screenPosition,
                _uiCamera,
                out Vector2 localPoint);
                
            return localPoint;
        }

        private GameObject GetObjectAtPosition(Vector3 position)
        {
            // Simple distance-based detection for UI elements
            var ducks = GameObject.FindObjectsByType<Duck>(FindObjectsSortMode.None);
            
            foreach (var duck in ducks)
            {
                var distance = Vector3.Distance(duck.transform.position, position);
                if (distance < 100f) // Adjust based on duck size
                {
                    return duck.gameObject;
                }
            }
            
            return null;
        }

        private GameObject GetBasketAtPosition(Vector3 position)
        {
            var baskets = GameObject.FindObjectsByType<Basket>(FindObjectsSortMode.None);
            
            foreach (var basket in baskets)
            {
                var distance = Vector3.Distance(basket.transform.position, position);
                if (distance < 150f) // Adjust based on basket size
                {
                    return basket.gameObject;
                }
            }
            
            return null;
        }

        public void Dispose()
        {
            _isDragging = false;
            _draggedObject = null;
        }
    }
}