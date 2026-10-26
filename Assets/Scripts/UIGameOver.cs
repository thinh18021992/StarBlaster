using TMPro;
using UnityEngine;

public class UIGameOver : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    int score = 0;

    private void Awake()
    {
        score = ScoreManager.GetInstance().GetCurrentScore();
        scoreText.text = score.ToString();
    }
}
