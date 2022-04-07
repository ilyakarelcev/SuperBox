using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CameraShakeOnTakeAttack : IAttackTaker
{
    [SerializeField] private CameraShake _cameraShake;

    public void TakeAttack(Attack attack)
    {
        _cameraShake.Shake(attack.AttackCoificent);
    }
}
