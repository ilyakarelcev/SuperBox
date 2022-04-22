using UnityEngine;

public class ImpulsTaker : IAttackTaker
{
    public void TakeAttack(Attack attack)
    {
        Rigidbody rb = attack.AttackedPerson.Rigidbody;

        if (attack.AddClearImpuls)
            rb.velocity = Vector3.zero;
        rb.AddForceAtPosition(attack.Impuls, attack.ContactPoint, ForceMode.Impulse);
    }
}