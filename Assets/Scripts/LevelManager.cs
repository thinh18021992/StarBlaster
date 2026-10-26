using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] float delay = 2f;
    [SerializeField] float intervalDifficultyScaleTime = 30f;
    [SerializeField] float scaleRate = 10;
    [SerializeField] int turnToIncreaseScale = 3;
    int counter = 0;        
    private void OnEnable()
    {
        EventBus.Subcribe<ObjectDestroyEvent>(LoadGameOver);        
    }
    private void OnDisable()
    {
        EventBus.Unsubcribe<ObjectDestroyEvent>(LoadGameOver);        
    }
    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "GameScene")
        { StartCoroutine(SetDynamicDifficulty()); }
    }
    public void LoadGame() 
    {
        SceneManager.LoadScene("GameScene");        
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void LoadGameOver(ObjectDestroyEvent data)
    {
        if (data.type == PublicConstant.ObjectType.Player)
        {
            StartCoroutine(GameOverRoutine());
        }        
    }
    IEnumerator GameOverRoutine()
    {
        yield return new WaitForSeconds(delay);
        yield return null;
        SceneManager.LoadScene("GameOver");
    }

    IEnumerator SetDynamicDifficulty()
    {
        while (true) 
        {
            yield return new WaitForSeconds(intervalDifficultyScaleTime);
            DifficultyScaled action = new DifficultyScaled { scaleValue = scaleRate };
            EventBus.Publish(action);
            counter++;
            if (counter == turnToIncreaseScale)
            {
                ChangeScaleRate();
                counter = 0;
            }
        }        
    }
    void ChangeScaleRate() 
    {
        scaleRate = Mathf.Clamp(scaleRate * 1.3f, float.MinValue, 100);
    }
}
