using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static PublicConstant;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI hpText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] Slider hpBar;
    [SerializeField] Button pauseButton;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] Button resumeButton;
    [SerializeField] Button quitButton;
    bool isPressed = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        UpdateHPUI(GameConfig.GetPlayerHealth(), GameConfig.GetPlayerHealth());
        UpdateScoreUI(0);        
        pauseMenu.SetActive(false);
    }
    private void OnEnable()
    {
        EventBus.Subcribe<ScoreUpdate>(OnScoreUpdate);
        EventBus.Subcribe<PlayerTakeDamageEvent>(OnPlayerTakeDamage);
        EventBus.Subcribe<PlayerHealthChanged>(OnPlayerHealthChanged);
    }
    private void OnDisable()
    {
        EventBus.Unsubcribe<ScoreUpdate>(OnScoreUpdate);
        EventBus.Unsubcribe<PlayerTakeDamageEvent>(OnPlayerTakeDamage);
        EventBus.Unsubcribe<PlayerHealthChanged>(OnPlayerHealthChanged);
    }

    void OnScoreUpdate(ScoreUpdate data)
    {
        UpdateScoreUI(data.value);
    }
    void OnPlayerTakeDamage(PlayerTakeDamageEvent data)
    {        
        UpdateHPUI(data.currentHealth, data.maxHealth);
    }
    void OnPlayerHealthChanged(PlayerHealthChanged data)
    {        
        UpdateHPUI(data.currentHealth, data.maxHealth);     
    }
    void UpdateHPUI(int current, int max) 
    {        
        hpText.text = $"{current}/{max}";
        hpBar.maxValue = (float)max;
        hpBar.value = current;
    }
    void UpdateScoreUI(int value) 
    {
        scoreText.text = value.ToString("0000000000");
    }
    public void PauseGame()
    {
        if (isPressed) { return; }
        pauseMenu.SetActive(true);        
        Time.timeScale = 0f;
        isPressed = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPressed = false;
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
