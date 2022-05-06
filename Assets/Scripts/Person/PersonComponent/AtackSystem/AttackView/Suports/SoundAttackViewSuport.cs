using UnityEngine;

[System.Serializable]
public class SoundAttackViewSuport : SoundSource, IATtackViewSuport, IPersonComponent
{
    public IPerson Person { get; private set; }

    public void Init(IPerson person)
    {
        Person = person;
        Init(person.GetPersonComponentIs<IAttackView>());
    }

    public void Init(IAttackView attackView)
    {
        attackView.BeginingOfDamageEvent += OnAttack;
    }

    public void OnAttack()
    {
        PlaySetup(_soundSetup);
    }
}