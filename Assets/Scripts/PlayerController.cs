using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10;
    [SerializeField] SpriteRenderer playerTexture;
    [SerializeField] Vector2 offsetPos;
    [Range(0f, 100f)] [SerializeField] float verticalBorderRate = 30;
    Shooter shooter;    
    InputAction fireAction;    
    Vector2 minBound;
    Vector2 maxBound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shooter = GetComponentInChildren<Shooter>();        
        fireAction = InputSystem.actions.FindAction("Fire");        
        InitBounds();
        fireAction.performed += Fire;
        fireAction.canceled += CeaseFire;
    }    

    private void OnDisable()
    {
        fireAction.performed -= Fire;
        fireAction.canceled -= CeaseFire;
    }
    // Update is called once per frame
    void Update()
    {
        MovePlayer();        
    }
    void MovePlayer()
    {
        Vector2 targetScreenPos = Vector2.zero;
        bool isInputActive = false;

        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            targetScreenPos = Touchscreen.current.primaryTouch.position.ReadValue();            
            isInputActive = true;
        }
        else if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            targetScreenPos = Mouse.current.position.ReadValue();            
            isInputActive = true;
        }

        if (isInputActive)
        {
            Vector3 targetWorldPos = Camera.main.ScreenToWorldPoint(targetScreenPos);
            targetWorldPos.z = transform.position.z;            
            if (targetWorldPos.y <= maxBound.y)
            {
                targetWorldPos.x = Mathf.Clamp(targetWorldPos.x, minBound.x, maxBound.x);
                targetWorldPos.y = Mathf.Clamp(targetWorldPos.y, minBound.y + 1, maxBound.y);
                transform.position = Vector3.MoveTowards(transform.position, targetWorldPos, moveSpeed * 3f * Time.deltaTime);
            }
        }
        else
        {
        }
    }
    void InitBounds() 
    {
        Camera mainCamera = Camera.main;
        minBound = mainCamera.ViewportToWorldPoint(Vector2.zero);
        maxBound = mainCamera.ViewportToWorldPoint(Vector2.one);
        float screenHeight = (maxBound.y - minBound.y) * ((verticalBorderRate) / 100) + minBound.y;        
        Vector2 sizeInWorld = playerTexture.bounds.size;        
        minBound = new Vector2(minBound.x + (sizeInWorld.x / 2), minBound.y + (sizeInWorld.y / 2));
        maxBound = new Vector2(maxBound.x - (sizeInWorld.x / 2), screenHeight - (sizeInWorld.y / 2));        
    }
    
    void Fire(CallbackContext context)
    {
        shooter.IsFiring = true;
    }
    void CeaseFire(CallbackContext context)
    {
        shooter.IsFiring = false;
    }
}
