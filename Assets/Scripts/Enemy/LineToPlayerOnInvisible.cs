using Cephei;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineToPlayerOnInvisible : MonoBehaviour, IPersonComponent
{
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private LineRenderer _line;

    public IPerson Person { get; private set; }

    private Transform _player;
    public bool _lineIsShowed;
    private bool _isActive;

    public void Init(IPerson person)
    {
        Person = person;
        _player = PlayerStaticInfo.Player.Transform;
    }

    private void LateUpdate()
    {
        if (_lineIsShowed == false) return;
        if (_player == null)
            _player = PlayerStaticInfo.Player.Transform;

        _line.SetPositions(new Vector3[] {
            Person.Position.ZeroY() + Vector3.up * 0.05f,
            _player.position.ZeroY() + Vector3.up * 0.05f
        });        
    }

    private void OnBecameVisible()
    {
        if (_isActive == false) return;

        SetEnabledLine(false);
        PlayAnimation();
    }

    private void OnBecameInvisible()
    {
        if (_isActive == false) return;

        SetEnabledLine(true);
        PlayAnimation();
    }

    [ContextMenu("Active")]
    public void Activate()
    {
        _isActive = true;
        gameObject.SetActive(true);

        if (_renderer.isVisible == false)
        {
            SetEnabledLine(true);
            PlayAnimation();
        }
    }

    [ContextMenu("Deactive")]
    public void Deactive()
    {
        _isActive = false;
        gameObject.SetActive(false);

        SetEnabledLine(false);
    }

    public void SetEnabledLine(bool enabled)
    {
        _lineIsShowed = enabled;

        if(enabled == false)
            _line.SetPositions(new Vector3[] { Vector3.zero, Vector3.zero });
    }

    public void PlayAnimation()
    {

    }
}