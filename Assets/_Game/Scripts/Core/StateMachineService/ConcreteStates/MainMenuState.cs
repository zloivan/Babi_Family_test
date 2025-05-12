using System.Threading;
using Cysharp.Threading.Tasks;
using IKhom.StateMachineSystem.Runtime.abstractions;

namespace _Game.Scripts.Core.StateMachineService.ConcreteStates
{
    public class MainMenuState : IState<AppState>
    {

        public  UniTask EnterAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return UniTask.CompletedTask;
        }

        public UniTask UpdateAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return UniTask.CompletedTask;
        }

        public UniTask ExitAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            
            return UniTask.CompletedTask;
        }
    }
}