using System.Collections.Generic;
using UnityEngine;

public class TouchInputMaster : MonoBehaviour
{
    public SwipeDetector swipeDetector = new SwipeDetector();
    public HoldDetector holdDetector = new HoldDetector();
    public TapDetector tapDetector = new TapDetector();

    private Dictionary<int, Vector2> touchesUpPosition = new Dictionary<int, Vector2>();
    private Dictionary<int, Vector2> touchesDownPosition = new Dictionary<int, Vector2>();
    private Dictionary<int, float> touchesTime = new Dictionary<int, float>();

    private void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            var touchID = touch.fingerId;
            var touchPosition = touch.position;

            if (touch.phase == TouchPhase.Began)
            {
                OnTouchBegan(touchID, touchPosition);
            }
            touchesTime[touchID] += Time.deltaTime;

            if (touch.phase == TouchPhase.Moved)
            {
                OnTouchMoved(touchID, touchPosition);
            }
            else if (touch.phase == TouchPhase.Stationary)
            {
                OnTouchStationary(touchID, touchPosition);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                OnTouchEnded(touchID, touchPosition);
            }
        }
    }


    private void OnTouchBegan(int touchID, Vector2 touchPosition)
    {
        touchesDownPosition.Add(touchID, touchPosition);
        touchesUpPosition.Add(touchID, touchPosition);
        touchesTime.Add(touchID, 0f);

        holdDetector.RegisterPotentialHold(touchID);
        swipeDetector.RegisterPotentialSwipe(touchID);
    }

    private void OnTouchMoved(int touchID, Vector2 touchPosition)
    {
        touchesUpPosition[touchID] = touchPosition;

        swipeDetector.DetectSwipe(touchID, touchesUpPosition[touchID], touchesDownPosition[touchID], TouchPhase.Moved);
        if (swipeDetector.swiped[touchID])
        {
            touchesDownPosition[touchID] = touchesUpPosition[touchID];
        }
    }

    private void OnTouchStationary(int touchID, Vector2 touchPosition)
    {
        touchesUpPosition[touchID] = touchPosition;

        if (!swipeDetector.swipeDistanceAchieved[touchID])
        {
            holdDetector.DetectHold(touchID, touchesTime[touchID], touchesUpPosition[touchID]);
        }
    }

    private void OnTouchEnded(int touchID, Vector2 touchPosition)
    {
        touchesUpPosition[touchID] = touchPosition;

        swipeDetector.DetectSwipe(touchID, touchesUpPosition[touchID], touchesDownPosition[touchID], TouchPhase.Ended);
        if (!swipeDetector.swipeDistanceAchieved[touchID] && !swipeDetector.swiped[touchID])
        {
            tapDetector.DetectTap(touchesTime[touchID], touchesUpPosition[touchID]);
        }

        if (holdDetector.holded[touchID])
        {
            holdDetector.EndHold(touchID, touchesUpPosition[touchID]);
        }

        ResetVariables(touchID);
    }

    private void ResetVariables(int touchID)
    {
        touchesDownPosition.Remove(touchID);
        touchesUpPosition.Remove(touchID);
        touchesTime.Remove(touchID);

        holdDetector.UnRegisterPotentialHold(touchID);
        swipeDetector.UnRegisterPotentialSwipe(touchID);
    }
}

