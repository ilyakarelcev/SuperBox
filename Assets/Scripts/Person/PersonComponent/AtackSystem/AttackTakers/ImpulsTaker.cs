using UnityEngine;

public class ImpulsTaker : IAttackTaker
{
    public void TakeAttack(Attack attack)
    {
        Rigidbody rb = attack.AttackedPerson.Rigidbody;

        rb.AddForceAtPosition(attack.Impuls, attack.ContactPoint, ForceMode.Impulse);
    }
}