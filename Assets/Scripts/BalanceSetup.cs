using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class BalanceSetup: MonoBehaviour
{
    [SerializeField] private bool _enemyHitToEnemy;
    [SerializeField] private bool _personHitPerson = true;    

    public static bool EnemyHitToEnemy { get; private set; }
    public static bool PersonHitPerson { get; private set; }

    private void Start()
    {
        EnemyHitToEnemy = _enemyHitToEnemy;
        PersonHitPerson = _personHitPerson;        
    }
}
