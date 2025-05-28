using System.Threading;
using _Game.Scripts.Core.StateMachineService;
using _Game.Scripts.DuckGame.Services;
using Cysharp.Threading.Tasks;
using IKhom.EventBusSystem.Runtime;
using IKhom.ServiceLocatorSystem.Runtime;
using IKhom.StateMachineSystem.Runtime.abstractions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Game.Scripts.DuckGame
{
    public class DuckGameState : IState<AppState>
    {
        private const string DUCK_SCENE_NAME = "Duck";
        private const string MAIN_SCENE_NAME = "Main";

        private DuckGameManager _gameManager;
        private ParticleService _particleService;
        private DuckSpawnService _spawnService;

        private IStateContext<AppState> _stateContext;
        private ServiceLocator _serviceLocator;

        public DuckGameState(IStateContext<AppState> stateContext)
        {
            _stateContext = stateContext;
        }

        public async UniTask EnterAsync(CancellationToken cancellationToken = default)
        {
            Debug.Log("[DuckGameState] Entering Duck Game");

            await SceneManager.LoadSceneAsync(DUCK_SCENE_NAME, LoadSceneMode.Single);

            _serviceLocator = ServiceLocator.Global;

            RegisterServices();
            await InitializeServices();
            SubscribeToEvents();
            await _gameManager.StartGameAsync();
        }

        public async UniTask UpdateAsync(CancellationToken cancellationToken = default)
        {
            _gameManager?.Update();
        }

        public async UniTask ExitAsync(CancellationToken cancellationToken = default)
        {
            Debug.Log("[DuckGameState] Exiting Duck Game");

            UnsubscribeFromEvents();
            CleanupServices();
            await SceneManager.LoadSceneAsync(MAIN_SCENE_NAME, LoadSceneMode.Single);
        }

        private void RegisterServices()
        {
            var gameConfig = Object.FindFirstObjectByType<DuckGameConfigProvider>()?.Config;
            if (gameConfig == null)
            {
                Debug.LogError("[DuckGameState] No GameConfig found in scene!");
                return;
            }

            _gameManager = new DuckGameManager(gameConfig);
            _particleService = new ParticleService();
            _spawnService = new DuckSpawnService();

            _serviceLocator.Register(_gameManager);
            _serviceLocator.Register(_particleService);
            _serviceLocator.Register(_spawnService);
        }

        private async UniTask InitializeServices()
        {
            await _gameManager.InitializeAsync();
            await _particleService.InitializeAsync();
            await _spawnService.InitializeAsync();
        }

        private void SubscribeToEvents()
        {
            EventBus<DuckEvents.GameCompleteEvent>.Register(
                new EventBinding<DuckEvents.GameCompleteEvent>(OnGameComplete));
        }

        private void UnsubscribeFromEvents()
        {
        }

        private void OnGameComplete(DuckEvents.GameCompleteEvent gameCompleteEvent)
        {
            Debug.Log($"[DuckGameState] Game completed after {gameCompleteEvent.TotalRounds} rounds!");
            ReturnToMainMenuAsync().Forget();
        }

        private async UniTaskVoid ReturnToMainMenuAsync()
        {
            await UniTask.Delay(2000);
            _stateContext.ChangeState(AppState.MainMenu);
        }

        private void CleanupServices()
        {
            _gameManager?.Dispose();
            _particleService?.Dispose();
            _spawnService?.Dispose();
        }
    }
}