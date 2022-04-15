using UnityEngine;

[System.Serializable]
public class BalanceSetup: MonoBehaviour, ISengleTone
{
    [SerializeField] private bool _enemyHitToEnemy;
    [SerializeField] private bool _personHitPerson = true;    

    public static bool EnemyHitToEnemy { get; private set; }
    public static bool PersonHitPerson { get; private set; }

    public void Init()
    {
        EnemyHitToEnemy = _enemyHitToEnemy;
        PersonHitPerson = _personHitPerson;        
    }
}
