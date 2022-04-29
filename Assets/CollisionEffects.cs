using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.NiceVibrations;

public class CollisionEffects : MonoBehaviour
{

    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private AudioSource _hitAudio;

    [SerializeField] private float _pitch;

    //[SerializeField] private Color32 _startColorA;
    //[SerializeField] private Color32 _startColorB;
    //[SerializeField] private float _lifeTime;

    private void OnCollisionEnter(Collision collision) {

        for (int i = 0; i < collision.contactCount; i++) {
            ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
            emitParams.velocity = _rigidbody.velocity * 0.5f + Vector3.up * 4f; // * 0.5f;
            emitParams.position = collision.contacts[i].point;
            _particleSystem.Emit(emitParams, 5);
        }

        
        float volume = collision.impulse.magnitude * 0.2f;
        if (volume > 0.05f) {
            _hitAudio.volume = volume;
            Debug.Log(volume);
            float pitch = Mathf.Lerp(_pitch - 0.2f, _pitch + 0.2f, volume);
            _hitAudio.pitch = pitch;
            _hitAudio.Play();
            MMVibrationManager.Haptic(HapticTypes.LightImpact, false, true, this);
        }
        
    }

}
