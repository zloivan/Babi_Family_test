using _Game.Scripts.Core.StateMachineService;
using Cysharp.Threading.Tasks;
using IKhom.SoundSystem.Runtime.components;
using IKhom.StateMachineSystem.Runtime;
using UnityEngine;

namespace _Game.Scripts.Core.Services
{
    public interface ISoundService
    {
        SoundManager SoundManager { get; }
    }
    
    public interface ISceneLoaderService
    {
        UniTask LoadSceneAsync(string sceneName);
        UniTask UnloadSceneAsync(string mainmenu);
    }
    
    public interface IStateMachineService
    {
        StateMachine<AppState> StateMachine { get; }
        void ChangeState(AppState newState);
    }
    
    public interface IAssetProviderService
    {
        T LoadAsset<T>(string path) where T : Object;
        T[] LoadAllAssets<T>(string folderPath) where T : Object;
    }
    
    public interface IInputService
    {
        Vector3 GetPointerWorldPosition(Camera camera);
        bool IsPointerDown();
        bool IsPointerUp();
    }
}