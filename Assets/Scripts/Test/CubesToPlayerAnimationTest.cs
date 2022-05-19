using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubesToPlayerAnimationTest : MonoBehaviour
{
    public CubesMoveToPlayerAnimation Animation;
    public Transform BoxTransform;

    public List<Transform> cubes;

    void Start()
    {
        LinkedList<Transform> linkedList = new LinkedList<Transform>();
        foreach (var item in cubes)
        {
            linkedList.AddLast(item);
        }

        Animation._boxTransform = BoxTransform;
        Animation.Init(linkedList);
    }

    private void Update()
    {
        if(Animation.Work() == false)
        {
            Debug.LogError("Finish!!!");
        }
    }
}
