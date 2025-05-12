using System.Threading;
using _Game.Scripts.Core.Services;
using BabiFamily.DuckGame.Events;
using BabiFamily.DuckGame.Models;
using Cysharp.Threading.Tasks;
using IKhom.EventBusSystem.Runtime;
using IKhom.ServiceLocatorSystem.Runtime;
using UnityEngine;

namespace BabiFamily.DuckGame.Utils
{
    public class HintSystem : MonoBehaviour
    {
        [SerializeField] private GameObject _handCursor;
        [SerializeField] private float _moveDuration = 1.5f;
        [SerializeField] private AnimationCurve _moveCurve;
        
        private CancellationTokenSource _cts;
        private IAssetProviderService _assetProvider;
        private DuckGameModel _gameModel;
        
        private void Awake()
        {
            _handCursor.SetActive(false);
            _cts = new CancellationTokenSource();
            
            // Получаем сервисы из ServiceLocator
            _assetProvider = ServiceLocator.Global.Get<IAssetProviderService>();
            
            // Получаем модель игры через ServiceLocator
            _gameModel = ServiceLocator.Global.Get<DuckGameModel>();
            
            RegisterEvents();
        }
        
        private void OnDestroy()
        {
            _cts.Cancel();
            _cts.Dispose();
            UnregisterEvents();
        }
        
        private void RegisterEvents()
        {
            EventBus<ShowHintEvent>.Register(new EventBinding<ShowHintEvent>(OnShowHint));
            EventBus<GameStartEvent>.Register(new EventBinding<GameStartEvent>(OnGameStart));
        }
        
        private void UnregisterEvents()
        {
            // Отписываемся от событий
        }
        
        private void OnGameStart(GameStartEvent evt)
        {
            // Если показывалась подсказка, останавливаем ее
            StopHint();
        }
        
        private void OnShowHint(ShowHintEvent evt)
        {
            // Если уже показывается подсказка, останавливаем ее
            StopHint();
            
            // Получаем информацию об утке и корзине из модели
            if (_gameModel.TryGetDuckAndBasketPositions(evt.DuckIndex, out Vector3 duckPosition, out Vector3 basketPosition))
            {
                // Показываем подсказку
                ShowHintAsync(duckPosition, basketPosition, _cts.Token).Forget();
            }
        }
        
        private void StopHint()
        {
            _cts.Cancel();
            _cts = new CancellationTokenSource();
            _handCursor.SetActive(false);
        }
        
        private async UniTask ShowHintAsync(Vector3 startPos, Vector3 endPos, CancellationToken cancellationToken)
        {
            _handCursor.SetActive(true);
            _handCursor.transform.position = startPos;
            
            // Повторяем анимацию 3 раза
            for (int i = 0; i < 3; i++)
            {
                if (cancellationToken.IsCancellationRequested) return;
                
                float elapsedTime = 0f;
                
                while (elapsedTime < _moveDuration)
                {
                    if (cancellationToken.IsCancellationRequested) return;
                    
                    elapsedTime += Time.deltaTime;
                    float t = elapsedTime / _moveDuration;
                    float curvedT = _moveCurve.Evaluate(t);
                    
                    _handCursor.transform.position = Vector3.Lerp(startPos, endPos, curvedT);
                    
                    await UniTask.Yield(cancellationToken);
                }
                
                await UniTask.Delay(500, cancellationToken: cancellationToken);
            }
            
            _handCursor.SetActive(false);
        }
    }
}