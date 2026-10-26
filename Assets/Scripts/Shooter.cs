using NUnit.Framework.Internal.Builders;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static PublicConstant;

public class Shooter : MonoBehaviour
{
    [SerializeField] GameObject projectileObject;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float lifeTime = 5f;
    [SerializeField] float fireRate = 1f;
    [SerializeField] PublicConstant.ObjectType objectType;
    [SerializeField] float minFireRate = 0.1f;
    [Header("EnemyConfig")]
    [SerializeField] float minFireRateVariance = 0.2f;
    [SerializeField] float fireRateVariance = 0.4f;
    [SerializeField] int damage;
    bool isFiring;
    public bool IsFiring
    { 
        get => isFiring; 
        set 
        {
            if (isFiring != value) 
            {
                isFiring = value;
                HandleFiring(isFiring);
            }
            
        }
    }    
    Coroutine fireCoroutine;
    float nextFireTime = 0f;
    private void Awake()
    {
        SetInfo();
    }
    private void Start()
    {
        IsFiring = objectType == ObjectType.Enemy;
    }
    private void Update()
    {}

    private void OnEnable()
    {
        if (objectType == ObjectType.Player)
        {
            EventBus.Subcribe<GetPowerUp>(OnPlayerPowerUp);
        }        
    }

    private void OnDisable()
    {
        if (fireCoroutine != null) 
        {
            StopCoroutine(fireCoroutine);
        }
        if (objectType == ObjectType.Player)
        {
            EventBus.Unsubcribe<GetPowerUp>(OnPlayerPowerUp);
        }        
    }  

    IEnumerator FireContinously()
    {        
        while (isFiring) 
        {
            if (Time.time < nextFireTime)
            {                
                yield return new WaitForSeconds(nextFireTime - Time.time);
                continue;
            }
            SharedProjectiles.sharedProjectiles.GetProjectile(projectileObject, transform.position, transform.up, transform.rotation, projectileSpeed, lifeTime, damage);
            if (objectType == PublicConstant.ObjectType.Player)
            {
                PlayerShooting act = new PlayerShooting { position = transform.position };
                EventBus.Publish(act);
            }
            float currentFireRate = fireRate;
            if (objectType == PublicConstant.ObjectType.Enemy)
            {
                currentFireRate = Ultility.RandomFireRate(fireRate, minFireRateVariance, fireRateVariance);
            }
            nextFireTime = Time.time + Mathf.Clamp(currentFireRate, minFireRate, float.MaxValue);
            yield return new WaitForSeconds(Mathf.Clamp(currentFireRate, minFireRate, float.MaxValue));
            // Logic fire with delay and velocity
        }
        fireCoroutine = null;
    }
    void HandleFiring(bool isFiring) 
    {
        if (isFiring)
        {            
            if (fireCoroutine == null)
            {                
                fireCoroutine = StartCoroutine(FireContinously());
            }
        }
        else 
        {
            if (fireCoroutine != null)
            {
                
                StopCoroutine(fireCoroutine);
                fireCoroutine = null;
            }
        }        
    }

    void SetInfo() 
    {        
        if (objectType == ObjectType.Player)
        {
            fireRate = GameConfig.GetPlayerFireRate();
            damage = GameConfig.GetPlayerDamage();
        }
        else
        {
            fireRate = GameConfig.GetEnemyFireRate();
            damage = GameConfig.GetEnemyDamage();
        }
    }
    void OnPlayerPowerUp(GetPowerUp data)
    {
        if (data.type == PowerupTypes.FireRate) 
        {
            fireRate = fireRate * (1 - (data.value / 100));
        }
    }    
}
