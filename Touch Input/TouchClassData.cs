using UnityEngine;

public struct TapData
{
    public Vector2 Position;
    public float time;
}

public struct HoldData
{
    public Vector2 Position;
    public float time;
}

public struct SwipeData
{
    public Vector2 StartPosition;
    public Vector2 EndPosition;
    public SwipeDirection Direction;
}

public enum SwipeDirection
{
    Up,
    Down,
    Left,
    Right
}
