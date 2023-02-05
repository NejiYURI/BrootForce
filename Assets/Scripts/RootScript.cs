using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RootScript : MonoBehaviour
{

    public int Health;

    public int SpawnPosId;

    public int SpawnRootIndex;

    public SpriteRenderer spriteRenderer;
    public List<RootPhase> rootPhases;

    public float CrushDamage;

    private Vector3 OriginScale;

    [Range(0.1f, 10f)]
    public float GrowTime;
    [SerializeField]
    private float CurTime;

    void Start()
    {
        StartFunc();
    }


    private void FixedUpdate()
    {
        FixedUpdateFunc();
    }

    public virtual void StartFunc()
    {
    }

    public virtual void FixedUpdateFunc()
    {
        if (Health <= 0) return;
        CurTime = Mathf.Clamp(CurTime + Time.fixedDeltaTime, 0f, GrowTime);
        GetCurrentPhase(CurTime / GrowTime);
    }

    public virtual void GetCurrentPhase(float Ratio)
    {
        for (int index = rootPhases.Count - 1; index >= 0; index--)
        {
            if (Ratio >= rootPhases[index].StartRatio && spriteRenderer != null)
            {
                spriteRenderer.sprite = rootPhases[index].PhaseImage;
                if (MainGameManager.mainGameManager != null)
                {
                    if (rootPhases[index].TreeSpeedMultiply)
                    {
                        MainGameManager.mainGameManager.AddGrowSpeed(gameObject);
                    }
                    else
                    {
                        MainGameManager.mainGameManager.ReduceGrowSpeed(gameObject);
                    }
                }
                return;
            }
        }
    }

    public void ResetState()
    {
        CurTime = 0;
    }

    public virtual void GetStomped()
    {
        Health--;
        StartCoroutine(ShakeFunc());
        CurTime -= 0.2f;
        if (Health <= 0)
        {
            if (GameEventManager.instance != null)
            {
                GameEventManager.instance.RootCrushed.Invoke(SpawnPosId, SpawnRootIndex, CrushDamage);

            }
            if (MainGameManager.mainGameManager != null)
            {
                MainGameManager.mainGameManager.ReduceGrowSpeed(gameObject);
            }
            Destroy(gameObject,0.1f);
        }
    }

    public IEnumerator ShakeFunc()
    {
        Vector2 originPos = this.transform.position;
        for (int i = 0; i < 5; i++)
        {
            this.transform.position = originPos + new Vector2(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f));
            yield return new WaitForSeconds(0.06f);
        }
        this.transform.position = originPos;
    }
}
