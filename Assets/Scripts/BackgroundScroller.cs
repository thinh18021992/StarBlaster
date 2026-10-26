using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] Vector2 moveSpeed;

    Vector2 offset;
    [SerializeField] Material material;

    private void Start()
    {
        material = GetComponent<SpriteRenderer>().material;
        
    }

    private void Update()
    {
        offset += moveSpeed * Time.deltaTime;
        material.mainTextureOffset = offset;
    }
}
