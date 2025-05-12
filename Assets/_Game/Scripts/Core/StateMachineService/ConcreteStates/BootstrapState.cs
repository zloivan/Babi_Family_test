using System.Threading;
using Cysharp.Threading.Tasks;
using IKhom.StateMachineSystem.Runtime.abstractions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Game.Scripts.Core.StateMachineService.ConcreteStates
{
    public class BootstrapState : IState<AppState>
    {
        
        public async UniTask EnterAsync(CancellationToken cancellationToken = default)
        {
            Debug.Log("Entering Bootstrap State");
            
            //setup service locator and all services within it.
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