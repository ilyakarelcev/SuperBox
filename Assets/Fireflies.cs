using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireflies : MonoBehaviour {

    [SerializeField] private Transform _target;

    void LateUpdate() {
        transform.position = new Vector3(_target.position.x, 0.5f, _target.position.z);
    }
}
