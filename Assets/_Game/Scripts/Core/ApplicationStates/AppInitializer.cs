using _Game.Scripts.Core.StateMachineService;
using _Game.Scripts.Core.StateMachineService.ConcreteStates;
using IKhom.ServiceLocatorSystem.Runtime;
using IKhom.StateMachineSystem.Runtime;
using UnityEngine;

namespace BabiFamily
{
    public class AppInitializer : MonoBehaviour
    {
        [SerializeField] private ServiceLocatorGlobal _serviceLocator;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            InitializeStateMachine();
        }

        private static void InitializeStateMachine()
        {
            var appStateMachine = new StateMachine<AppState>();

            appStateMachine.AddStateWithContext(AppState.Bootstrap, ctx => new BootstrapState(ctx));
            appStateMachine.AddStateWithContext(AppState.MainMenu, ctx => new MainMenuState(ctx));
            appStateMachine.AddStateWithContext(AppState.DuckGame, ctx => new DuckGameState(ctx));
            appStateMachine.AddStateWithContext(AppState.QuizGame, ctx => new QuizGameState(ctx));

            ServiceLocator.Global.Register(appStateMachine);

            appStateMachine.SetInitialState(AppState.Bootstrap);
        }
    }
}