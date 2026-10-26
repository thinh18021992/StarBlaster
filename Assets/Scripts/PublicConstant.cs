using UnityEngine;
using static PublicConstant;

public static class PublicConstant
{
    public enum ObjectType
    {
        Player,
        Enemy,
        Item
    }
    public enum PowerupTypes
    {
        Attack,
        FireRate,
        Health
    }    
}
public static class Ultility 
{
    public static float RandomFireRate(float baseValue, float minVariance, float variance)
    {
        return Mathf.Clamp(Random.Range(baseValue - variance, baseValue + variance), minVariance, float.MaxValue);
    }
    public static bool MakeSingleton<T>(ref T currentInstance, GameObject gameObjectToDestroy, bool dontDestroyOnLoad = true) where T : MonoBehaviour
    { 
        int instanceCount = Object.FindObjectsByType<T>(FindObjectsInactive.Include).Length;
        if (instanceCount > 1)
        {
            gameObjectToDestroy.SetActive(false);
            Object.Destroy(gameObjectToDestroy);
            return false;
        }
        if (currentInstance == null) 
        {
            currentInstance = gameObjectToDestroy.GetComponent<T>();            
        }
        Object.DontDestroyOnLoad(gameObjectToDestroy);
        return true;
    }
}
public struct ObjectDestroyEvent
{
    public Vector3 position;
    public int score;
    public ObjectType type;
}

public struct EnemyTakeDamageEvent
{
    public Vector3 position;
    public int currentHealth;    
}
public struct PlayerTakeDamageEvent
{
    public Vector3 position;
    public int currentHealth;
    public int maxHealth;
}
public struct PlayerHealthChanged
{
    public Vector3 position;
    public int currentHealth;
    public int maxHealth;
    public bool isRecovery;
}
public struct PlayerShooting
{ 
    public Vector3 position;    
}
public struct ScoreUpdate
{
    public int value;
}

public struct GetPowerUp
{
    public PublicConstant.PowerupTypes type;
    public float value;
}
public struct DifficultyScaled 
{
    public float scaleValue;
}

public static class ExtensionMethods
{
    public static float ToPercentRate(this float value) 
    {
        return value / 100;
    }
    public static float ToPercentRate(this double value)
    {
        return (float)(value / 100);
    }
    public static float ToPercentRate(this int value)
    {
        return (float)value / 100;
    }
}
