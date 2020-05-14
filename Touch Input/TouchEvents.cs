using UnityEngine;
using System;

public static class TouchEvents
{
    public static Action<SwipeData> OnSwipe;
    public static Action<Vector2> OnTap;
    public static Action<Vector2> OnHold;
    public static Action<Vector2> EndHold;
}