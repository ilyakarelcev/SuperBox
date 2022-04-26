using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [SerializeField] private int _scoreForShortAttackEnemy = 10;
    [SerializeField] private int _scoreForArcher = 10;
    [SerializeField] private int _scoreForTriceratops = 20;
    [SerializeField] private int _scoreForMagic = 20;
    [Space]
    [SerializeField] private Text _scoreText;
    [Space]
    [SerializeField] private EnemiesOnScene _enemiesOnScene;

    public int CurrentScore { get; private set; }
    public int AllScore { get; private set; }

    private void Start()
    {
        _enemiesOnScene.ByPassList(p => OnEnemyDie(p, (s) => AllScore += s));

        _enemiesOnScene.ByPassList(p =>
        p.GetComponent<HealthManager>().DieEvent +=
        () => OnEnemyDie(p, (s) => CurrentScore += s));

        UpdateText();
    }

    private void OnEnemyDie(IPerson enemy, Action<int> action)
    {
        if (enemy is ShortAttackEnemy)
            action.Invoke(_scoreForShortAttackEnemy);
        else if (enemy is Archer)
            action.Invoke(_scoreForArcher);
        else if(enemy is Triceratops)
            action.Invoke(_scoreForTriceratops);
        else if (enemy is Magic)
            action.Invoke(_scoreForMagic);
        else
            Debug.LogError("Uncnowen enemy: " + enemy.GetType().Name);

        UpdateText();
    }

    private void UpdateText()
    {
        _scoreText.text = CurrentScore.ToString() + "/" + AllScore.ToString();
    }
}
