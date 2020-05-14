using UnityEngine;

[System.Serializable]
public class HoldDetector
{
    public bool detectHold = true;
    public float minTimeForHold = 0.3f;

    [HideInInspector] public bool holded = false;

    public void DetectHold(float touchTime, Vector2 touchPosition)
    {
        if ( detectHold &&
            touchTime >= minTimeForHold )
        {
            TouchEvents.OnHold?.Invoke(touchPosition);
            holded = true;
        }
        else
        {
            holded = false;
        }
    }
}

