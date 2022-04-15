using Cephei.StateMachine;
using Cephei;
using UnityEngine;

[System.Serializable]
public class ArcherStartAttackPatternWithDelay : StateMachinePatternBase, IPersonComponent
{
    [SerializeField] private Vector2 _minMaxDelay = new Vector2(0.3f, 1); 
    [SerializeField] private PlayEffectPattern _playEffectPattern; 

    public IMover Mover;
    public Transform Opponent;

    public IPerson Person { get; private set; }

    private CastomCoroutine _delayCoroutine;

    public void Init(IPerson person)
    {
        Person = person;
        Opponent = PlayerStaticInfo.Player.Transform;
    }

    public override void Activate()
    {
        base.Activate();
        _playEffectPattern.Activate();

        Mover.StartMove();
        Mover.SetTarget(Opponent);

        float randomDelay = _minMaxDelay.GetRandomValue();
        _delayCoroutine = Person.Operator.OpenCoroutineWithTimeStep(InvokeEndWorkEvent, randomDelay, LifeType.OneShot);


        ///
        beginTime = Time.time;
        RandomTimeView = randomDelay;
    }

    float beginTime;
    float endTime;

    public float RandomTimeView;
    public float ResaultTimeView;

    public override void DeActivate()
    {
        base.DeActivate();
        _playEffectPattern.DeActivate();

        _delayCoroutine.Destroy();
        _delayCoroutine = null;


        ///
        endTime = Time.time;
        ResaultTimeView = endTime - beginTime;
    }
}
