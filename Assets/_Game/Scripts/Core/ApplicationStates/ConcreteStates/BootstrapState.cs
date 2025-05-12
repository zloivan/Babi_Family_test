using System.Threading;
using Cysharp.Threading.Tasks;
using IKhom.StateMachineSystem.Runtime.abstractions;
using UnityEngine;

namespace _Game.Scripts.Core.StateMachineService.ConcreteStates
{
    public class BootstrapState : IState<AppState>
    {
        private readonly IStateContext<AppState> _context;

        public BootstrapState(IStateContext<AppState> context)
        {
            _context = context;
        }

        public UniTask EnterAsync(CancellationToken cancellationToken = default)
        {
            Debug.Log("Entering Bootstrap State");

            _context.ChangeState(AppState.MainMenu);
            return UniTask.CompletedTask;
        }

        public UniTask UpdateAsync(CancellationToken cancellationToken = default)
        {
            return UniTask.CompletedTask;
        }

        public UniTask ExitAsync(CancellationToken cancellationToken = default)
        {
            Debug.Log("Exiting Bootstrap State");
            return UniTask.CompletedTask;
        }
    }
}