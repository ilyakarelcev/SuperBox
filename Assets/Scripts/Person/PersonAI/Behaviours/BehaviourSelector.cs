using Cephei.StateMachine;
using UnityEngine;

public enum BehaviourType
{
    Animal,
    Demon
}

[System.Serializable]
public class BehaviourSelector
{
    [SerializeField] private BehaviourType _behaviourType;
    [Space]
    [SerializeField] private AnimalBehaviour _animalBehaviour = new AnimalBehaviour();
    [SerializeField] private DemonBehaviour _demonBehaviour;

    public BehaviourType BehaviourType => _behaviourType;

    public StateMachineLevle GetBehaviour()
    {
        if (BehaviourType == BehaviourType.Animal)
            return _animalBehaviour;
        else if (BehaviourType == BehaviourType.Demon)
            return _demonBehaviour;

        Debug.LogError("Not correct BehaviourType");
        return null;
    }
}