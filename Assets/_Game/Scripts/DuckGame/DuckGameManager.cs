using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.DuckGame.Services;
using Cysharp.Threading.Tasks;
using IKhom.EventBusSystem.Runtime;
using IKhom.ServiceLocatorSystem.Runtime;
using IKhom.SoundSystem.Runtime.components;
using IKhom.SoundSystem.Runtime.data;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.DuckGame
{
    public class DuckGameManager : IDisposable
    {
        private readonly GameConfig _config;
        private int _currentRound = 1;
        private int _ducksPlacedInRound = 0;
        private List<DuckData> _currentDucks = new();
        private List<BasketData> _baskets = new();
        private bool _gameActive = false;
        private float _lastActivityTime;

        // Event bindings
        private EventBinding<DuckEvents.DuckPlacedCorrectlyEvent> _duckPlacedBinding;
        private EventBinding<DuckEvents.DuckTouchedEvent> _duckTouchedBinding;

        public bool IsGameActive => _gameActive;
        public int CurrentRound => _currentRound;
        public int TotalRounds => _config.totalRounds;

        public DuckGameManager(GameConfig config)
        {
            _config = config;
            SetupEventBindings();
        }

        public async UniTask InitializeAsync()
        {
            Debug.Log("[DuckGameManager] Initializing...");
            SetupBaskets();
            ResetInactivityTimer();
        }

        public async UniTask StartGameAsync()
        {
            Debug.Log("[DuckGameManager] Starting game...");
            _gameActive = true;
            
            // Start background music
            var soundManager = ServiceLocator.Global.Get<SoundManager>();
            if (soundManager != null)
            {
                soundManager.CreateSound()
                    .WithSoundData(GetSoundData(_config.backgroundMusicId))
                    .Play();
            }

            await StartNewRound();
        }

        public void Update()
        {
            if (!_gameActive) return;

            CheckForInactivity();
        }

        private async UniTask StartNewRound()
        {
            Debug.Log($"[DuckGameManager] Starting round {_currentRound}");
            
            _ducksPlacedInRound = 0;
            GenerateRoundDucks();
            
            // Spawn baskets with particles
            await SpawnBaskets();
            
            // Wait a bit, then spawn ducks
            await UniTask.Delay(500);
            await SpawnDucks();
            
            ResetInactivityTimer();
        }

        private void GenerateRoundDucks()
        {
            _currentDucks.Clear();
            
            // Create 3 ducks with random colors (one of each type)
            var availableColors = new List<DuckColor> { DuckColor.Yellow, DuckColor.White, DuckColor.Blue };
            availableColors = availableColors.OrderBy(x => UnityEngine.Random.value).ToList();

            for (int i = 0; i < _config.ducksPerRound; i++)
            {
                var duckData = new DuckData
                {
                    color = availableColors[i],
                    startPosition = _config.duckSpawnPositions[i],
                    floatingPosition = _config.duckSpawnPositions[i] + Vector3.up * 50f, // Slight float
                    isPlaced = false
                };
                _currentDucks.Add(duckData);
            }
        }

        private void SetupBaskets()
        {
            _baskets.Clear();
            var colors = new[] { DuckColor.Yellow, DuckColor.White, DuckColor.Blue };
            
            for (int i = 0; i < colors.Length; i++)
            {
                var basketData = new BasketData
                {
                    color = colors[i],
                    position = _config.basketPositions[i],
                    hasParticleEffect = true
                };
                _baskets.Add(basketData);
            }
        }

        private async UniTask SpawnBaskets()
        {
            var particleService = ServiceLocator.Global.Get<ParticleService>();
            
            EventBus<DuckEvents.BasketsSpawnedEvent>.Raise(new DuckEvents.BasketsSpawnedEvent { RoundNumber = _currentRound });
            
            // Trigger particle effects for basket appearances
            if (particleService != null)
            {
                foreach (var basket in _baskets)
                {
                    await particleService.PlayBasketAppearEffect(basket.position);
                }
            }
        }

        private async UniTask SpawnDucks()
        {
            var spawnService = ServiceLocator.Global.Get<DuckSpawnService>();
            var soundManager = ServiceLocator.Global.Get<SoundManager>();
            
            EventBus<DuckEvents.DucksSpawnedEvent>.Raise(new DuckEvents.DucksSpawnedEvent 
            { 
                RoundNumber = _currentRound,
                DuckColors = _currentDucks.Select(d => d.color).ToArray()
            });

            // Play duck sound when they appear
            if (soundManager != null)
            {
                soundManager.CreateSound()
                    .WithSoundData(GetSoundData(_config.duckSoundId))
                    .Play();
            }

            // Animate ducks swimming in
            if (spawnService != null)
            {
                await spawnService.AnimateDucksSwimmingIn(_currentDucks.ToArray());
            }
        }

        private void OnDuckPlacedCorrectly(DuckEvents.DuckPlacedCorrectlyEvent eventData)
        {
            Debug.Log($"[DuckGameManager] Duck {eventData.DuckColor} placed correctly!");
            
            // Mark duck as placed
            var duck = _currentDucks.FirstOrDefault(d => d.color == eventData.DuckColor);
            if (duck != null)
            {
                duck.isPlaced = true;
            }

            _ducksPlacedInRound++;
            ResetInactivityTimer();

            // Play success sound
            var soundManager = ServiceLocator.Global.Get<SoundManager>();
            if (soundManager != null)
            {
                soundManager.CreateSound()
                    .WithSoundData(GetSoundData(_config.successSoundId))
                    .Play();
            }

            // Check if round is complete
            if (_ducksPlacedInRound >= _config.ducksPerRound)
            {
                CompleteRound().Forget();
            }
        }

        private async UniTaskVoid CompleteRound()
        {
            Debug.Log($"[DuckGameManager] Round {_currentRound} completed!");
            
            EventBus<DuckEvents.RoundCompleteEvent>.Raise(new DuckEvents.RoundCompleteEvent 
            { 
                CompletedRound = _currentRound,
                TotalRounds = _config.totalRounds
            });

            // Play big success particles
            var particleService = ServiceLocator.Global.Get<ParticleService>();
            if (particleService != null)
            {
                await particleService.PlayRoundCompleteEffect();
            }

            await UniTask.Delay(2000); // Wait for effects

            // Check if game is complete
            if (_currentRound >= _config.totalRounds)
            {
                CompleteGame();
            }
            else
            {
                _currentRound++;
                await StartNewRound();
            }
        }

        private void CompleteGame()
        {
            Debug.Log("[DuckGameManager] Game completed!");
            _gameActive = false;
            
            EventBus<DuckEvents.GameCompleteEvent>.Raise(new DuckEvents.GameCompleteEvent 
            { 
                TotalRounds = _config.totalRounds
            });
        }

        private void OnDuckTouched(DuckEvents.DuckTouchedEvent eventData)
        {
            ResetInactivityTimer();
        }

        private void CheckForInactivity()
        {
            if (Time.time - _lastActivityTime > _config.helperHandDelay)
            {
                ShowHelperHand();
                ResetInactivityTimer(); // Reset to avoid spamming
            }
        }

        private void ShowHelperHand()
        {
            // Find a duck that hasn't been placed yet
            var unplacedDuck = _currentDucks.FirstOrDefault(d => !d.isPlaced);
            if (unplacedDuck == null) return;

            // Find the matching basket
            var matchingBasket = _baskets.FirstOrDefault(b => b.color == unplacedDuck.color);
            if (matchingBasket == null) return;

            EventBus<DuckEvents.HelperHandRequestedEvent>.Raise(new DuckEvents.HelperHandRequestedEvent
            {
                DuckColor = unplacedDuck.color,
                DuckPosition = unplacedDuck.floatingPosition,
                TargetBasketPosition = matchingBasket.position
            });
        }

        private void ResetInactivityTimer()
        {
            _lastActivityTime = Time.time;
        }

        private void SetupEventBindings()
        {
            _duckPlacedBinding = new EventBinding<DuckEvents.DuckPlacedCorrectlyEvent>(OnDuckPlacedCorrectly);
            _duckTouchedBinding = new EventBinding<DuckEvents.DuckTouchedEvent>(OnDuckTouched);
            
            EventBus<DuckEvents.DuckPlacedCorrectlyEvent>.Register(_duckPlacedBinding);
            EventBus<DuckEvents.DuckTouchedEvent>.Register(_duckTouchedBinding);
        }

        private SoundData GetSoundData(string soundId)
        {
            // This would be replaced with actual sound data lookup
            // For now, return a placeholder
            return new SoundData();
        }

        public void Dispose()
        {
            EventBus<DuckEvents.DuckPlacedCorrectlyEvent>.Deregister(_duckPlacedBinding);
            EventBus<DuckEvents.DuckTouchedEvent>.Deregister(_duckTouchedBinding);
            
            _gameActive = false;
        }
    }
}