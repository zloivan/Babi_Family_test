using UnityEngine;

namespace _Game.Scripts.DuckGame
{
    public class Basket : MonoBehaviour
    {
        [SerializeField] private Color _paintColor;
        
        public Color PaintColor => _paintColor;
        
        
    }
}