using System;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private AudioSource _audioSource;

    public event Action<Attack> OnPersonCollision;

    private Vector3 _direction;

    private bool _isUsed;
    private LinkedList<IPerson> _attackedPersons = new LinkedList<IPerson>();
    
    public void Init(Vector3 direction, float speed)
    {
        _rb.velocity = direction * speed;
        _direction = direction;
    }

    private void OnCollisionEnter(Collision collision)
    {
        IPerson person = IPerson.GetPersonFromRigidbody(collision.rigidbody);

        if (person != null)
        {
            if (_attackedPersons.Contains(person))
                return;
            _attackedPersons.AddLast(person);

            Attack attack = new Attack(null, person, _direction, 1, collision.contacts[0].point);
            OnPersonCollision.Invoke(attack);
        }

        HandleCollision();
    }

    private void HandleCollision()
    {
        if (_isUsed) return;
        _isUsed = true;

        _audioSource.Play();
        _audioSource.transform.parent = null;
        Destroy(_audioSource.gameObject, _audioSource.clip.length);

        Destroy(gameObject);
    }
}