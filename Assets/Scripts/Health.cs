using System.Collections;
using System.Xml.Serialization;
using UnityEngine;
using static PublicConstant;


public class Health : MonoBehaviour
{
    [SerializeField] int health;
    [SerializeField] int maxHealth;
    [SerializeField] PublicConstant.ObjectType objectType;
    [SerializeField] int baseScore;
    [Header("Player Recovery")]
    [SerializeField] float intervalRecoveryTime = 5f;
    [SerializeField] int recoveryRate = 10;
    Coroutine healingFactor;
    private void Awake()
    {        
        SetInfo();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageDealer damageDealer = collision.GetComponent<DamageDealer>();
        if (damageDealer != null)
        {
            TakeDamage(damageDealer.GetDamage());
        }
    }

    private void OnEnable()
    {
        if (objectType == ObjectType.Player)
        {
            EventBus.Subcribe<GetPowerUp>(OnPlayerGetPowerUp);            
        }        
        
    }
    private void OnDisable()
    {
        if (objectType == ObjectType.Player)
        {
            EventBus.Unsubcribe<GetPowerUp>(OnPlayerGetPowerUp);
        }        
    }
    void TakeDamage(int changeValue) 
    {
        health -= changeValue;
        if (objectType == ObjectType.Player)
        {

            PlayerTakeDamageEvent noti = new PlayerTakeDamageEvent { position = gameObject.transform.position, currentHealth = health, maxHealth = maxHealth };
            EventBus.Publish(noti);
            if (healingFactor != null) 
            {
                StopCoroutine(healingFactor);
            }
            healingFactor = StartCoroutine(HealthRecovery());

        }
        else if (objectType == ObjectType.Enemy)
        {
            EnemyTakeDamageEvent noti = new EnemyTakeDamageEvent { position = gameObject.transform.position, currentHealth = health };
            EventBus.Publish(noti);
        }
        if (health <= 0)
        {
            ObjectDestroyEvent noti = new ObjectDestroyEvent { position = gameObject.transform.position, score = baseScore, type = objectType };
            EventBus.Publish(noti);
            Destroy(gameObject);
        }
    }
    void SetInfo() 
    {        
        if (objectType == ObjectType.Player)
        {     
            maxHealth = GameConfig.GetPlayerHealth();
            health = maxHealth;
            baseScore = 0;
        }
        else
        {         
            maxHealth = GameConfig.GetEnemyHealth();
            health = maxHealth;
            baseScore = GameConfig.GetEnemyBaseScore();
        }
        
    }
    void OnPlayerGetPowerUp(GetPowerUp data) 
    {
        if (data.type == PowerupTypes.Health)
        {
            health += (int)data.value;
            maxHealth += (int)data.value;
            PlayerHealthChanged action = new PlayerHealthChanged { position = transform.position, currentHealth = health, maxHealth = maxHealth, isRecovery = true };
            EventBus.Publish(action);
        }        
    }    
    IEnumerator HealthRecovery()
    {        
        while (health < maxHealth)
        {
            yield return new WaitForSeconds(intervalRecoveryTime);
            AddHealth(recoveryRate);
        }
        healingFactor = null;
    }
    void AddHealth(int value)
    {
        health = Mathf.Min(maxHealth, health + value);
        PlayerHealthChanged action = new PlayerHealthChanged { position = transform.position, currentHealth = health, maxHealth = maxHealth, isRecovery = true};
        EventBus.Publish(action);
    }
}
