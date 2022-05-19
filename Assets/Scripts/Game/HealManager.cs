using Cephei;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealManager : MonoBehaviour
{
    [SerializeField, Range(0, 1)] private float _chance = 0.1f;
    [SerializeField] private float _noTriggerTime = 1;
    [Space]
    [SerializeField] private float _velosity = 5;
    [SerializeField, Range(0, 90)] private float _angleOfThrow = 45;
    [Space]
    [SerializeField] private float _addedHeal = 5;
    [Space]
    [SerializeField] private Heal _healPrefab;
    [Space]
    [SerializeField] private EnemiesOnScene _enemiesOnScene;

    private SmartRandom _smartRandom;

    private void Start()
    {
        _smartRandom = new SmartRandom(5, 5);
        _enemiesOnScene.ByPassList(p =>
        p.GetComponent<HealthManager>().DieEvent +=
        () => OnEnemyDie(p));
    }

    private void OnEnemyDie(IPerson enemy)
    {
        if (_chance >= _smartRandom.GetValue())
            SpawnHeal(enemy.Position);
    }

    private void SpawnHeal(Vector3 position)
    {
        Vector3 throwDirection = Random.onUnitSphere.ZeroY().TurnOnSelfRight(_angleOfThrow);
        throwDirection = throwDirection.normalized;

        Heal heal = Instantiate(_healPrefab, position, throwDirection.GetRotation());
        heal.Init(throwDirection * _velosity, _noTriggerTime, _addedHeal);
    }
}

