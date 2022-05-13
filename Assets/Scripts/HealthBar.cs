using Cephei;
using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private DamageAnimation _damageAnimation;
    [SerializeField] private HealAnimation _healAnimation;

    public Slider slider;
    public Animator Animator;

    public HealthManager HealthManager;

    private float startHealth;

    public float CurrentHealth => TestHealth;////
    public float StartHealth => HealthManager.StartHealth;////

    private Action _animation;

    [Header("Test")]

    public float TestDelta;
    public float TestHealth;

    public bool TestDamage;
    public bool TestHeal;

    private void Start()
    {
        startHealth = HealthManager.StartHealth;
        HealthManager.HealthChangedEvent += UpdateValue;

        slider.value = 1;

        ////
        _damageAnimation.Init(this);
        HealthManager.ApplyDamageEvent += _damageAnimation.OnEvent;

        _damageAnimation.BeginAnimationEvent += (a) => _animation = a.UpdateAnimation;
        _damageAnimation.EndAnimationEvent += (a) => _animation = null;
        ////

        ////
        _healAnimation.Init(this);
        HealthManager.AddHealEvent += _healAnimation.OnEvent;

        _healAnimation.BeginAnimationEvent += (a) => _animation = a.UpdateAnimation;
        _healAnimation.EndAnimationEvent += (a) => _animation = null;
    }

    public void Update()
    {
        _animation?.Invoke();

        if (TestDamage)
        {
            TestDamage = false;
            _damageAnimation.OnEvent(TestDelta);
        }
        if (TestHeal)
        {
            TestHeal = false;
            _healAnimation.OnEvent(TestDelta);
        }
    }

    private void UpdateValue(float delta)
    {
        //slider.value = HealthManager.Health / startHealth;
        Animator.SetTrigger("ApplyDamage");
    }

    private abstract class Animation
    {
        protected HealthBar _healthBar;
        protected float _timer;

        public event Action<Animation> BeginAnimationEvent;
        public event Action<Animation> EndAnimationEvent;
        protected void EndAnimation() => EndAnimationEvent?.Invoke(this);


        public void Init(HealthBar healthBar)
        {
            _healthBar = healthBar;
        }

        public virtual void OnEvent(float delta)
        {
            _timer = 0;

            BeginAnimationEvent?.Invoke(this);
        }

        public abstract void UpdateAnimation();

        public abstract void Deactivate();
    }

    [System.Serializable]
    private class DamageAnimation : Animation
    {
        [SerializeField] private float _time = 1;
        [SerializeField] private AnimationCurve _curve;
        [Space]
        [SerializeField] private Slider _mainSlider;
        [SerializeField] private Slider _damageSlider;

        private float _valueBefor;
        private float _targetValue;
        private float _difference;

        public override void OnEvent(float delta)
        {
            base.OnEvent(delta);

            _damageSlider.gameObject.SetActive(true);

            _targetValue = _healthBar.CurrentHealth / _healthBar.StartHealth;
            _valueBefor = (_healthBar.CurrentHealth + delta) / _healthBar.StartHealth;
            _difference = delta / _healthBar.StartHealth;

            _damageSlider.value = _valueBefor;
            _mainSlider.value = _targetValue;
        }

        public override void UpdateAnimation()
        {
            _timer += Time.deltaTime;
            float percent = (_timer / _time).Clamp01();
            
            _damageSlider.value = _valueBefor - _curve.Evaluate(percent) * _difference;

            if (percent >= 1)
                EndAnimation();
        }

        public override void Deactivate()
        {
            _damageSlider.gameObject.SetActive(false);
        }
    }

    [System.Serializable]
    private class HealAnimation : Animation
    {
        [SerializeField] private float _time = 1;
        [SerializeField] private AnimationCurve _curve;
        [Space]
        [SerializeField] private Slider _mainSlider;
        [SerializeField] private Slider _healSlider;

        private float _valueBefor;
        private float _targetValue;
        private float _difference;

        public override void OnEvent(float delta)
        {
            base.OnEvent(delta);

            _healSlider.gameObject.SetActive(true);

            _targetValue = _healthBar.CurrentHealth;
            _valueBefor = _healSlider.value;
            _difference = _targetValue - _valueBefor;

            _healSlider.value = _targetValue;
        }

        public override void UpdateAnimation()
        {
            _timer += Time.deltaTime;
            float percent = (_timer / _time).Clamp01();

            _mainSlider.value = _valueBefor + _curve.Evaluate(percent) * _difference;

            if (percent >= 1)
                EndAnimation();
        }

        public override void Deactivate()
        {
            _healSlider.gameObject.SetActive(false);
        }
    }
}
