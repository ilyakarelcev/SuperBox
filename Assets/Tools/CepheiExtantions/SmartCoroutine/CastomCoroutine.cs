using System;

public abstract class CastomCoroutine
{
    public event Action Action;
    public bool IsPused { get; private set; }

    protected LifeType _lifeType;

    public CastomCoroutine(Action action, LifeType lifeType)
    {
        Action = action;
        _lifeType = lifeType;
    }

    public void Pause() => IsPused = true;

    public void Play() => IsPused = false;

    public abstract void Destroy();

    protected void InvokeAction()
    {
        Action.Invoke();

        if (_lifeType == LifeType.OneShot)
            Destroy();
    }
}