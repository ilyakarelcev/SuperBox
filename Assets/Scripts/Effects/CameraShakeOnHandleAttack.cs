using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CameraShakeOnHandleAttack : IAttackHandler
{
    [SerializeField] private CameraShake _cameraShake;

    public void Handle(Attack attack)
    {
        _cameraShake.Shake(attack.AttackMultiply);
    }
}
