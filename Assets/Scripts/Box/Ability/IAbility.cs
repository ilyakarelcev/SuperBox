using System.Collections;
using UnityEngine;

public interface IAbility
{
    public float CoolDownPercent { get; }
    public bool CanUse { get; }

    public void Use();
}

public abstract class AbilityBase : MonoBehaviour, IAbility
{ 
    [SerializeField] private float _coolDownTime = 1;

    public float CoolDownPercent { get; private set; }
    public bool CanUse { get; private set; }

    public void Init()
    {
        CoolDownPercent = 1;
        CanUse = true;
    }

    public virtual void Use()
    {
        StartCoroutine(RecoveryCoolDown());
    }

    private IEnumerator RecoveryCoolDown()
    {
        CanUse = false;
        float timer = 0;

        while (true)
        {
            timer += Time.deltaTime;
            CoolDownPercent = timer / _coolDownTime;

            if (CoolDownPercent >= 1) 
                break;

            yield return null;
        }

        CoolDownPercent = 1;
        CanUse = true;
    }
}