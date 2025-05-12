using System.Collections.Generic;
using UnityEngine;

namespace BabiFamily.DuckGame.Models
{
    public class DuckGameModel
    {
        private readonly List<DuckData> _ducks = new();
        private readonly List<BasketData> _baskets = new();
        private readonly Dictionary<int, Vector3> _duckPositions = new();
        private readonly Dictionary<int, Vector3> _basketPositions = new();
        
        public int CurrentLevel { get; private set; }
        public int MaxLevels { get; private set; }
        public int CorrectlyPlacedDucks { get; private set; }
        public int DucksPerLevel { get; private set; }
        
        public DuckGameModel(int maxLevels = 5, int ducksPerLevel = 3)
        {
            MaxLevels = maxLevels;
            DucksPerLevel = ducksPerLevel;
            ResetLevel();
        }
        
        public void ResetLevel()
        {
            _ducks.Clear();
            _baskets.Clear();
            _duckPositions.Clear();
            _basketPositions.Clear();
            CorrectlyPlacedDucks = 0;
        }
        
        public void AddDuck(DuckData duck, Vector3 position)
        {
            _ducks.Add(duck);
            _duckPositions[duck.Id] = position;
        }
        
        public void AddBasket(BasketData basket, Vector3 position)
        {
            _baskets.Add(basket);
            _basketPositions[basket.Id] = position;
        }
        
        public void IncrementCorrectlyPlacedDucks()
        {
            CorrectlyPlacedDucks++;
        }
        
        public void IncrementLevel()
        {
            CurrentLevel++;
        }
        
        public bool IsLevelComplete()
        {
            return CorrectlyPlacedDucks >= DucksPerLevel;
        }
        
        public bool IsGameComplete()
        {
            return CurrentLevel >= MaxLevels;
        }
        
        public bool TryGetDuckAndBasketPositions(int duckId, out Vector3 duckPosition, out Vector3 basketPosition)
        {
            if (_duckPositions.TryGetValue(duckId, out duckPosition) && _basketPositions.TryGetValue(duckId, out basketPosition))
            {
                return true;
            }
            
            duckPosition = Vector3.zero;
            basketPosition = Vector3.zero;
            return false;
        }
        
        public DuckData GetDuck(int duckId)
        {
            return _ducks.Find(d => d.Id == duckId);
        }
        
        public BasketData GetBasket(int basketId)
        {
            return _baskets.Find(b => b.Id == basketId);
        }
        
        public List<DuckData> GetAllDucks()
        {
            return _ducks;
        }
        
        public List<BasketData> GetAllBaskets()
        {
            return _baskets;
        }
    }
}