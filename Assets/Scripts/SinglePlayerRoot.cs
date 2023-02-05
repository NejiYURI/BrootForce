using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePlayerRoot : RootScript
{

    public override void GetStomped()
    {
        StartCoroutine(ShakeFunc());
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
            Destroy(gameObject,0.1f);
        }
    }
}
