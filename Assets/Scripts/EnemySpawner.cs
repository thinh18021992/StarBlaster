using System;
using System.Collections;
using Unity.VisualScripting;

//using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] WaveConfigSO[] waveConfigs;
    WaveConfigSO[] cloneWaveConfigs;
    [Range(0.5f, float.MaxValue)][SerializeField] float delayBetweenWave;
    [SerializeField] bool isLooping;    
    [SerializeField] int damage;
    void Start()
    {
        CloneWaveConfigs();
        StartCoroutine(SpawnEnemies());
        damage = GameConfig.GetEnemyDamage();        
    }

    private void OnEnable() 
    {
        EventBus.Subcribe<DifficultyScaled>(OnDifficultyScaled);
    }
    private void OnDisable()
    {
        EventBus.Unsubcribe<DifficultyScaled>(OnDifficultyScaled);
    }

    IEnumerator SpawnEnemies()
    {
        do
        {
            foreach (WaveConfigSO wave in cloneWaveConfigs)
            {
                WaveConfigSO currentWave;
                currentWave = wave;
                for (int j = 0; j < currentWave.GetNumberOfWave(); j++)
                {
                    float delay = currentWave.GetSpawnDelay(j);
                    int enemyAmount = currentWave.GetEnemyAmount(j);
                    for (int i = 0; i < enemyAmount; i++)
                    {
                        GameObject newEnemy = Instantiate(currentWave.GetEnemyPrefab(j), currentWave.GetStartingWayPoint().position, Quaternion.identity, this.transform);
                        newEnemy.GetComponent<PathFinding>().SetWaveConfig(currentWave);
                        newEnemy.GetComponent<DamageDealer>().SetDamage(damage);
                        yield return new WaitForSeconds(delay);
                    }
                }
                yield return new WaitForSeconds(delayBetweenWave);
            }
            cloneWaveConfigs = RearrangeWave();
        }
        while (isLooping);             
    }
    WaveConfigSO[] RearrangeWave()
    {
        for (int i = 0; i < cloneWaveConfigs.Length; i++) 
        {
            int ranIndex = UnityEngine.Random.Range(i, cloneWaveConfigs.Length);
            var temp = cloneWaveConfigs[i];
            cloneWaveConfigs[i] = cloneWaveConfigs[ranIndex];
            cloneWaveConfigs[ranIndex] = temp;            
        }
        return cloneWaveConfigs;        
    }
    void OnDifficultyScaled(DifficultyScaled data)
    {        
        damage += (int)data.scaleValue;
        foreach (WaveConfigSO wave in cloneWaveConfigs)
        {            
            for (int i = 0; i < wave.GetNumberOfWave(); i++)
            {                
                wave.SetEnemyAmount(i, (int)data.scaleValue);
                wave.SetSpawnDelay(i, data.scaleValue);
            }
        }
    }
    void CloneWaveConfigs() 
    {
        cloneWaveConfigs = new WaveConfigSO[waveConfigs.Length];
        for (int i = 0; i < waveConfigs.Length; i++) 
        {
            cloneWaveConfigs[i] = Instantiate(waveConfigs[i]);
        }
    }
}
