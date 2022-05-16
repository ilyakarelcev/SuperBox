using Cephei;
using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private DamageAnimation _damageAnimation;
    [SerializeField] private HealAnimation _healAnimation;
    [Space]
    public Animator Animator;
    public HealthManager HealthManager;

    public float CurrentHealth => HealthManager.Health;
    public float StartHealth => HealthManager.StartHealth;

    private Animation _currentAnimation;

    [Space]
    [SerializeField] private Test _test;    

    private void Start()
    {
        HealthManager.HealthChangedEvent += UpdateValue;


        _damageAnimation.Init(this);
        HealthManager.ApplyDamageEvent += _damageAnimation.OnEvent;

        _damageAnimation.BeginAnimationEvent += OnAnimationBegin;
        _damageAnimation.EndAnimationEvent += (a) => _currentAnimation = null;

        
        _healAnimation.Init(this);
        HealthManager.AddHealEvent += _healAnimation.OnEvent;

        _healAnimation.BeginAnimationEvent += OnAnimationBegin;
        _healAnimation.EndAnimationEvent += (a) => _currentAnimation = null;
    }

    public void Update()
    {
        _currentAnimation?.UpdateAnimation();

        //Test
        _test.TryTest(_damageAnimation, _healAnimation);
    }

    private void OnAnimationBegin(Animation animation)
    {
        _currentAnimation?.Deactivate();
        _currentAnimation = animation;
    }

    private void UpdateValue(float delta)
    {
        Animator.SetTrigger("ApplyDamage");
    }

    [Serializable]
    private class Test
    {
        public float TestDelta;
        public float TestHealth;

        public bool TestDamage;
        public bool TestHeal;

        public void TryTest(Animation damageAnimation, Animation healAnimation)
        {
            if (TestDamage)
            {
                TestDamage = false;
                damageAnimation.OnEvent(TestDelta);
            }
            if (TestHeal)
            {
                TestHeal = false;
                healAnimation.OnEvent(TestDelta);
            }
        }
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
            {
                Deactivate();
                EndAnimation();
            }
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

            _targetValue = _healthBar.CurrentHealth / _healthBar.StartHealth;
            _valueBefor = _mainSlider.value;
            _difference = _targetValue - _valueBefor;

            _healSlider.value = _targetValue;
        }

        public override void UpdateAnimation()
        {
            _timer += Time.deltaTime;
            float percent = (_timer / _time).Clamp01();

            _mainSlider.value = _valueBefor + _curve.Evaluate(percent) * _difference;

            if (percent >= 1)
            {
                Deactivate();
                EndAnimation();
            }
        }

        public override void Deactivate()
        {
            _healSlider.gameObject.SetActive(false);
        }
    }
}
