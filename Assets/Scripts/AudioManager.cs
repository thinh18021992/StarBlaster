using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("BGM")]
    [SerializeField] AudioClip mainMenuBGM;
    [SerializeField] AudioClip gameBGM;
    [Header("Shooting SFX")]
    [SerializeField] AudioClip shootingClip;
    [SerializeField][Range(0, 1)] float shootingVolume = 1f;
    [Header("Hit SFX")]
    [SerializeField] AudioClip hittingClip;
    [SerializeField][Range(0, 1)] float hitVolume = 1f;
    [Header("Scale Difficulty")]
    [SerializeField] AudioClip difficultyClip;
    [SerializeField][Range(0, 1)] float difficultyVolume = 1f;
    [Header("Power Up")]
    [SerializeField] AudioClip powerUpClip;
    [SerializeField][Range(0, 1)] float powerUpVolume = 1f;

    public static AudioManager instance;
    private void Awake()
    {
        if (!Ultility.MakeSingleton<AudioManager>(ref instance, gameObject)) return;
    }
    void ManageSingleton()
    {
        int instanceCount = FindObjectsByType<AudioManager>(FindObjectsInactive.Include).Length;
        if (instanceCount > 1)
        {
            Destroy(gameObject);
        }
        else
        { 
            DontDestroyOnLoad(gameObject);
        }
    }
    private void OnEnable()
    {        
        EventBus.Subcribe<PlayerShooting>(PlayShootingSFX);
        EventBus.Subcribe<EnemyTakeDamageEvent>(PlayHittingSFX);
        EventBus.Subcribe<GetPowerUp>(PlayPoweredUp);
        EventBus.Subcribe<DifficultyScaled>(PlayDifficultyChanged);
        SceneManager.sceneLoaded += ChangeBGM;        
    }
    private void OnDisable()
    {
        EventBus.Unsubcribe<PlayerShooting>(PlayShootingSFX);
        EventBus.Unsubcribe<EnemyTakeDamageEvent>(PlayHittingSFX);
        EventBus.Unsubcribe<GetPowerUp>(PlayPoweredUp);
        EventBus.Unsubcribe<DifficultyScaled>(PlayDifficultyChanged);
        SceneManager.sceneLoaded -= ChangeBGM;        
    }
    void PlayShootingSFX(PlayerShooting eventData)
    {   
        if (shootingClip != null) 
        {
            AudioSource.PlayClipAtPoint(shootingClip, eventData.position, shootingVolume);
        }
    }
    void PlayHittingSFX(EnemyTakeDamageEvent eventData)
    {
        if (hittingClip != null)
        {
            AudioSource.PlayClipAtPoint(hittingClip, eventData.position, hitVolume);
        }
    }
    void PlayPoweredUp(GetPowerUp data)
    {
        AudioSource.PlayClipAtPoint(powerUpClip, transform.position, powerUpVolume);
    }
    void PlayDifficultyChanged(DifficultyScaled data)
    {
        AudioSource.PlayClipAtPoint(difficultyClip, transform.position, difficultyVolume);
    }
    void ChangeBGM(Scene scene, LoadSceneMode mode)
    {        
        AudioSource source = GetComponent<AudioSource>();
        if (source != null)
        {
            source.Stop();
            switch (scene.name)
            {
                case "MainMenu":
                    source.clip = mainMenuBGM;
                    break;
                case "GameScene":
                    source.clip = gameBGM;
                    break;                
            }
            source.Play();
        }
    }
}
