using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;

public class PowerUpItem : MonoBehaviour
{
    [SerializeField] PublicConstant.PowerupTypes powerType;
    [SerializeField] float moveSpeed = 10;
    [SerializeField] float rotateSpeed = 1;
    [SerializeField] float powerValue;
    Vector3 launchPosition;
    Vector3 initPosition;
    CircleCollider2D hitBox;
    [SerializeField] float padding = 4;
    [SerializeField] float lifeTime = 5;
    private void Awake()
    {
        initPosition = GetComponentInParent<Transform>().position;
        transform.position = initPosition;
    }

    private void Update()
    {
        if (gameObject.activeInHierarchy) 
        {
            transform.rotation *= Quaternion.Euler(0, 0, rotateSpeed * Time.deltaTime);
            transform.position -= new Vector3(0, moveSpeed * Time.deltaTime, 0);
        }
    }
    private void OnEnable()
    {
        GoToLaunchPosition();
        StartCoroutine(LifeTimeCounter());
    }
    private void OnDisable()
    {
        GoBack();
    }
    void GoToLaunchPosition()
    {
        launchPosition = new Vector3( Random.Range(-padding, padding), initPosition.y, 0);
        transform.position = launchPosition;
        gameObject.SetActive(true);
    }
    void GoBack()
    {
        transform.position = initPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerProjectile"))
        {            
            gameObject.SetActive(false);
            GetPowerUp action = new GetPowerUp { type = powerType, value = powerValue };
            EventBus.Publish(action);
        }
    }

    IEnumerator LifeTimeCounter()
    {
        yield return new WaitForSeconds(lifeTime);
        gameObject.SetActive(false);
    }

}
