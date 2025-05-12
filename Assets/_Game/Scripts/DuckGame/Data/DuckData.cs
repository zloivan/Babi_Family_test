using UnityEngine;
using Vector3 = System.Numerics.Vector3;

[System.Serializable]
public class DuckData
{
    public int Id;
    public Color Color;
    public float SwimSpeed = 1f;
    public float DragScale = 1.1f;
    public float DefaultScale = 1.0f;
    public bool IsPlaced;
}

[System.Serializable]
public class BasketData
{
    public int Id;
    public Color Color;
    public Vector3 Position;
}