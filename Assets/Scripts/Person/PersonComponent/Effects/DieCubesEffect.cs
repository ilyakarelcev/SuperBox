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
    [SerializeField, Range(0, 1)] private float _percentCreatedCubes = 1;
    [Space]
    [SerializeField] private Vector2 _forceMultiply = new Vector2(0.8f, 1);
    [Space]
    [SerializeField] private Vector2 _torque;
    [SerializeField] private PhysicsSuporter.DividVectorByThreeAxi _vectorDivider;
    [Space]
    [SerializeField] private Transform[] _points;
    [SerializeField] private Rigidbody _prifab;    

    public IPerson Person { get; private set; }
    public Action EndAnimationEvent;

    private LinkedList<Transform> _cubes = new LinkedList<Transform>();

    [Header("Test")]
    public Vector3 TestDirection;
    public float TestCoificent;

    public bool Test;

    public void Init(IPerson person)
    {
        Person = person;
    }

    public void Update()
    {
        if (Test)
        {
            TestCast();
            Test = false;
        }
    }

    private void TestCast()
    {
        Vector3 direction = TestDirection;
        float coificent = TestCoificent;

        for (int i = 0; i < _points.Length; i++)
        {
            Transform point = _points[i];

            Rigidbody cube = Instantiate(_prifab, point.position, point.rotation);
            cube.gameObject.SetActive(true);

            cube.AddForce(_vectorDivider.DivideDirection(direction, coificent), ForceMode.VelocityChange);
            cube.AddTorque(UnityEngine.Random.onUnitSphere * _torque.GetRandomValue(), ForceMode.VelocityChange);

            _cubes.AddLast(cube.transform);
        }
    }

    public void OnDie()
    {
        Vector3 direction = Person.AttackTakerManager.CurentAttack.AttackDirection;
        float coificent = Person.AttackTakerManager.CurentAttack.AttackCoificent;

        SmartRandom random = new SmartRandom(10, 5);

        for (int i = 0; i < _points.Length; i++)
        {
            if (random.GetValue() > _percentCreatedCubes)
                continue;

            Transform point = _points[i];

            Rigidbody cube = Instantiate(_prifab, point.position, point.rotation);
            cube.gameObject.SetActive(true);

            Vector3 velocity = _vectorDivider.DivideDirection(direction, coificent);
            Vector3 velosityOnRandom = velocity * _forceMultiply.GetRandomValue();
            cube.AddForce(velosityOnRandom, ForceMode.VelocityChange);

            cube.AddTorque(UnityEngine.Random.onUnitSphere * _torque.GetRandomValue(), ForceMode.VelocityChange);

            _cubes.AddLast(cube.transform);
        }

        Person.Operator.OpenCoroutineWithTimeStep(AnimateCube, _cubesTimeAlive, LifeType.OneShot);

        Debug.LogError("Cubes Count: " + _cubes.Count);
    }

    private void AnimateCube()
    {
        float timer = 0;
        Vector3 startScale = _cubes.First.Value.localScale;

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

[System.Serializable]
public class CubesMoveToPlayerAnimation
{
    [SerializeField] private Vector2 _timeToMove = new Vector2(2, 3);
    [SerializeField] private Vector2 _speed = new Vector2(4, 6);

    public Transform _boxTransform;
    private List<CubeUnit> _cubes;
    private float _timer;

    public void Init(LinkedList<Transform> cubes)
    {
        if(_boxTransform == null)/////
            _boxTransform = PlayerStaticInfo.Player.Transform;
        _cubes = new List<CubeUnit>(cubes.Count);

        foreach (var cube in cubes)
        {
            CubeUnit cubeUnit = new CubeUnit(_timeToMove.GetRandomValue(), _speed.GetRandomValue(), cube);

            _cubes.Add(cubeUnit);
        }
    }

    public bool Work()
    {
        _timer += Time.deltaTime;

        for (int i = 0; i < _cubes.Count; i++)
        {
            if (_cubes[i].TimeToMove > _timer)
                continue;

            _cubes[i].Transform.MoveTowards(_boxTransform.position, _cubes[i].Speed * Time.deltaTime);

            if (_cubes[i].Transform.position == _boxTransform.position)
                _cubes.SmartDelete(i);
        }

        return _cubes.Count != 0;
    }

    private struct CubeUnit
    {
        public float TimeToMove;
        public float Speed;

        public Transform Transform;

        public CubeUnit(float timeToMove, float speed, Transform transform)
        {
            TimeToMove = timeToMove;
            Speed = speed;
            Transform = transform;
        }
    }
}
