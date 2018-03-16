using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI healthText;
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
}
