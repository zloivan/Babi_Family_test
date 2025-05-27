using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Game.Scripts.DuckGame.Services
{
    public class ParticleService : IDisposable
    {
        private ParticleSystem _basketAppearEffect;
        private ParticleSystem _successEffect;
        private ParticleSystem _roundCompleteEffect;

        public async UniTask InitializeAsync()
        {
            Debug.Log("[ParticleService] Initializing...");
            
            // Find particle systems in the scene
            var particles = Object.FindObjectsByType<ParticleSystem>(FindObjectsSortMode.None);
            
            foreach (var ps in particles)
            {
                if (ps.name.Contains("BasketAppear"))
                    _basketAppearEffect = ps;
                else if (ps.name.Contains("Success"))
                    _successEffect = ps;
                else if (ps.name.Contains("RoundComplete"))
                    _roundCompleteEffect = ps;
            }
        }

        public async UniTask PlayBasketAppearEffect(Vector3 position)
        {
            if (_basketAppearEffect != null)
            {
                _basketAppearEffect.transform.position = position;
                _basketAppearEffect.Play();
            }
            
            await UniTask.Delay(100); // Small delay
        }

        public async UniTask PlaySuccessEffect(Vector3 position)
        {
            if (_successEffect != null)
            {
                _successEffect.transform.position = position;
                _successEffect.Play();
            }
            
            await UniTask.Delay(500); // Wait for effect
        }

        public async UniTask PlayRoundCompleteEffect()
        {
            if (_roundCompleteEffect != null)
            {
                _roundCompleteEffect.Play();
            }
            
            await UniTask.Delay(1000); // Wait for big effect
        }

        public void Dispose()
        {
            // Stop all effects
            if (_basketAppearEffect != null) _basketAppearEffect.Stop();
            if (_successEffect != null) _successEffect.Stop();
            if (_roundCompleteEffect != null) _roundCompleteEffect.Stop();
        }
    }
}