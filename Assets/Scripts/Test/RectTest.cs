using Cephei;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode()]
public class RectTest : MonoBehaviour
{
    public RectTransform TestRect;
    public RectTransform TestPoint;

    public Vector2 XValuesView;
    public Vector2 YValuesView;

    public bool TestOnPointIsInside;
    public bool IsPointInsideView;

    private void Update()
    {
        Rect rect = TestRect.rect;
        XValuesView = new Vector2(rect.xMin, rect.xMax);
        YValuesView = new Vector2(rect.yMin, rect.yMax);

        if (TestOnPointIsInside)
        {
            TestOnPointIsInside = false;

            IsPointInsideView = UISuporter.IsPointUnsideRect(TestPoint.position, TestRect);
        }
    }
}
