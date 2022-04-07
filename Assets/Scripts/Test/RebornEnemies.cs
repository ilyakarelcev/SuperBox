using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RebornEnemies : MonoBehaviour
{
    public bool Reborn;

    public GameObject Prafab;
    public Transform SpawnPosition;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) || Reborn)
        {
            Reborn = false;

            GameObject enemy = Instantiate(Prafab, SpawnPosition.position, SpawnPosition.rotation, transform);
            enemy.SetActive(true);
        }
    }
}
