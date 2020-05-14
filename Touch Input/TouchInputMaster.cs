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

            if (touch.phase == TouchPhase.Began)
            {
                touchesDownPosition.Add(touchID, touch.position);
                touchesUpPosition.Add(touchID, touch.position);
                touchesTime.Add(touchID, 0f);
            }
            touchesTime[touchID] += Time.deltaTime;


            if (touch.phase == TouchPhase.Moved)
            {
                touchesUpPosition[touchID] = touch.position;

                swipeDetector.DetectSwipe(touchesUpPosition[touchID], touchesDownPosition[touchID], TouchPhase.Moved);
                if (swipeDetector.swiped)
                {
                    touchesDownPosition[touchID] = touchesUpPosition[touchID];
                }
            }
            else if (touch.phase == TouchPhase.Stationary)
            {
                touchesUpPosition[touchID] = touch.position;

                if (!swipeDetector.swipeDistanceAchieved)
                {
                    holdDetector.DetectHold(touchesTime[touchID], touchesUpPosition[touchID]);
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                touchesUpPosition[touchID] = touch.position;

                swipeDetector.DetectSwipe(touchesUpPosition[touchID], touchesDownPosition[touchID], TouchPhase.Ended);
                if (!swipeDetector.swipeDistanceAchieved && !swipeDetector.swiped)
                {
                    tapDetector.DetectTap(touchesTime[touchID], touchesUpPosition[touchID]);
                }

                if (holdDetector.holded)
                {
                    TouchEvents.EndHold?.Invoke(touchesUpPosition[touchID]);
                }

                touchesDownPosition.Remove(touchID);
                touchesUpPosition.Remove(touchID);
                touchesTime.Remove(touchID);

                swipeDetector.swiped = false;
                swipeDetector.swipeDistanceAchieved = false;
            }
        }
    }
}

