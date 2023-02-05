using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentScript : MonoBehaviour
{
    [Header("Tree Control")]
    public Transform TreeObj;
    public Vector2 StartPos;
    public Vector2 EndPos;

    public SpriteRenderer spriteRenderer;

    public List<TreePhase> treePhases;

    private bool IsHurt;

    [Header("EnvironmentColorControl")]
    public Camera Camera;
    public Color StartColor;
    public Color EndColor;

    private float TimeRatio;

    void Start()
    {
        if (TreeObj != null) TreeObj.position = StartPos;
        if (GameEventManager.instance != null) GameEventManager.instance.RootCrushed.AddListener(RootCrushed);
        if (Camera != null) Camera.backgroundColor = StartColor;
    }

    private void Update()
    {
        if (Camera != null) Camera.backgroundColor = Color.Lerp(StartColor, EndColor, TimeRatio / 60f);
    }

    public void SetTreeRatio(float i_ratio)
    {
        TimeRatio = i_ratio;
        SetTreeImage();

    }
    void SetTreeImage()
    {
        if (GetCurrentPhase(TimeRatio) == null) return;
        if (IsHurt)
        {
            spriteRenderer.sprite = GetCurrentPhase(TimeRatio).HurtImage;
        }
        else
        {
            spriteRenderer.sprite = GetCurrentPhase(TimeRatio).NormalImage;
        }
    }

    public TreePhase GetCurrentPhase(float i_ratio)
    {
        for (int index = treePhases.Count - 1; index >= 0; index--)
        {
            if (i_ratio >= treePhases[index].StartRatio)
            {
                return treePhases[index];
            }
        }
        return null;
    }

    public void RootCrushed(int Id_1, int Id_2, float Yee)
    {
        TreePhase curP = GetCurrentPhase(TimeRatio);
        if (++curP.cur_Count >= curP.MakeSoundCount)
        {
            curP.cur_Count = 0;
            if (GameSoundController.Instance != null) GameSoundController.Instance.PlaySound(curP.SoundData, 0.2f);
        }
        if (!IsHurt)
            StartCoroutine(TreeHurt());
    }



    public void SetTreePos(float i_Ratio)
    {
        Vector2 TargetPos = ((StartPos - EndPos) * (i_Ratio)) + EndPos;
        TargetPos = new Vector2(Mathf.Clamp(TargetPos.x, EndPos.x, StartPos.x), Mathf.Clamp(TargetPos.y, EndPos.y, StartPos.y));
        TreeObj.position = (Vector3)TargetPos;
    }

    IEnumerator TreeHurt()
    {
        IsHurt = true;
        SetTreeImage();
        yield return new WaitForSeconds(1f);
        IsHurt = false;
        SetTreeImage();
    }
}
