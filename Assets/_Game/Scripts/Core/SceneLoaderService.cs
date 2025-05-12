using _Game.Scripts.Core.Services;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

public class SceneLoaderService : ISceneLoaderService
{
    public async UniTask LoadSceneAsync(string sceneName)
    {
        await SceneManager.LoadSceneAsync(sceneName).ToUniTask();
    }

    public UniTask UnloadSceneAsync(string mainmenu)
    {
        return SceneManager.UnloadSceneAsync(mainmenu).ToUniTask();
    }
}