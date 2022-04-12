using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cephei
{
    public static class UISuporter
    {
        public static bool IsPointUnsideRect(Vector2 point, RectTransform rect)
        {
            if (point.x < (rect.position.x + rect.rect.xMax)
            && point.x > (rect.position.x + rect.rect.xMin)
            && point.y < (rect.position.y + rect.rect.yMax)
            && point.y > (rect.position.y + rect.rect.yMin))
            {
                return true;
            }
            return false;
        }
    }
}
