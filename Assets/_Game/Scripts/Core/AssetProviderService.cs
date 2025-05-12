using _Game.Scripts.Core.Services;
using UnityEngine;

public class AssetProviderService : IAssetProviderService
{
    public T LoadAsset<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }

    public T[] LoadAllAssets<T>(string folderPath) where T : Object
    {
        return Resources.LoadAll<T>(folderPath);
    }
}