using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PersonBase : MonoBehaviour, IPerson
{
    public HealthManager HealthManager { get; protected set; }

    public Rigidbody Rigidbody { get; protected set; }

    public IMover Mover { get; private set; }

    public IAttackTakerManager AttackTakerManager { get; private set; }

    public Transform Transform { get; protected set; }

    public CoroutineOperator Operator { get; protected set; }

    private List<object> AllComponents = new List<object>();

    [SerializeField] private Component[] _components;
    [SerializeField] private Component[] _componentsToInit;

    public void Init(HealthManager healthManager, Rigidbody rigidbody, IMover mover, IAttackTakerManager attackTakerManager, Transform transform)
    {
        HealthManager = healthManager;
        Rigidbody = rigidbody;
        Mover = mover;
        AttackTakerManager = attackTakerManager;
        Transform = transform;

        Operator = new CoroutineOperator();

        AllComponents.AddRange(_components);
    }

    protected void InitializeAllComponent()
    {
        foreach (var component in AllComponents)
        {
            if (component is IPersonComponent)
                (component as IPersonComponent).Init(this);
        }

        InitializeThisComponents(_componentsToInit.Select(x => (IPersonComponent)x).ToArray());
    }

    public void InitializeThisComponents(params IPersonComponent[] components)
    {
        foreach (var component in components)
        {
            component.Init(this);
        }
    }

    public void AddComponent(object component)
    {
        AllComponents.Add(component);
    }

    public void AddComponents(params object[] components)
    {
        AllComponents.AddRange(components);
    }

    public T GetPersonComponent<T>()
    {
        foreach (var component in AllComponents)
        {
            if (component.GetType() == typeof(T))
                return (T)component;
        }
        throw new System.Exception("Don't have any component");
        return default;
    }

    public bool GetPersonComponent<T>(out T component)
    {
        foreach (var componentLocal in AllComponents)
        {
            if (componentLocal.GetType() == typeof(T))
            {
                component = (T)componentLocal;
                return true;
            }
        }
        component = default;
        return false;
    }

    public T GetPersonComponentIs<T>()
    {
        foreach (var component in AllComponents)
        {
            if (component is T)
                return (T)component;
        }
        return default;
    }

    public bool GetPersonComponentIs<T>(out T component)
    {
        foreach (var componentLocal in AllComponents)
        {
            if (componentLocal is T)
            {
                component = (T)componentLocal;
                return true;
            }
        }
        component = default;
        return false;
    }

    public T[] GetAllPersonComponentIs<T>()
    {
        LinkedList<T> componentsList = new LinkedList<T>();
        foreach (var componentLocal in AllComponents)
        {
            if (componentLocal is T)
            {
                componentsList.AddLast((T)componentLocal);
            }
        }
        return componentsList.ToArray();
    }

    public bool GetAllPersonComponentIs<T>(out T[] components)
    {
        LinkedList<T> componentsList = new LinkedList<T>();
        foreach (var componentLocal in AllComponents)
        {
            if (componentLocal is T)
            {
                componentsList.AddLast((T)componentLocal);
            }
        }
        components = componentsList.ToArray();
        return components.Length > 0;
    }

    protected void Update()
    {
        CustomUpdate();
    }

    private void FixedUpdate()
    {
        CustomFixedUpdate();
    }

    private void LateUpdate()
    {
        CustomLateUpdate();
    }

    protected virtual void CustomUpdate()
    {
        Operator.RunUpdate();
    }

    protected virtual void CustomFixedUpdate()
    {
        Operator.RunFixedUpdate();
    }

    protected virtual void CustomLateUpdate()
    {
        Operator.RunLateUpdate();
    }
}