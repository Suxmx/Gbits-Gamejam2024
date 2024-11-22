using System.Threading.Tasks;
using GameFramework.Resource;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public static class ResourceExtension
    {
        public static Task<T> LoadAssetAsync<T>(this ResourceComponent resourceComponent, string assetPath)
            where T : class
        {
            var taskCompletionSource = new TaskCompletionSource<T>();

            resourceComponent.LoadAsset(assetPath, new LoadAssetCallbacks(
                (assetName, asset, duration, userData) =>
                {
                    // 成功加载，设置Task结果
                    taskCompletionSource.SetResult(asset as T);
                },
                (assetName, status, errorMessage, userData) =>
                {
                    // 加载失败，设置Task异常
                    taskCompletionSource.SetException(
                        new System.Exception($"Failed to load asset '{assetName}': {errorMessage}"));
                }));

            return taskCompletionSource.Task;
        }
    }
}