using UnityEditor.SceneManagement;
using UnityEngine;

namespace _Game.Scripts.DuckGame
{
    [CreateAssetMenu(fileName = "BasketConfig", menuName = "BasketConfig", order = 0)]
    public class BasketConfig : ScriptableObject
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private Color _color;
        
    }
}