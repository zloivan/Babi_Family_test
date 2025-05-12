using System.Threading;
using BabiFamily.DuckGame.Events;
using Cysharp.Threading.Tasks;
using IKhom.EventBusSystem.Runtime;
using IKhom.StateMachineSystem.Runtime.abstractions;
using UnityEngine;

namespace BabiFamily.DuckGame.States
{
    public enum GameState
    {
        None,
        Start,
        SpawnDucks,
        Gameplay,
        LevelCompleted
    }
    
    public class GameStartState : IState<GameState>
    {
        public async UniTask EnterAsync(CancellationToken cancellationToken = default)
        {
            // Инициализация игры
            EventBus<GameStartEvent>.Raise(new GameStartEvent());
            
            // Задержка для эффекта
            await UniTask.Delay(500, cancellationToken: cancellationToken);
        }
        
        public UniTask UpdateAsync(CancellationToken cancellationToken = default)
        {
            return UniTask.CompletedTask;
        }
        
        public UniTask ExitAsync(CancellationToken cancellationToken = default)
        {
            return UniTask.CompletedTask;
        }
    }
    
    public class DuckSpawnState : IState<GameState>
    {
        private readonly int _duckCount;
        
        public DuckSpawnState(int duckCount)
        {
            _duckCount = duckCount;
        }
        
        public async UniTask EnterAsync(CancellationToken cancellationToken = default)
        {
            // Создание корзин с эффектами
            // Затем спавн уток
            
            // Задержка между появлением каждой утки
            for (int i = 0; i < _duckCount; i++)
            {
                // Здесь был бы код создания утки
                
                await UniTask.Delay(500, cancellationToken: cancellationToken);
            }
        }
        
        public UniTask UpdateAsync(CancellationToken cancellationToken = default)
        {
            return UniTask.CompletedTask;
        }
        
        public UniTask ExitAsync(CancellationToken cancellationToken = default)
        {
            return UniTask.CompletedTask;
        }
    }
    
    public class GameplayState : IState<GameState>
    {
        private float _idleTime = 0f;
        private const float HINT_TIME = 8f; // 8 секунд бездействия для подсказки
        
        public UniTask EnterAsync(CancellationToken cancellationToken = default)
        {
            // Инициализация геймплея
            _idleTime = 0f;
            return UniTask.CompletedTask;
        }
        
        public async UniTask UpdateAsync(CancellationToken cancellationToken = default)
        {
            // Проверка времени бездействия для запуска подсказки
            if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
            {
                _idleTime = 0f;
            }
            else
            {
                _idleTime += UnityEngine.Time.deltaTime;
                
                if (_idleTime >= HINT_TIME)
                {
                    // Запуск подсказки
                    ShowHint();
                    _idleTime = 0f;
                }
            }
            
            await UniTask.Yield(cancellationToken);
        }
        
        private void ShowHint()
        {
            // Выбор случайной утки, которая еще не помещена в корзину
            int randomDuckIndex = 0; // В реальном проекте здесь был бы код выбора случайной утки
            
            EventBus<ShowHintEvent>.Raise(new ShowHintEvent { DuckIndex = randomDuckIndex });
        }
        
        public UniTask ExitAsync(CancellationToken cancellationToken = default)
        {
            return UniTask.CompletedTask;
        }
    }
    
    public class LevelCompletedState : IState<GameState>
    {
        private readonly int _levelIndex;
        private readonly int _maxLevels;
        
        public LevelCompletedState(int levelIndex, int maxLevels)
        {
            _levelIndex = levelIndex;
            _maxLevels = maxLevels;
        }
        
        public async UniTask EnterAsync(CancellationToken cancellationToken = default)
        {
            // Сообщаем о завершении уровня
            EventBus<LevelCompleteEvent>.Raise(new LevelCompleteEvent { LevelIndex = _levelIndex });
            
            // Обновляем прогресс
            EventBus<ProgressUpdateEvent>.Raise(new ProgressUpdateEvent 
            { 
                CurrentProgress = _levelIndex + 1,
                MaxProgress = _maxLevels
            });
            
            // Задержка для эффектов
            await UniTask.Delay(2000, cancellationToken: cancellationToken);
            
            // Проверяем, все ли уровни пройдены
            if (_levelIndex >= _maxLevels - 1)
            {
                // Игра завершена
                EventBus<GameEndEvent>.Raise(new GameEndEvent { Success = true });
            }
        }
        
        public UniTask UpdateAsync(CancellationToken cancellationToken = default)
        {
            return UniTask.CompletedTask;
        }
        
        public UniTask ExitAsync(CancellationToken cancellationToken = default)
        {
            return UniTask.CompletedTask;
        }
    }
}