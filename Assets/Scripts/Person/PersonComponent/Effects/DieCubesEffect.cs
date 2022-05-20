using Cephei;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieCubesEffect : MonoBehaviour, IPersonComponent
{
    [SerializeField] private CubesMoveToPlayerAnimation _moveToPlayerAnimation;
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
    [SerializeField] private DieCubesEffectUnit _prefab;    

    public IPerson Person { get; private set; }
    public Action EndAnimationEvent;

    private LinkedList<DieCubesEffectUnit> _cubes = new LinkedList<DieCubesEffectUnit>();

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

            DieCubesEffectUnit newCube = Instantiate(_prefab, point.position, point.rotation);
            newCube.Create(_vectorDivider.DivideDirection(direction, coificent), UnityEngine.Random.onUnitSphere * _torque.GetRandomValue());
            
            _cubes.AddLast(newCube);
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

            DieCubesEffectUnit newCube = Instantiate(_prefab, _points[i].position, _points[i].rotation);

            Vector3 velocity = _vectorDivider.DivideDirection(direction, coificent);
            Vector3 velosityOnRandom = velocity * _forceMultiply.GetRandomValue();
            Vector3 torque = UnityEngine.Random.onUnitSphere * _torque.GetRandomValue();

            newCube.Create(velosityOnRandom, torque);

            _cubes.AddLast(newCube);
        }

        _moveToPlayerAnimation.Init(_cubes);
        Person.Operator.OpenUpdateCoroutine(UpdateMoveToPlayer, LifeType.Cycle);

        Debug.LogError("Cubes Count: " + _cubes.Count);
    }

    private void UpdateMoveToPlayer()
    {
        if (_moveToPlayerAnimation.Work() == false)
        {
            //DestroyAllCube();
            EndAnimationEvent?.Invoke();
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
    private List<DieCubesEffectUnit> _cubes;
    private float _timer;

    public void Init(LinkedList<DieCubesEffectUnit> cubes)
    {
        _boxTransform = PlayerStaticInfo.Player.Transform;
        _cubes = new List<DieCubesEffectUnit>(cubes.Count);

        foreach (var cube in cubes)
        {
            cube.InitToMove(_timeToMove.GetRandomValue(), _speed.GetRandomValue());

            _cubes.Add(cube);
        }
    }

    public bool Work()
    {
        _timer += Time.deltaTime;

        for (int i = 0; i < _cubes.Count; i++)
        {
            if (_cubes[i].TimeToMove > _timer)
                continue;

            if (_cubes[i].MoveToTarger(_boxTransform.position))
                _cubes.SmartDelete(i);
        }

        return _cubes.Count != 0;
    }

    private struct CubeUnit
    {
        public float TimeToMove;
        public float Speed;
        public bool IsMove;

        public Transform Transform;

        public CubeUnit(float timeToMove, float speed, Transform transform)
        {
            TimeToMove = timeToMove;
            Speed = speed;
            Transform = transform;
            IsMove = false;
        }

        public void StartMove()
        {
            if (IsMove == false)
            {
                Transform.GetComponent<Collider>().enabled = false;
                Transform.GetComponent<Rigidbody>().isKinematic = true;
            }

            IsMove = true;            
        }
    }
}
