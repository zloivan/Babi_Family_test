using _Game.Scripts.Core.Services;
using _Game.Scripts.Core.StateMachineService.ConcreteStates;
using IKhom.StateMachineSystem.Runtime;

namespace _Game.Scripts.Core.StateMachineService
{
    public class StateMachineService : IStateMachineService
    {
        public StateMachine<AppState> StateMachine { get; }

        public StateMachineService()
        {
            StateMachine = new StateMachine<AppState>();

            // Регистрируем состояния
            StateMachine.AddState<BootstrapState>(AppState.Bootstrap);
            StateMachine.AddState<MainMenuState>(AppState.MainMenu);
            StateMachine.AddState<DuckGameState>(AppState.DuckGame);
            StateMachine.AddState<QuizGameState>(AppState.QuizGame);

            // Добавляем переходы между состояниями
            StateMachine.AddTransition(AppState.Bootstrap, AppState.MainMenu, () => true);
        }

        public void ChangeState(AppState newState)
        {
            StateMachine.ChangeState(newState);
        }
    }
}