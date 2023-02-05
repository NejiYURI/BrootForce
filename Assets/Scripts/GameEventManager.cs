using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventManager : MonoBehaviour
{
    public static GameEventManager instance;
    private void Awake()
    {
        instance = this;
    }

    public UnityEvent<int,int,float> RootCrushed;
    public UnityEvent GameOverEvent;

    public UnityEvent RootDestroyAll;
}
