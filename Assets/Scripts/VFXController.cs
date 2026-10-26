using NUnit.Framework;
using UnityEngine;

public class VFXController : MonoBehaviour
{
    [SerializeField] ParticleSystem explosionEffect;
    [SerializeField] ParticleSystem recoveryEffect;

    private void Awake()
    {
        
    }

    private void OnEnable()
    {
        EventBus.Subcribe<ObjectDestroyEvent>(OnEnemyDesTroyed);
        EventBus.Subcribe<PlayerTakeDamageEvent>(OnPlayerTakeDamage);
        EventBus.Subcribe<PlayerHealthChanged>(OnPlayerHealthChanged);
    }

    private void OnDisable()
    {
        EventBus.Unsubcribe<ObjectDestroyEvent>(OnEnemyDesTroyed);
        EventBus.Unsubcribe<PlayerTakeDamageEvent>(OnPlayerTakeDamage);
        EventBus.Unsubcribe<PlayerHealthChanged>(OnPlayerHealthChanged);
    }

    void OnEnemyDesTroyed(ObjectDestroyEvent eventData)
    {
        ParticleSystem fxToPlay = Instantiate(explosionEffect, eventData.position, Quaternion.identity, gameObject.transform);
        fxToPlay.Play();
        Destroy(fxToPlay, fxToPlay.main.duration + fxToPlay.main.startLifetime.constantMax);
    }
    void OnPlayerTakeDamage(PlayerTakeDamageEvent eventData)
    { 
        ParticleSystem fxToPlay = Instantiate(explosionEffect, eventData.position, Quaternion.identity, gameObject.transform); 
        fxToPlay.Play();
        Destroy(fxToPlay, fxToPlay.main.duration + fxToPlay.main.startLifetime.constantMax);
    }
    void OnPlayerHealthChanged(PlayerHealthChanged data)
    {
        if (data.isRecovery)
        {
            ParticleSystem fxToPlay = Instantiate(recoveryEffect, data.position, Quaternion.identity, gameObject.transform);
            fxToPlay.Play();
            Destroy(fxToPlay, fxToPlay.main.duration + fxToPlay.main.startLifetime.constantMax);
        }
    }
}
