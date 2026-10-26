using UnityEngine;

public class Projectile : MonoBehaviour
{
    GameObject prefab;
    float projectileSpeed;
    float lifeTime;
    float timer = 0f;
    Vector2 direction;
    Rigidbody2D projecttileRB;
    int damage;

    public void Construct(GameObject oriPrefab, Vector2 dir, float speed, float lifetime, int dmg) 
    {
        prefab = oriPrefab;        
        direction = dir;
        projectileSpeed = speed;
        this.lifeTime = lifetime;
        this.damage = dmg;
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > lifeTime) 
        {
            gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        projecttileRB = GetComponent<Rigidbody2D>();
        projecttileRB.linearVelocity = direction * projectileSpeed;
    }
    private void OnDisable()
    {
        if (prefab != null) 
        {
            SharedProjectiles.sharedProjectiles.ReturnToPool(prefab, gameObject);
            timer = 0f;
        }        
    }
    public void OnHit()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") || collision.gameObject.layer == LayerMask.NameToLayer("Player")) 
        {
            OnHit();
        }
    }
}
