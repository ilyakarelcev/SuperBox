using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineToPlayerOnInvisible : MonoBehaviour, IPersonComponent
{
    [SerializeField] private LineRenderer _line;

    public IPerson Person { get; private set; }

    private Transform _player;
    private bool _lineIsShowed;

    public void Init(IPerson person)
    {
        Person = person;
        _player = PlayerStaticInfo.Player.Transform;
    }

    private void Update()
    {
        if (_lineIsShowed == false) return;

        _line.SetPositions(new Vector3[] {
            Person.Position + Vector3.up * 0.05f,
            _player.position + Vector3.up * 0.05f
        });
    }

    private void OnBecameVisible()
    {
        SetEnabledLine(true);
    }

    private void OnBecameInvisible()
    {
        SetEnabledLine(false);
    }

    public void SetEnabled(bool enabled)
    {
        this.enabled = enabled;
    }

    public void SetEnabledLine(bool enabled)
    {
        _lineIsShowed = enabled;
        _line.enabled = enabled;
    }
}