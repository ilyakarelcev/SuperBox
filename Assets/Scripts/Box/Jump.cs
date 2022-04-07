using UnityEngine;

public class Jump
{
    public Vector3 Direction;
    public float Multiply;

    public float Duration;

    public bool IsEnd => _startTime + Duration < Time.time;
    public float TimePercent => Mathf.InverseLerp(_startTime + Duration, _startTime, Time.time);
    public float MultiplyOnTimePercent => Multiply * TimePercent;

    private float _startTime;

    public Jump(Vector3 direction, float multiply, float duration)
    {
        SetValues(direction, multiply, duration);
    }

    public void SetValues(Vector3 direction, float multiply, float duration)
    {
        Direction = direction;
        Multiply = multiply;
        Duration = duration;

        _startTime = Time.time;
    }
}
