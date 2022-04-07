using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleTest : MonoBehaviour
{
    public bool Fall;
    public bool ComeBack;
    [Space]
    public float Speed = 5;
    public int Count;
    [Space]
    public Vector3 Volume;
    public Rigidbody Prefab;

    public Rigidbody[] _fallObjects;

    private Dictionary<Rigidbody, Vector3> _startPositions = new Dictionary<Rigidbody, Vector3>();

    private void Start()
    {
        foreach (var item in _fallObjects)
        {
            _startPositions.Add(item, item.position);
        }
    }

    private void Update()
    {
        if (Fall || Input.GetKeyDown(KeyCode.F))
        {
            FallObjects();
            Fall = false;
        }

        if (ComeBack || Input.GetKeyDown(KeyCode.C))
        {
            ComeBack = true;
            ComeBackLerp();
        }
    }

    [ContextMenu("Spawn")]
    public void SpawnObjects()
    {
        foreach (var item in _fallObjects)
        {
            Destroy(item);
        }

        _fallObjects = new Rigidbody[Count];
        for (int i = 0; i < Count; i++)
        {
            Vector3 randomPosition = Cephei.VectorExtantion.GetRandomPositionInBox(transform.position, Volume);
            Rigidbody newRb = Instantiate(Prefab, randomPosition, Quaternion.identity, transform);
            _fallObjects[i] = newRb;
        }
    }

    private void FallObjects()
    {
        foreach (var item in _fallObjects)
        {
            item.isKinematic = false;
        }
        ComeBack = false;
    }

    private void ComeBackLerp()
    {
        foreach (var item in _startPositions)
        {
            item.Key.isKinematic = true;
            item.Key.transform.position = Vector3.Lerp(item.Key.transform.position, item.Value, Speed * Time.deltaTime);
        }
    }
}
