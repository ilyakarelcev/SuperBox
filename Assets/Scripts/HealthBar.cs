using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Animator Animator;

    public HealthManager HealthManager;

    private float startHealth;

    private void Start()
    {
        startHealth = HealthManager.StartHealth;
        HealthManager.HealthChangedEvent += UpdateValue;

        slider.value = 1;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            UpdateValue(0);
        }
    }

    private void UpdateValue(float delta)
    {
        slider.value = HealthManager.Health / startHealth;
        Animator.SetTrigger("ApplyDamage");
    }
}
