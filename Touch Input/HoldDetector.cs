using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HoldDetector
{
    public bool detectHold = true;
    public float minTimeForHold = 0.3f;

    public Dictionary<int, bool> holded = new Dictionary<int, bool>();

    public void RegisterPotentialHold(int touchID)
    {
        holded.Add(touchID, false);
    }

    public void DetectHold(int touchID, float touchTime, Vector2 touchPosition)
    {
        if ( detectHold &&
            touchTime >= minTimeForHold )
        {
            TouchEvents.OnHold?.Invoke(touchPosition);
            holded[touchID] = true;
        }
        else
        {
            holded[touchID] = false;
        }
    }

    public void EndHold(int touchID, Vector2 touchPosition)
    {
        if (detectHold)
        {
            TouchEvents.EndHold?.Invoke(touchPosition);

            holded[touchID] = false;
        }
    }

    public void UnRegisterPotentialHold(int touchID)
    {
        holded.Remove(touchID);
    }
}

