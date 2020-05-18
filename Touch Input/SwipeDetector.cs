using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SwipeDetector
{
    public bool detectSwipe = true;
    public bool detectSwipeOnlyAfterRelease = false;
    public float minDistanceForSwipe = 100f;

    public Dictionary<int, bool> swipeDistanceAchieved = new Dictionary<int, bool>();
    public Dictionary<int, bool> swiped = new Dictionary<int, bool>();

    private Vector2 fingerDownPosition;
    private Vector2 fingerUpPosition;

    public void RegisterPotentialSwipe(int touchID)
    {
        swipeDistanceAchieved.Add(touchID, false);
        swiped.Add(touchID, false);
    }

    public void DetectSwipe(int touchID, Vector2 fingerUpPosition, Vector2 fingerDownPosition, TouchPhase touchPhase)
    {
        this.fingerDownPosition = fingerUpPosition;
        this.fingerUpPosition = fingerDownPosition;

        if ( detectSwipe && 
            SwipeDistanceCheckMet() )
        {
            if ( !detectSwipeOnlyAfterRelease && touchPhase == TouchPhase.Moved ||
                detectSwipeOnlyAfterRelease && touchPhase == TouchPhase.Ended )        
            {
                if (IsVerticalSwipe())
                {
                    var direction = fingerUpPosition.y - fingerDownPosition.y > 0 ? SwipeDirection.Up : SwipeDirection.Down;
                    SendSwipe(direction);
                }
                else
                {
                    var direction = fingerUpPosition.x - fingerDownPosition.x > 0 ? SwipeDirection.Right : SwipeDirection.Left;
                    SendSwipe(direction);
                }

                swiped[touchID] = true;
            }

            swipeDistanceAchieved[touchID] = true;
        }
    }

    public void UnRegisterPotentialSwipe(int touchID)
    {
        swipeDistanceAchieved.Remove(touchID);
        swiped.Remove(touchID);
    }

    private bool IsVerticalSwipe()
    {
        return VerticalMovementDistance() > HorizontalMovementDistance();
    }

    private bool SwipeDistanceCheckMet()
    {
        return VerticalMovementDistance() > minDistanceForSwipe || HorizontalMovementDistance() > minDistanceForSwipe;
    }

    private float VerticalMovementDistance()
    {
        return Mathf.Abs(fingerDownPosition.y - fingerUpPosition.y);
    }

    private float HorizontalMovementDistance()
    {
        return Mathf.Abs(fingerDownPosition.x - fingerUpPosition.x);
    }


    private void SendSwipe(SwipeDirection direction)
    {
        SwipeData swipeData = new SwipeData()
        {
            Direction = direction,
            StartPosition = fingerDownPosition,
            EndPosition = fingerUpPosition
        };

        TouchEvents.OnSwipe?.Invoke(swipeData);
    }
}

