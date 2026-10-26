using UnityEngine;

[CreateAssetMenu(fileName = "GameConfigSO", menuName = "Scriptable Objects/GameConfigSO")]
public class GameConfigSO : ScriptableObject
{    
    public int playerMaxHealth;
    public int playerDamage;
    public int playerFireRate;
    public int enemyHealth;
    public int enemyDamage;
    public int enemyFireRate;
    public int enemyBaseScore;
}
