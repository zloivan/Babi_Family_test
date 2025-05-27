using UnityEngine;

namespace _Game.Scripts.DuckGame
{
    public class DuckGameConfigProvider : MonoBehaviour
    {
        [SerializeField] private GameConfig _config;
        
        public GameConfig Config => _config;

        private void Awake()
        {
            if (_config == null)
            {
                _config = new GameConfig();
                SetupDefaultConfig();
            }
        }

        private void SetupDefaultConfig()
        {
            // Set default positions (adjust based on your UI layout)
            _config.duckSpawnPositions = new Vector3[]
            {
                new Vector3(-400f, -200f, 0f), // Left duck
                new Vector3(0f, -200f, 0f),    // Center duck
                new Vector3(400f, -200f, 0f)   // Right duck
            };

            _config.basketPositions = new Vector3[]
            {
                new Vector3(-300f, 100f, 0f),  // Left basket
                new Vector3(0f, 100f, 0f),     // Center basket
                new Vector3(300f, 100f, 0f)    // Right basket
            };
        }
    }
}