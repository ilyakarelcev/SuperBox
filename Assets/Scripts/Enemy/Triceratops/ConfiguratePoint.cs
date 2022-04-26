using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfiguratePoint : MonoBehaviour
{
    public Vector3 Size;
    public Vector3Int CountCubeOnAxis;

    public Transform[] points;
    public Transform origin;

    [ContextMenu("Configurate")]
    public void Configurate()
    {
        float xSegment = Size.x / (CountCubeOnAxis.x * 2);
        float ySegment = Size.y / (CountCubeOnAxis.y * 2);
        float zSegment = Size.z / (CountCubeOnAxis.z * 2);

        Action<float, Transform> setByX = (f, p) => 
            p.localPosition = new Vector3(f, p.localPosition.y, p.localPosition.z);
        SetPositionsByAxis(CountCubeOnAxis.x, Size.x, xSegment, setByX);

        Action<float, Transform> setByY = (f, p) =>
            p.localPosition = new Vector3(p.localPosition.x, f, p.localPosition.z);
        SetPositionsByAxis(CountCubeOnAxis.y, Size.y, ySegment, setByY);

        Action<float, Transform> setByZ = (f, p) =>
            p.localPosition = new Vector3(p.localPosition.x, p.localPosition.y, f);
        SetPositionsByAxis(CountCubeOnAxis.z, Size.z, zSegment, setByZ);
    }

    private void SetPositionsByAxis(int countOnAxis, float axisSize, float segmentLenght, Action<float, Transform> action)
    {
        float offset = -1 * axisSize / 2;

        for (int i = 0, position = 0; i < points.Length; i++, position++)
        {
            if (position == countOnAxis)
            {
                position = 0;
            }

            float f = offset + segmentLenght + (position * segmentLenght * 2);
            action.Invoke(f, points[i]);
        }

        //Думаю нужно сделать так чтобы скрипт расставлял точки рядами. Тоесть скажем по Х первые две точки внизу следущие две вверху
    }
}
