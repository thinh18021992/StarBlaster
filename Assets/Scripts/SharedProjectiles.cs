using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct ProjectilePrefabs
{
    public GameObject projectile;
    public int amount;
}
public class SharedProjectiles : MonoBehaviour
{
    public static SharedProjectiles sharedProjectiles;
    [SerializeField] ProjectilePrefabs[] projectilePrefabs;
    Dictionary<GameObject, Queue<GameObject>> poolDictionary = new Dictionary<GameObject, Queue<GameObject>>();
    

    void Awake()
    {
        if (sharedProjectiles == null)
        {
            sharedProjectiles = this;
        }
        else
        {
            Destroy(gameObject);
        }
        InitPool();
    }
    
    
    void InitPool() 
    {
        foreach (ProjectilePrefabs prefab in projectilePrefabs)
        {
            Queue<GameObject> tempQueue = new Queue<GameObject>();
            for (int i = 0; i < prefab.amount; i++)
            {
                var temp = Instantiate(prefab.projectile, transform.position, Quaternion.identity);
                temp.SetActive(false);
                tempQueue.Enqueue(temp);
            }
            poolDictionary.Add(prefab.projectile, tempQueue);

        }
    }
    public GameObject GetProjectile(GameObject prefab, Vector3 position, Vector2 direction, Quaternion rotation, float speed, float lifeTime, int damage)
    {
        if(prefab == null)
    {
            Debug.LogError("Prefab null!");
            return null;
        }
        if (!poolDictionary.ContainsKey(prefab)) { poolDictionary.Add(prefab, new Queue<GameObject>()); }
        Queue<GameObject> queue = poolDictionary[prefab];

        GameObject projectile;
        if (queue.Count > 0)
        {
            projectile = queue.Dequeue();
        }
        else
        {
            projectile = Instantiate(prefab, transform.position, Quaternion.identity);
            projectile.SetActive(false);
            //Create new
        }
        projectile.transform.position = position;
        projectile.transform.rotation = rotation;
        projectile.GetComponent<Projectile>().Construct(prefab, direction, speed, lifeTime, damage);
        projectile.SetActive(true);
        return projectile;
    }

    public void ReturnToPool(GameObject prefab, GameObject obj) 
    {
        if (poolDictionary.ContainsKey(prefab))
        {
            poolDictionary[prefab].Enqueue(obj);
        }
        else
        {
            Destroy(obj);
        }
    }
}
