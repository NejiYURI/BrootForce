using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GetSpawnResult
{
    public bool IsSuccess;
    public int SpawnPosId;
    public Vector2 Pos;
}

public class SpawnPointObj
{
    public SpawnPointObj(Transform i_Pos)
    {
        SpawnPos = i_Pos;
        CanSpawn = true;
    }
    public Transform SpawnPos;
    public bool CanSpawn;
}

[System.Serializable]
public class RootPhase
{
    public Sprite PhaseImage;
    public float StartRatio;
    public bool TreeSpeedMultiply;
}

[System.Serializable]
public class TreePhase
{
    public Sprite NormalImage;
    public Sprite HurtImage;
    public float StartRatio;
    public int MakeSoundCount;
    public int cur_Count;
    public AudioClip SoundData;
}

[System.Serializable]
public class SpawnItemSetting
{
    public string Name;
    public GameObject obj;

    public int SpawnRate;

    public int Amount_min;
    public int Amount_Max;

    [SerializeField]
    private int Amount = 0;

    public bool CanSpawn()
    {
        return (obj != null) && (Amount >= Amount_min && Amount < Amount_Max);
    }

    public bool CanSpawn(int Rate)
    {
        return (obj != null) && (Amount >= Amount_min && Amount < Amount_Max) && Rate >= SpawnRate;
    }
    public void AmountChange(int val)
    {
        Amount = Mathf.Clamp(Amount + val, Amount_min, Amount_Max);
    }
}