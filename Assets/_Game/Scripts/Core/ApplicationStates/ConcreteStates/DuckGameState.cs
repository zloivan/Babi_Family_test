using System.Threading;
using Cysharp.Threading.Tasks;
using IKhom.StateMachineSystem.Runtime.abstractions;
using UnityEngine;

namespace _Game.Scripts.Core.StateMachineService.ConcreteStates
{
    public class DuckGameState : IState<AppState>
    {
        private readonly IStateContext<AppState> _context;

        
        public DuckGameState(IStateContext<AppState> context)
        {
            _context = context;
        }

        public async UniTask EnterAsync(CancellationToken cancellationToken = default)
        {
            Debug.Log("Entering Duck Game State");
            
        }

        public UniTask UpdateAsync(CancellationToken cancellationToken = default)
        {
            return UniTask.CompletedTask;
        }

        public UniTask ExitAsync(CancellationToken cancellationToken = default)
        {
            Debug.Log("Exiting Duck Game State");
            
            return UniTask.CompletedTask;
        }

        // Public methods for game events
        public void OnGameComplete()
        {
            _context.ChangeState(AppState.MainMenu);
        }
        
        public void OnExitGame()
        {
            _context.ChangeState(AppState.MainMenu);
        }
    }
}