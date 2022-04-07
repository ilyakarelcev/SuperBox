using UnityEngine;

[System.Serializable]
public class AttackCoificentCalculator : IAttackTaker
{
    [SerializeField] private float _damageToMaxMultiply = 10;

    public void TakeAttack(Attack attack)
    {
        attack.AttackCoificent = attack.Damage / _damageToMaxMultiply;
    }
}