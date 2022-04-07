using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleTest : MonoBehaviour
{
    [Range(0.1f, 3)] public float Radius = 1;

    public List<Transform> Enemies;

    [SerializeField, HideInInspector] private List<Vector3> _positions;

    private void OnValidate()
    {
        if (_positions.Count == 0) return;

        for (int i = 0; i < Enemies.Count; i++)
        {
            Vector3 newPosition = transform.position + (_positions[i] - transform.position) * Radius;

            Enemies[i].position = newPosition;
        }
    }

    [ContextMenu("Remember Position")]
    public void RememberPosition()
    {
        _positions.Clear();

        foreach (var item in Enemies)
        {
            _positions.Add(item.position);
        }
    }
}
