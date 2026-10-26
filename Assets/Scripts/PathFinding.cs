using UnityEngine;

public class PathFinding : MonoBehaviour
{
    WaveConfigSO waveConfig;
    Transform[] waypoints;
    int waypointIndex = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        waypoints = waveConfig.GetWayPoints();
        transform.position = waveConfig.GetStartingWayPoint().position;
    }

    // Update is called once per frame
    void Update()
    {
        FollowPath();
    }
    public void SetWaveConfig(WaveConfigSO inputWaveConfig) 
    {
        waveConfig = inputWaveConfig;
    }
    void FollowPath() 
    {
        if (waypointIndex < waypoints.Length)
        {
            Vector3 targetPosition = waypoints[waypointIndex].position;
            float moveDelta = waveConfig.GetEnemiesMoveSpeed() * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveDelta);
            if (transform.position == targetPosition)
            {
                waypointIndex++;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
