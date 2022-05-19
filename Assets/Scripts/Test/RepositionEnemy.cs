using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepositionEnemy : MonoBehaviour
{
    public bool Reposition;

    public Transform Enemy;
    public Transform SpawnPosition;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) || Reposition)
        {
            Reposition = false;

            Enemy.position = SpawnPosition.position;
        }
    }
}
