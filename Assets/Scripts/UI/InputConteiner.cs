using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputType
{
    Mouse,
    Touch
}

public class InputConteiner : MonoBehaviour
{
    [SerializeField] private InputType _inputType;

    public event Action<ITouchUnit> TouchEvent;

    private Dictionary<int, TouchUnit> _touches = new Dictionary<int, TouchUnit>();

    private bool _mouseIsPressed;

    private void Update()
    {
        if (_inputType == InputType.Mouse)
            HandleMouse();

        else if (_inputType == InputType.Touch)
            HandleTouch();
    }

    private void HandleMouse()
    {
        //This order very important!!!


        if (Input.GetMouseButtonDown(0) && _mouseIsPressed == false)
        {
            _mouseIsPressed = true;

            //Debug.Log("Mouse button down" + " Time: " + Time.time);

            TouchUnit touchUnit = new TouchUnit(0, Input.mousePosition);
            _touches.Add(0, touchUnit);
            TouchEvent?.Invoke(touchUnit);
        }

        if (Input.GetMouseButton(0) && _mouseIsPressed)
        {
            _touches[0].SetPosition(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0) && _mouseIsPressed)
        {
            _mouseIsPressed = false;

            //Debug.Log("Mouse button up" + " Time: " + Time.time);

            _touches[0].EndOfTouch(Input.mousePosition);
            _touches.Remove(0);
        }




        //if (Input.GetMouseButtonDown(0))
        //{
        //    if (down) {
        //        Debug.LogError("It's very stuped DOWN pressed two");
            
        //    }
        //    down = true;
        //}
        //if (Input.GetMouseButton(0)) ;
        //if (Input.GetMouseButtonUp(0))
        //{
        //    if (down == false) Debug.LogError("UP pressed two");
        //    down = false;
        //}
    }

    bool down;

    private void HandleTouch()
    {
        foreach (var touch in Input.touches)
        {
            if(touch.phase == TouchPhase.Began)
            {
                TouchUnit touchUnit = new TouchUnit(touch.fingerId, touch.position);
                _touches.Add(touch.fingerId, touchUnit);
                TouchEvent?.Invoke(touchUnit);
            }
            if(touch.phase == TouchPhase.Moved)
            {
                _touches[touch.fingerId].SetPosition(touch.position);
            }
            if(touch.phase == TouchPhase.Ended)
            {
                _touches[touch.fingerId].EndOfTouch(touch.position);
            }
        }
    }
}

public class TouchUnit : ITouchUnit
{
    public int Number { get; private set; }

    public Vector2 StartPosition { get; private set; }
    public Vector2 CurentPosition { get; private set; }
    public Vector2 PositionDelta { get; private set; }

    public Vector2 Swipe => StartPosition - CurentPosition;

    public event Action<ITouchUnit> PositionChangeEvent;
    public event Action<ITouchUnit> EndTouchEvent;

    public TouchUnit(int number, Vector2 position)
    {
        Number = number;
        StartPosition = position;
        CurentPosition = position;
    }

    public void SetPosition(Vector2 position)
    {
        if (CurentPosition != position)
        {
            PositionDelta = position - CurentPosition;
            CurentPosition = position;

            PositionChangeEvent?.Invoke(this);
        }
    }

    public void EndOfTouch(Vector2 position)
    {
        CurentPosition = position;
        EndTouchEvent?.Invoke(this);
    }
}

public interface ITouchUnit
{
    public int Number { get; }
    public Vector2 StartPosition { get; }
    public Vector2 CurentPosition { get; }
    public Vector2 PositionDelta { get; }

    public Vector2 Swipe { get; }

    public event Action<ITouchUnit> PositionChangeEvent;
    public event Action<ITouchUnit> EndTouchEvent;
}