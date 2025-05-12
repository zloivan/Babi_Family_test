using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.DuckGame
{
    public class DuckGameManager : MonoBehaviour
    {
        [SerializeField] private Duck _duckPrefab;
        [SerializeField] private Basket _basketPrefab;

        [SerializeField] private List<Transform> _basketSpawnPoint;

        [SerializeField] private List<BasketConfig> _duckFactory;

        [SerializeField] private Image _swimZone;
        
        private void Start()
        {
            // Initialize the game
            InitializeGame();
        }
        
        private void InitializeGame()
        {
            foreach (var bsp in _basketSpawnPoint)
            {
                var basket = Instantiate(_basketPrefab);
                basket.transform.position = bsp.position;
            }
            
            //for each basket, spawn a duck outside of screen
            foreach (var bsp in _basketSpawnPoint)
            {
                var duck = Instantiate(_duckPrefab);
                duck.transform.position = new Vector3(bsp.position.x, bsp.position.y + 5, bsp.position.z);
            }
        }
    }
}