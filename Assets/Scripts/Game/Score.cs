using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [SerializeField] private int _scoreForShortAttackEnemy = 10;
    [SerializeField] private int _scoreForArcher = 10;
    [Space]
    [SerializeField] private Text _scoreText;
    [Space]
    [SerializeField] private EnemiesOnScene _enemiesOnScene;

    private int _score;
    private int _allScore;

    private void Start()
    {
        _enemiesOnScene.ByPassList(p => OnEnemyDie(p, ref _allScore));

        _enemiesOnScene.ByPassList(p => 
        p.GetComponent<HealthManager>().DieEvent += 
        () => OnEnemyDie(p, ref _score));

        UpdateText();
    }

    private void OnEnemyDie(IPerson enemy, ref int score)
    {
        if (enemy is ShortAttackEnemy)
            score += _scoreForShortAttackEnemy;
        else if (enemy is Archer)
            score += _scoreForArcher;
        else
            Debug.LogError("Uncnowen enemy");

        UpdateText();
    }

    private void UpdateText()
    {
        _scoreText.text = _score.ToString() + "/" + _allScore.ToString();
    }
}
