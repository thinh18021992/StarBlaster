using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.VisualScripting.Member;

public class ScoreManager : MonoBehaviour
{
    int score = 0;

    static ScoreManager instance;
    public static ScoreManager GetInstance()
    { 
        return instance;
    }
    private void Awake()
    {
        if (!Ultility.MakeSingleton<ScoreManager>(ref instance, gameObject)) return;
    }
    private void OnEnable()
    {
        EventBus.Subcribe<ObjectDestroyEvent>(OnEnemyDestroy);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        EventBus.Unsubcribe<ObjectDestroyEvent>(OnEnemyDestroy);
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnEnemyDestroy(ObjectDestroyEvent data) 
    {        
        if (data.type == PublicConstant.ObjectType.Enemy)
        {            
            AddScore(data.score); 
        }
    }
    public int GetCurrentScore()
    {
        return score;
    }
    void AddScore(int value) 
    {
        score += value;
        score = Mathf.Clamp(score, 0, int.MaxValue);
        ScoreUpdate action = new ScoreUpdate { value = score };
        EventBus.Publish(action);
    }
    private void ResetScore()
    {
        score = 0;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) 
    {
        switch (scene.name)
        {
            case "MainMenu":
                ResetScore();
                break;
            case "GameScene":
                ResetScore();
                break;
            case "GameOver":
                GetCurrentScore();
                break;
        }
    }
}
