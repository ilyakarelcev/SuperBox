using Cephei;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriceratopsAttacker : MonoBehaviour, IAttacker, IPersonComponent
{
    [SerializeField] private float _smoth;
    [SerializeField] private float _timeToForgetPerson = 0.2f;
    [Space]
    [SerializeField] private Collider[] _colliders;
    [SerializeField] private Transform _headPoint;

    public event Action<Attack> FindPersonEvent;

    public bool IsActive { get; private set; }
    public Vector3 Direction { get; set; }
    
    public IPerson Person { get; private set; }

    private LinkedList<IPerson> _attackedPersons = new LinkedList<IPerson>();

    public int CountAttackedPerson;

    public void Init(IPerson person)
    {
        Person = person;
    }

    private void Update()
    {
        //GetDirection(TestOpponent.position - _headPoint.position);
        CountAttackedPerson = _attackedPersons.Count;
    }

    private void Start()
    {
        SetEnabledOfColliders(false);
    }

    public void StartAttack()
    {
        SetEnabledOfColliders(true);
    }

    public void EndAttack()
    {
        SetEnabledOfColliders(false);
        _attackedPersons.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        IPerson person = IPerson.GetPersonFromRigidbody(other.attachedRigidbody);

        if (person != null && person != Person)
            CreateAttack(person);
    }

    private void CreateAttack(IPerson person)
    {
        if (_attackedPersons.Contains(person))
            return;

        LinkedListNode<IPerson> node = _attackedPersons.AddLast(person);
        StartCoroutine(ForgetPerson(node));

        Vector3 toPerson = person.Position - _headPoint.position;

        Vector3 direction = GetDirection(toPerson);
        direction.y = Mathf.Max(0, direction.y);

        Vector3 contactPoint = GetContactPointByDirection(toPerson);

        Attack attack = new Attack(null, person, direction, 1, contactPoint);
        attack.AddClearImpuls = true;

        FindPersonEvent?.Invoke(attack);
    }

    private Vector3 GetDirection(Vector3 toPerson)
    {
        Vector3 projectOnRight = Vector3.Project(toPerson, _headPoint.right);
        float angle = Vector3.Angle(toPerson.ZeroY(), projectOnRight);        
        float smothAngle = SmothAngle(angle);

        Vector3 toPersonUp = toPerson.GetUp();

        float signRight = Vector3.Dot(projectOnRight, _headPoint.right).Sign();
        float signForward = Vector3.Dot(toPerson, _headPoint.forward).Sign();
        float mainSign = signRight * signForward;

        return (Quaternion.AngleAxis(mainSign * (angle - smothAngle), toPersonUp) * toPerson).normalized;
    }

    private float SmothAngle(float angle)
    {
        float angleKoif = Mathf.InverseLerp(0, 90, angle);
        float smothKoif = Mathf.Sqrt(angleKoif) * _smoth;
        float clampedSmothKoif = Mathf.Min(angleKoif, smothKoif);
        return clampedSmothKoif * 90;
    }

    private Vector3 GetContactPointByDirection(Vector3 direction)
    {
        Physics.Raycast(transform.position, direction, out RaycastHit hit);

        return hit.point;
    }

    private void SetEnabledOfColliders(bool enabled)
    {
        if (IsActive == enabled) return;
        IsActive = enabled;

        foreach (var collider in _colliders)
        {
            collider.enabled = enabled;
        }
    }

    private IEnumerator ForgetPerson(LinkedListNode<IPerson> node)
    {
        yield return new WaitForSeconds(_timeToForgetPerson);

        if(_attackedPersons.Contains(node.Value))
            _attackedPersons.Remove(node);
    }
}