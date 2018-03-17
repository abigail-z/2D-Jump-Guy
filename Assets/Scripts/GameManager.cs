using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI finalScoreText;
    public GameObject gameOverScreen;
    public EnemySpawner spawner;
    public float waitTimePerScore;

    public static GameManager instance;
    private uint score;

    private void Awake()
    {
#if UNITY_EDITOR
        Debug.Assert(instance == null);
#endif
        instance = this;
    }

    // Use this for initialization
    void Start ()
    {
        score = 0;
        scoreText.text = score.ToString();
        gameOverScreen.SetActive(false);
    }

    public void IncreaseScore()
    {
        score++;
        scoreText.text = score.ToString();
        spawner.IncreaseSpeed();
    }

    public void UpdateHealth(uint health)
    {
        healthText.text = health.ToString();
    }

    public void GameOver()
    {
        scoreText.gameObject.SetActive(false);
        healthText.gameObject.SetActive(false);
        finalScoreText.text = "Score: " + score;
        gameOverScreen.SetActive(true);
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
