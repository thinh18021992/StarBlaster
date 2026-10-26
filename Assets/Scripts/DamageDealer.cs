using UnityEngine;
using static PublicConstant;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] int damage = 10;
    [SerializeField] ObjectType type;

    private void Awake()
    {
        if (type == ObjectType.Player)
        { damage = GameConfig.GetPlayerDamage(); }
        else 
        { damage = GameConfig.GetEnemyDamage(); }
        
    }
    public int GetDamage() 
    {
        return damage;
    }
    public void SetDamage(int value)
    {
        damage = value;
    }
    private void OnEnable()
    {
        if (type == ObjectType.Player)
        {
            EventBus.Subcribe<GetPowerUp>(OnPlayerPowerUp);
        }        
    }

    private void OnDisable()
    {
        if (type == ObjectType.Player)
        {
            EventBus.Unsubcribe<GetPowerUp>(OnPlayerPowerUp);
        }
    }

    void OnPlayerPowerUp(GetPowerUp data)
    {
        if (data.type == PowerupTypes.Attack && type == ObjectType.Player)
        {
            damage += (int)data.value;
        }        
    }   
}
