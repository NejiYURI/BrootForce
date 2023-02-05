using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;



public class MainGameManager : MonoBehaviour
{
    public static MainGameManager mainGameManager;

    public float MaxTime;
    [SerializeField]
    private float RemainTime;
    [SerializeField]
    private float TimeCounter;

    public float RootPunishSpeedMul = 0.5f;

    private bool GameStart;
    [Header("Spawn Control")]
    public float SpawnFreq = 1f;
    [SerializeField]
    private float SpawnFreqMul;
    public List<Transform> SpawnPos;
    [SerializeField]
    private Dictionary<int, Transform> EnableSpawnList;
    private Dictionary<int, Transform> DisableSpawnList;
   
    public EnvironmentScript environmentControl;

    public AudioClip TreeCreak;
    public AudioClip RootSpawn;
    private bool CanPlayCreakSound;

    [Header("UI Control")]
    public GameObject GameOverUI;
    public TextMeshProUGUI TimeResultTxt;

    private Dictionary<GameObject, float> GrowMul;

    public List<SpawnItemSetting> RootObj;

    private void Awake()
    {
        mainGameManager = this;
    }
    void Start()
    {

        if (GameEventManager.instance != null) GameEventManager.instance.RootCrushed.AddListener(RootCrushed);
        RemainTime = MaxTime;
        TimeCounter = 0;

        GrowMul = new Dictionary<GameObject, float>();
        SpawnFreqMul = 1f;
        EnableSpawnList = new Dictionary<int, Transform>();
        DisableSpawnList = new Dictionary<int, Transform>();
        int SpawnCounter = 1;
        foreach (var item in SpawnPos)
        {
            EnableSpawnList.Add(SpawnCounter++, item);
        }
        GameStart = true;
        CanPlayCreakSound = true;
        StartCoroutine(SpawnTimer());

    }

    private void Update()
    {
        SpawnFreqMul = Mathf.Clamp(1 - (((TimeCounter * TimeCounter) / 5) * 0.002f), 0.2f, 1f);
        if (GameStart)
        {
            float DelTime = Time.deltaTime;
            TimeCounter += Time.deltaTime;
            RemainTime -= DelTime + (GetGrowMul() * DelTime);
            if (environmentControl != null)
            {
                environmentControl.SetTreePos(RemainTime / MaxTime);
                environmentControl.SetTreeRatio((TimeCounter / 50f) * 100f);
            }
            if (RemainTime <= 0) GameOverFunc();
        }

    }
    void RootCrushed(int SpawnId, int SpawnRootIndex, float CrushDamage)
    {
        RemainTime = Mathf.Clamp(RemainTime + CrushDamage, 0f, MaxTime);
        if (DisableSpawnList.ContainsKey(SpawnId))
        {
            StartCoroutine(SpawnPosCoolDown(SpawnId));
        }
        if (RootObj[SpawnRootIndex] != null) RootObj[SpawnRootIndex].AmountChange(-1);
    }

    void GameOverFunc()
    {
        GameStart = false;
        if (GameEventManager.instance != null) GameEventManager.instance.GameOverEvent.Invoke();
        if (GameOverUI != null) GameOverUI.SetActive(true);
        if (TimeResultTxt != null) TimeResultTxt.text = TimeCounter.ToString("0.#") + " S!";
        StopAllCoroutines();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ToTitle()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void AddGrowSpeed(GameObject i_obj)
    {
        if (GrowMul.ContainsKey(i_obj)) return;
        if (CanPlayCreakSound) StartCoroutine(PlayCreakSound());
        GrowMul.Add(i_obj, RootPunishSpeedMul);
    }

    public void ReduceGrowSpeed(GameObject i_obj)
    {
        if (!GrowMul.ContainsKey(i_obj)) return;
        GrowMul.Remove(i_obj);
    }

    IEnumerator SpawnTimer()
    {
        while (true)
        {

            if (RootObj.Count > 0)
            {
                GetSpawnResult getPos = GetSpawnPos();
                bool CanSpawn = false;
                foreach (var item in RootObj)
                {
                    if (item.CanSpawn()) CanSpawn = true;
                }
                if (getPos.IsSuccess && CanSpawn)
                {
                    int RandomRootIndex = Random.Range(0, RootObj.Count);
                    int RandomRate = Random.Range(0, 101);
                    while (!RootObj[RandomRootIndex].CanSpawn(RandomRate))
                    {
                        RandomRootIndex = Random.Range(0, RootObj.Count);
                        RandomRate = Random.Range(0, 101);
                    }
                    GameObject obj = Instantiate(RootObj[RandomRootIndex].obj, getPos.Pos, Quaternion.identity);
                    RootObj[RandomRootIndex].AmountChange(1);
                    if (obj.GetComponent<RootScript>())
                    {
                        obj.GetComponent<RootScript>().SpawnPosId = getPos.SpawnPosId;
                        obj.GetComponent<RootScript>().SpawnRootIndex = RandomRootIndex;
                    }
                    if (GameSoundController.Instance != null) GameSoundController.Instance.PlaySound(RootSpawn, 0.2f);
                }
            }
            yield return new WaitForSeconds(SpawnFreq * SpawnFreqMul);
        }
    }

    IEnumerator PlayCreakSound()
    {
        CanPlayCreakSound = false;
        if (GameSoundController.Instance != null) GameSoundController.Instance.PlaySound(TreeCreak, 0.2f);
        yield return new WaitForSeconds(3f);
        CanPlayCreakSound = true;
    }

    IEnumerator SpawnPosCoolDown(int SpawnId)
    {
        yield return new WaitForSeconds(1f);
        if (DisableSpawnList.ContainsKey(SpawnId))
        {
            EnableSpawnList.Add(SpawnId, DisableSpawnList[SpawnId]);
            DisableSpawnList.Remove(SpawnId);
        }
    }

    private GetSpawnResult GetSpawnPos()
    {
        GetSpawnResult rslt = new GetSpawnResult();
        int GetNum = Random.Range(1, 9);
        if (EnableSpawnList.Count <= 0)
        {
            Debug.Log("Oops, no place for new root;");
            return rslt;
        }
        while (!EnableSpawnList.ContainsKey(GetNum))
        {
            GetNum = Random.Range(1, 9);
        }
        rslt.Pos = EnableSpawnList[GetNum].position;
        rslt.SpawnPosId = GetNum;
        rslt.IsSuccess = true;
        DisableSpawnList.Add(GetNum, EnableSpawnList[GetNum]);
        EnableSpawnList.Remove(GetNum);
        return rslt;
    }

    private float GetGrowMul()
    {
        float rslt = 0;
        foreach (var item in GrowMul)
        {
            rslt += item.Value;
        }
        return rslt;
    }
}
