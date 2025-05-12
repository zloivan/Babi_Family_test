using System.Threading;
using Cysharp.Threading.Tasks;
using IKhom.StateMachineSystem.Runtime.abstractions;
using UnityEngine;

namespace _Game.Scripts.Core.StateMachineService.ConcreteStates
{
    public class MainMenuState : IState<AppState>
    {
        private readonly IStateContext<AppState> _context;

        public MainMenuState(IStateContext<AppState> ctx)
        {
            _context = ctx;
        }

        public async UniTask EnterAsync(CancellationToken cancellationToken = default)
        {
            Debug.Log("Entering Main Menu State");

            // Load main menu UI
            // Show main menu elements
        }

        public UniTask UpdateAsync(CancellationToken cancellationToken = default)
        {
            // Main menu update logic if needed
            return UniTask.CompletedTask;
        }

        public UniTask ExitAsync(CancellationToken cancellationToken = default)
        {
            Debug.Log("Exiting Main Menu State");

            // Hide main menu elements
            return UniTask.CompletedTask;
        }

        // Public methods for handling UI events
        public void OnDuckGameSelected()
        {
            _context.ChangeState(AppState.DuckGame);
        }

        public void OnQuizGameSelected()
        {
            _context.ChangeState(AppState.QuizGame);
        }
    }
}