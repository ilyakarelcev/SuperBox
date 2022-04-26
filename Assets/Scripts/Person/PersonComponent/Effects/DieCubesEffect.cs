using Cephei;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieCubesEffect : MonoBehaviour, IPersonComponent
{
    [SerializeField] private float _cubesTimeAlive = 3;
    [SerializeField] private float _animationTime = 1;
    [Space]
    [SerializeField] private Vector2 _torque;
    [SerializeField] private PhysicsSuporter.DividVectorByThreeAxi _vectorDivider;
    [Space]
    [SerializeField] private Transform[] _points;
    [SerializeField] private Rigidbody _prifab;    

    public IPerson Person { get; private set; }
    public Action EndAnimationEvent;

    private Transform[] _cubes;

    [Header("Test")]
    public Vector3 TestDirection;
    public float TestCoificent;

    public bool Test;

    public void Init(IPerson person)
    {
        Person = person;
    }

    public void OnDie()
    {
        Vector3 direction = Person.AttackTakerManager.CurentAttack.AttackDirection;
        float coificent = Person.AttackTakerManager.CurentAttack.AttackCoificent;

        _cubes = new Transform[_points.Length];

        for (int i = 0; i < _points.Length; i++)
        {
            Transform point = _points[i];

            Rigidbody cube = Instantiate(_prifab, point.position, point.rotation);
            cube.gameObject.SetActive(true);

            cube.AddForce(_vectorDivider.DivideDirection(direction, coificent), ForceMode.VelocityChange);
            cube.AddTorque(UnityEngine.Random.onUnitSphere * _torque.GetRandomValue(), ForceMode.VelocityChange);

            _cubes[i] = cube.transform;
        }

        Person.Operator.OpenCoroutineWithTimeStep(AnimateCube, _cubesTimeAlive, LifeType.OneShot);
    }

    private void AnimateCube()
    {
        float timer = 0;
        Vector3 startScale = _cubes[0].localScale;

        CastomCoroutine coroutine = null;
        coroutine = Person.Operator.OpenUpdateCoroutine(While, LifeType.Cycle);

        void While()
        {
            timer += Time.deltaTime;

            foreach (var cube in _cubes)
            {
                cube.localScale = startScale * Mathf.Min(1 - (timer / _animationTime), 1);
            }

            if (timer / _animationTime > 1)
            {
                DestroyAllCube();
                EndAnimationEvent?.Invoke();
                coroutine.Destroy();
            }
        }        
    }

    private void DestroyAllCube()
    {
        foreach (var cube in _cubes)
        {
            Destroy(cube.gameObject);
        }
    }
}
