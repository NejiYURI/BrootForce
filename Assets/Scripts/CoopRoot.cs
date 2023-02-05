using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoopRoot : RootScript
{
    private int StompCount;

    private bool IsCoroutineRunning;

    public override void GetStomped()
    {
        StompCount++;
        StartCoroutine(ShakeFunc());
        if (!IsCoroutineRunning) StartCoroutine(StompCounter());
        if (StompCount >= 2)
        {
            Health--;
            if (Health <= 0)
            {
                if (GameEventManager.instance != null)
                {
                    GameEventManager.instance.RootCrushed.Invoke(SpawnPosId, SpawnRootIndex, CrushDamage);
                    GameEventManager.instance.RootDestroyAll.Invoke();
                } 
                if (MainGameManager.mainGameManager != null)
                {
                    MainGameManager.mainGameManager.ReduceGrowSpeed(gameObject);
                }
                Destroy(gameObject, 0.1f);
            }
        }
    }

    IEnumerator StompCounter()
    {
        IsCoroutineRunning = true;
        yield return new WaitForSeconds(0.2f);
        StompCount = 0;
        IsCoroutineRunning = false;
    }
}
