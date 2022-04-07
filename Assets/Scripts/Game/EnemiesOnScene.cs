using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesOnScene : MonoBehaviour
{
    [SerializeField]
    private List<PersonBase> _enemies;

    [ContextMenu("FindAllEnemyOnScean")]
    public void FindAllEnemyOnScean()
    {
        _enemies = new List<PersonBase>();
        _enemies.AddRange(FindObjectsOfType<PersonBase>());

        PersonBase player = null;
        foreach (var item in _enemies)
        {
            if (item is Player) 
                player = item;
        }
        _enemies.Remove(player);
    }

    public void ByPassList(Action<PersonBase> action)
    {
        foreach (var enemy in _enemies)
        {
            action.Invoke(enemy);
        }
    }
}
