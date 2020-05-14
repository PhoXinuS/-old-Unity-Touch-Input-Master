using UnityEngine;

[System.Serializable]
public class SwipeDetector
{
    public bool detectSwipe = true;
    public bool detectSwipeOnlyAfterRelease = false;
    public float minDistanceForSwipe = 100f;

    [HideInInspector] public bool swipeDistanceAchieved = false;
    [HideInInspector] public bool swiped = false;

    private Vector2 fingerDownPosition;
    private Vector2 fingerUpPosition;

    public void DetectSwipe(Vector2 fingerUpPosition, Vector2 fingerDownPosition, TouchPhase touchPhase)
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

                swiped = true;
            }

            swipeDistanceAchieved = true;
        }
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

