using _Game.Scripts.Core.StateMachineService.ConcreteStates;
using IKhom.StateMachineSystem.Runtime;

namespace _Game.Scripts.Core.StateMachineService
{
    public class AppStateMachine
    {
        public StateMachine<AppState> StateMachine { get; }

        public AppStateMachine()
        {
            StateMachine = new StateMachine<AppState>();

            // Register states
            StateMachine.AddState<BootstrapState>(AppState.Bootstrap);
            StateMachine.AddState<MainMenuState>(AppState.MainMenu);
            StateMachine.AddState<DuckGameState>(AppState.DuckGame);
            StateMachine.AddState<QuizGameState>(AppState.QuizGame);
            
            //Register services in service locator
        }

        public void ChangeState(AppState newState)
        {
            StateMachine.ChangeState(newState);
        }
    }
}