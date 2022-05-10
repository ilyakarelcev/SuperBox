using Cephei;
using UnityEngine;

public class Heal : MonoBehaviour
{
    [SerializeField] private float _timeToDestroy = 10;
    [Space]
    [SerializeField] private float _forwardTorque = 5;
    [SerializeField] private float _sideTorque = 1;
    [Space]
    [SerializeField] private float _toDecracecTime = 1;
    [SerializeField] private AnimationCurve _scaleCurve;
    [Space]
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Collider _trigger;
    [Space]
    [SerializeField] private ParticleSystem _partical;
    [SerializeField] private AudioSource[] _audio;
    [SerializeField] private AudioSource _audioSource;

    private float _addedHeal;
    private bool _isWorked;

    private SoundSetup _soundSetup;

    public void Init(Vector3 velosity, float noTriggerTime, float addedHeal)
    {
        _rb.AddForce(velosity, ForceMode.VelocityChange);
        AddTorque(velosity.normalized);

        _trigger.enabled = false;
        StartCoroutine(Coroutines.WaitToAction(ActiveTrigger, noTriggerTime));

        _addedHeal = addedHeal;

        _soundSetup = Sound.Bank.Heal;
        Sound.AddSource(_audioSource);
    }

    private void ActiveTrigger()
    {
        _trigger.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isWorked) return;

        IPerson person = IPerson.GetPersonFromRigidbody(other.attachedRigidbody);
        if (person is Player)
        {
            person.HealthManager.AddHeal(_addedHeal);
            _isWorked = true;
            PlayEffects();
            Destroy(gameObject, _timeToDestroy);
        }            
    }

    private void AddTorque(Vector3 direction)
    {
        _rb.AddTorque(direction.GetRight() * _forwardTorque + direction * CustomRandom.Sign() * _sideTorque, ForceMode.VelocityChange);
    }

    [ContextMenu("PlayEffects")]
    private void PlayEffects()
    {
        _partical.transform.rotation = Quaternion.identity;
        _partical.Play();

        PlaySound();


        DecracesAnimation();
    }  

    private void PlaySound()
    {
        Sound.SetupSource(_soundSetup, _audioSource);
        _audioSource.Play();

        return;

        foreach (var audio in _audio)
        {
            audio.Play();
        }

    }
    
    [ContextMenu("Recovery scale")]
    private void RcoveryScale()
    {
        transform.localScale = Vector3.one * 0.6f;
        StopAllCoroutines();
    }

    private void DecracesAnimation()
    {
        transform.localScale = Vector3.zero;

        return;
        float timer = 0;
        Vector3 startScale = transform.localScale;

        void Animation()
        {
            timer += Time.deltaTime;
            transform.localScale = startScale * _scaleCurve.Evaluate(timer / _toDecracecTime);
        }

        StartCoroutine(Coroutines.ActionInUpdat(Animation));
    }
}
