using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private uint score;

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start ()
    {
        score = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void IncreaseScore()
    {
        score += 1;
    }
}
