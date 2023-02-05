using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSetting : MonoBehaviour
{
    public static VolumeSetting Instance;
    public float VolumeMax = 20f;
    public float Volumemin = -20;
    public AudioMixer SoundSetting;
    public GameObject SettingGroup;

    private bool IsShowPanel;

    public Slider BGMSlider;
    public Slider SoundSlider;
    // Start is called before the first frame update
    private void Awake()
    {
        if (VolumeSetting.Instance != null) Destroy(gameObject);
        Instance = this;
    }
    private void Start()
    {
      
        DontDestroyOnLoad(this);
        BGMVolChange();
        SVolChange();
        if (SettingGroup != null)
            SettingGroup.SetActive(false);
    }
    public void BGMVolChange()
    {
        SoundSetting.SetFloat("BGMVol", ((VolumeMax - Volumemin) * BGMSlider.value) + Volumemin);
    }

    public void SVolChange()
    {
        SoundSetting.SetFloat("SoundVol", ((VolumeMax - Volumemin) * SoundSlider.value) + Volumemin);
    }

    public void TogglePanel()
    {
        IsShowPanel = !IsShowPanel;
        if (SettingGroup != null)
            SettingGroup.SetActive(IsShowPanel);
    }
}
