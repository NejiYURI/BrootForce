using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicRoot : RootScript
{
    public override void StartFunc()
    {
        this.spriteRenderer.flipX = (Random.Range(0, 2) > 0);
        if (GameEventManager.instance != null) GameEventManager.instance.RootDestroyAll.AddListener(GetStomped);
    }

    private void OnDestroy()
    {
        GameEventManager.instance.RootDestroyAll.RemoveListener(GetStomped);
    }
}
