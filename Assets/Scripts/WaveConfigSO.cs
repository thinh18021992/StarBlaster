using JetBrains.Annotations;
using System;
using UnityEngine;


[System.Serializable]
public struct EnemiesWaveInfo
{
    public GameObject enemyPrefab;
    public int amount;
    public float spawnDelay;
}

[CreateAssetMenu(fileName = "WaveConfig", menuName = "New Wave Config")]
public class WaveConfigSO : ScriptableObject
{
    [SerializeField] EnemiesWaveInfo[] enemiesWaveInfo;
    [SerializeField] Transform pathPrefabs;
    [SerializeField] float moveSpeed = 5f;

    public int GetNumberOfWave()
    {
        return enemiesWaveInfo.Length;
    }
    public GameObject GetEnemyPrefab(int i)
    {
        return enemiesWaveInfo[i].enemyPrefab;
    }
    public int GetEnemyAmount(int i)
    {        
        return enemiesWaveInfo[i].amount;
    }
    public void SetEnemyAmount(int i, int value) 
    {   
        enemiesWaveInfo[i].amount = Mathf.Clamp(enemiesWaveInfo[i].amount +1, int.MinValue, 100);
    }
    public float GetSpawnDelay(int i)
    {
        return enemiesWaveInfo[i].spawnDelay;
    }
    public void SetSpawnDelay(int i, float value) 
    {        
        enemiesWaveInfo[i].spawnDelay *= (1 - (value/100));
        enemiesWaveInfo[i].spawnDelay = Mathf.Clamp(enemiesWaveInfo[i].spawnDelay, 0.2f, float.MaxValue);
    }
    public Transform GetStartingWayPoint() 
    {
        return pathPrefabs.GetChild(0);
    }

    public float GetEnemiesMoveSpeed() 
    {
        return moveSpeed;
    }

    public Transform[] GetWayPoints() 
    {
        Transform[] wayPoints = new Transform[pathPrefabs.childCount];
        for (int i = 0; i < wayPoints.Length; i++)
        {
            wayPoints[i] = pathPrefabs.GetChild(i);
        }

        return wayPoints;
    }
}
