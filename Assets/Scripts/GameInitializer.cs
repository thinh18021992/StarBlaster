using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] GameConfigSO gameConfigSO;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (gameConfigSO != null)
        {
            GameConfig.Initialize(gameConfigSO);
        }
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
    }
}
