using System;
using UnityEngine;

namespace _Game.Scripts.DuckGame
{
    [Serializable]
    public enum DuckColor
    {
        Yellow,
        White,
        Blue
    }

    [Serializable]
    public class GameConfig
    {
        [Header("Game Settings")]
        public int totalRounds = 5;
        public int ducksPerRound = 3;
        public float duckScaleOnTouch = 1.1f;
        public float helperHandDelay = 8f;
        
        [Header("Animation Settings")]
        public float duckSwimDuration = 2f;
        public float basketAppearDuration = 1f;
        public float duckJumpDuration = 0.5f;
        
        [Header("Spawn Positions")]
        public Vector3[] duckSpawnPositions = new Vector3[3];
        public Vector3[] basketPositions = new Vector3[3];
        
        [Header("Audio")]
        public string duckSoundId = "duck_sound";
        public string successSoundId = "success_sound";
        public string backgroundMusicId = "duck_game_music";
    }

    [Serializable]
    public class DuckData
    {
        public DuckColor color;
        public Vector3 startPosition;
        public Vector3 floatingPosition;
        public bool isPlaced;
    }

    [Serializable]
    public class BasketData
    {
        public DuckColor color;
        public Vector3 position;
        public bool hasParticleEffect;
    }
}