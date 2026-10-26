using System;
using UnityEngine;

public static class GameConfig
{
    private static GameConfigSO config;

    private static int playerMaxHealth;
    private static int playerDamage;
    private static float playerFireRate;
    private static int enemyHealth;
    private static int enemyDamage;
    private static float enemyFireRate;
    private static int enemyBaseScore;
    public static void Initialize(GameConfigSO so)
    {
        config = so;
        playerMaxHealth = config.playerMaxHealth;
        playerDamage = config.playerDamage;
        playerFireRate = config.playerFireRate;
        enemyHealth = config.enemyHealth;
        enemyDamage = config.enemyDamage;
        enemyFireRate = config.enemyFireRate;
        enemyBaseScore = config.enemyBaseScore;

        UnsubcribeDifficultyEvent();
        SubcribeDifficultyEvent();
}
    public static int GetPlayerHealth() 
    {
        return playerMaxHealth;
    }
    public static int GetPlayerDamage()
    {
        return playerDamage;
    }
    public static float GetPlayerFireRate()
    {
        return playerFireRate;
    }
    public static int GetEnemyHealth()
    {
        return enemyHealth;
    }
    public static int GetEnemyDamage()
    {
        return enemyDamage;
    }
    public static float GetEnemyFireRate()
    {
        return enemyFireRate;
    }
    public static int GetEnemyBaseScore()
    {
        return enemyBaseScore;
    }
    public static void SubcribeDifficultyEvent()
    {
        EventBus.Subcribe<DifficultyScaled>(OnDifficultyScaled);
    }

    public static void UnsubcribeDifficultyEvent()
    {
        EventBus.Unsubcribe<DifficultyScaled>(OnDifficultyScaled);
    }

    public static void OnDifficultyScaled(DifficultyScaled data) 
    {
        enemyDamage += (int)data.scaleValue;
        enemyFireRate *= (1 - data.scaleValue.ToPercentRate());
        enemyHealth += (int)data.scaleValue;
        enemyBaseScore += (int)data.scaleValue;
    }
}
