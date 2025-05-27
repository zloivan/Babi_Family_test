using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Game.Scripts.DuckGame.Services
{
    public class DuckSpawnService : IDisposable
    {
        public async UniTask InitializeAsync()
        {
            Debug.Log("[DuckSpawnService] Initializing...");
        }

        public async UniTask AnimateDucksSwimmingIn(DuckData[] ducks)
        {
            var duckComponents = Object.FindObjectsByType<Duck>(FindObjectsSortMode.None);
            
            for (int i = 0; i < ducks.Length && i < duckComponents.Length; i++)
            {
                var duck = duckComponents[i];
                var duckData = ducks[i];
                
                // Set duck color and position
                duck.Initialize(duckData.color, duckData.startPosition);
                
                // Animate swimming in (simple movement)
                await AnimateDuckSwimIn(duck, duckData.floatingPosition);
                
                // Small delay between ducks
                await UniTask.Delay(200);
            }
        }

        private async UniTask AnimateDuckSwimIn(Duck duck, Vector3 targetPosition)
        {
            var startPos = duck.transform.position;
            var duration = 2f;
            var elapsedTime = 0f;
            
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                var progress = elapsedTime / duration;
                
                // Simple lerp with some wave motion for "swimming"
                var currentPos = Vector3.Lerp(startPos, targetPosition, progress);
                currentPos.y += Mathf.Sin(progress * Mathf.PI * 4) * 10f; // Wave motion
                
                duck.transform.position = currentPos;
                await UniTask.Yield();
            }
            
            duck.transform.position = targetPosition;
        }

        public void Dispose()
        {
            // Cleanup if needed
        }
    }
}