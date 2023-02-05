using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMController : MonoBehaviour
{
    private AudioSource audioSource;

    public float LoopStartTime;
    public float LoopEndTime;

    private bool IsGameOver;
    void Start()
    {
        if (GameEventManager.instance != null) GameEventManager.instance.GameOverEvent.AddListener(GameOverFunc);
        audioSource = this.GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (audioSource.time >= LoopEndTime && !IsGameOver)
        {
            audioSource.time = LoopStartTime;
        }
    }

    void GameOverFunc()
    {
        IsGameOver = true;
    }
}
