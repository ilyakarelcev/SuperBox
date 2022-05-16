using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class EnemyPointer : MonoBehaviour {

    [SerializeField] HealthManager _enemyHealth;

    private void Start() {
        _enemyHealth.DieEvent += Deactivate;
    }

    public void Activate() {
        PointerManager.Instance.AddToList(this);
    }

    public void Deactivate() {
        PointerManager.Instance.RemoveFromList(this);
    }
}
