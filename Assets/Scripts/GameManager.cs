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
    public GameObject pauseScreen;
    public EnemySpawner spawner;
    public float waitTimePerScore;

    public static GameManager instance;
    private uint score;
    private bool paused;

    private void Awake()
    {
#if UNITY_EDITOR
        Debug.Assert(instance == null);
#endif
        instance = this;
    }

    public void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            paused = !paused;
            Time.timeScale = paused ? 0 : 1;
            pauseScreen.SetActive(paused);
            Cursor.visible = paused;
        }
    }

    // Use this for initialization
    void Start ()
    {
        score = 0;
        paused = false;
        scoreText.text = score.ToString();
        gameOverScreen.SetActive(false);
        pauseScreen.SetActive(false);

        Cursor.visible = false;
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

        Cursor.visible = true;
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }

}
