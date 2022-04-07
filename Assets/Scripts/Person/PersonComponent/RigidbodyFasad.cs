using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyFasad : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;

    public Rigidbody Rigidbody => _rigidbody;
}
