using System.Threading;
using _Game.Scripts.Core.Services;
using Cysharp.Threading.Tasks;
using IKhom.ServiceLocatorSystem.Runtime;
using IKhom.StateMachineSystem.Runtime.abstractions;

namespace _Game.Scripts.Core.StateMachineService
{
    public class MainMenuState : IState<AppState>
    {
        ISceneLoaderService _sceneLoaderService;

        public async UniTask EnterAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            _sceneLoaderService = ServiceLocator.Global.Get<ISceneLoaderService>();
            await _sceneLoaderService.LoadSceneAsync("MainMenu");
        }

        public UniTask UpdateAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return UniTask.CompletedTask;
        }

        public UniTask ExitAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            if (_sceneLoaderService != null)
            {
                _sceneLoaderService.UnloadSceneAsync("MainMenu");
                _sceneLoaderService = null;
            }
            return UniTask.CompletedTask;
        }
    }
}