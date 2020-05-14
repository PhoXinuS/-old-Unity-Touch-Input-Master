using UnityEngine;

[System.Serializable]
public class TapDetector
{
    public bool detectTap = true;
    public float maxTimeForTap = 0.3f;

    public void DetectTap(float swipeTime, Vector2 touchPosition)
    {
        if ( detectTap &&
            swipeTime < maxTimeForTap )
        {
            TouchEvents.OnTap?.Invoke(touchPosition);
        }
    }
}

