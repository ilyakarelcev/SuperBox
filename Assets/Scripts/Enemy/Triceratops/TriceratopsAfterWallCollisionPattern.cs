using Cephei.StateMachine;

[System.Serializable]
public class TriceratopsAfterWallCollisionPattern : StateMachinePatternBase, IPersonComponent
{
    public IMover Mover;

    public StopAndWaitPattern StopAndWaitPattern;

    public IPerson Person { get; private set; }

    public void Init(IPerson person)
    {
        Person = person;
        Person.InitializeThisComponents(StopAndWaitPattern);
        StopAndWaitPattern.Mover = Mover;
        StopAndWaitPattern.EndWorkEvent += InvokeEndWorkEvent;
    }

    public override void Activate()
    {
        base.Activate();

        //Animation.Play();

        StopAndWaitPattern.Activate();
    }

    public override void DeActivate()
    {
        base.DeActivate();

        StopAndWaitPattern.DeActivate();
    }
}
