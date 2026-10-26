using System.Collections;
using UnityEngine;

public class SpawnItems : MonoBehaviour
{
    [SerializeField] GameObject[] powerUp;
    [SerializeField] float intervalSpawnDelay = 10f;
    
    private void Start()
    {
        for (int i = 0; i < powerUp.Length; i++)
        {
            powerUp[i].SetActive(false);
        }
        StartCoroutine(SpawnItemsRoutine());
    }

    IEnumerator SpawnItemsRoutine() 
    {
        while (true)
        {
            int random = Random.Range(0, powerUp.Length);
            yield return new WaitForSeconds(intervalSpawnDelay);
            powerUp[random].SetActive(true);
        }        
    }

}
