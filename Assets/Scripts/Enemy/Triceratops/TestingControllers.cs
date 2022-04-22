using UnityEngine;

public class TestingControllers : MonoBehaviour
{
    public TriceratopsAttackPattern _pattern;
    public TriceratopsAttackView _view;
    [Space]
    public MoverBase Simple;
    public MoverBase Rotator;
    public MoverBase PersonMover;

    private void Start()
    {
        return;

        _pattern._attackView = _view;

        //_pattern.MoverForAttack = Simple;
        _pattern.Rotator = Rotator;
        _pattern.PersonMover = PersonMover;

        Simple.StopMove();

        _pattern.Activate();
    }
}
