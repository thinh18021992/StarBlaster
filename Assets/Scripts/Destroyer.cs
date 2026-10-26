using UnityEngine;

public class Destroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyProjectile") || collision.gameObject.layer == LayerMask.NameToLayer("PlayerProjectile")) 
        {
            collision.gameObject.SetActive(false);
        }
    }
}
