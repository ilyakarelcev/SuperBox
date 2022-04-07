using UnityEngine;
using UnityEngine.AI;

public class NavMeshTest : MonoBehaviour
{
    public bool IsGo;
    [Space]
    public Transform Target;
    public NavMeshAgent[] Agent;
    public MoverBase[] Movers;

    private void Update()
    {
        if (IsGo)
            foreach (var item in Movers)
            {
                item.StartMove();
                item.SetTarget(Target.position);
            }
        else
            foreach (var item in Movers)
            {
                item.StopMove();
            }

        return;
        if (IsGo)
            foreach (var item in Agent)
            {
                item.SetDestination(Target.position); 
            }
        else
            foreach (var item in Agent)
            {
                item.SetDestination(item.transform.position); 
            }
    }
}
