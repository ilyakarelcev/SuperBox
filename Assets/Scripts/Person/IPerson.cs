using System.Collections.Generic;
using UnityEngine;

public interface IPerson
{
    HealthManager HealthManager { get; }
    Rigidbody Rigidbody { get; }
    IMover Mover { get; }
    IAttackTakerManager AttackTakerManager { get; }

    Transform Transform { get; }
    CoroutineOperator Operator { get; }
    
    Vector3 Position => Transform.position;
    Vector3 Forward => Transform.forward;
    Quaternion Rotation => Transform.rotation;

    void AddComponent(object component);

    void AddComponents(params object[] component);

    void InitializeThisComponents(params IPersonComponent[] components);

    T GetPersonComponent<T>();

    bool GetPersonComponent<T>(out T component);

    T GetPersonComponentIs<T>();

    bool GetPersonComponentIs<T>(out T component);

    T[] GetAllPersonComponentIs<T>();

    bool GetAllPersonComponentIs<T>(out T[] component);

    public static IPerson GetPersonFromRigidbody(Rigidbody rigidbody)
    {
        if (rigidbody && rigidbody.TryGetComponent(out IPerson person))
            return person;
        return null;
    }
}