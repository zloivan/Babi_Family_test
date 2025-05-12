using System.Threading;
using Cysharp.Threading.Tasks;
using IKhom.StateMachineSystem.Runtime.abstractions;

namespace _Game.Scripts.Core.StateMachineService.ConcreteStates
{
    public class QuizGameState: IState<AppState>
    {
        public UniTask EnterAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            throw new System.NotImplementedException();
        }

        public UniTask UpdateAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            throw new System.NotImplementedException();
        }

        public UniTask ExitAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            throw new System.NotImplementedException();
        }
    }
}