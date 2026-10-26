using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] float shakeDuration;
    [SerializeField] float shakeMagnitude;

    Vector3 initialPosition;

    private void Awake()
    {
        initialPosition = transform.position;
    }
    private void OnEnable()
    {
        EventBus.Subcribe<PlayerTakeDamageEvent>(OnPlayerTakeDamage);
    }

    private void OnDisable()
    {
        EventBus.Unsubcribe<PlayerTakeDamageEvent>(OnPlayerTakeDamage);
    }
    void OnPlayerTakeDamage(PlayerTakeDamageEvent eventData) 
    {
        StartCoroutine(ShakeCamera());
    }
    IEnumerator ShakeCamera()
    {
        float timer = 0;
        while (timer < shakeDuration)
        {
            transform.position = initialPosition + (Vector3)Random.insideUnitCircle * shakeMagnitude;
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.position = initialPosition;        
    }
}
