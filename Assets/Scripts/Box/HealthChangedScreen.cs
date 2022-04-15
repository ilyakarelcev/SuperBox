using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class HealthChangedScreen : IPersonComponent
{
    [SerializeField] private Color _damageColor;
    [SerializeField] private Color _healColor;
    [Space]
    [SerializeField] private float _shadedTime;
    [SerializeField] private AnimationCurve _curve;
    [Space]
    [SerializeField] private Image _image;

    public IPerson Person { get; private set; }

    private CastomCoroutine _coroutine;

    public void Init(IPerson person)
    {
        Person = person;

        person.HealthManager.ApplyDamageEvent += OnApplyDamage;
        person.HealthManager.AddHealEvent += OnAddHealth;
    }

    private void OnApplyDamage(float damage)
    {
        _image.color = _damageColor;
        ShadingImage();
    }

    private void OnAddHealth(float addedHealth)
    {
        _image.color = _healColor;
        ShadingImage();
    }

    private void ShadingImage()
    {
        float timer = 0;
        Color color = _image.color;

        _coroutine?.Destroy();
        _coroutine = Person.Operator.OpenUpdateCoroutine(While, LifeType.Cycle);

        void While()
        {
            timer += Time.deltaTime;
            float timeCoificent = timer / _shadedTime;

            color.a = _curve.Evaluate(timeCoificent);
            _image.color = color;

            if (timeCoificent >= 1)
                _coroutine.Destroy();
        }
    }
}