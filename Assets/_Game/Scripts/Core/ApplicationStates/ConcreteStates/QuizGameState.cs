using System.Threading;
using Cysharp.Threading.Tasks;
using IKhom.StateMachineSystem.Runtime.abstractions;
using UnityEngine;

namespace _Game.Scripts.Core.StateMachineService.ConcreteStates
{
    public class QuizGameState : IState<AppState>
    {
        private readonly IStateContext<AppState> _context;

        public QuizGameState(IStateContext<AppState> context)
        {
            _context = context;
        }

        public async UniTask EnterAsync(CancellationToken cancellationToken = default)
        {
            Debug.Log("Entering Quiz Game State");

            // Initialize the Quiz game
            // Load necessary assets
            // Set up game scene/UI
        }

        public UniTask UpdateAsync(CancellationToken cancellationToken = default)
        {
            // Update logic for the Quiz game if needed
            return UniTask.CompletedTask;
        }

        public UniTask ExitAsync(CancellationToken cancellationToken = default)
        {
            Debug.Log("Exiting Quiz Game State");

            // Clean up resources
            // Hide game UI
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