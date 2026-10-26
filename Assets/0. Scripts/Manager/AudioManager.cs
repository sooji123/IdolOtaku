using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class AudioManager : Singleton<AudioManager>
{
    public AudioSource bgmPlayer;
    public AudioSource clickSFX;
    public AudioSource closeSFX;
    public AudioSource scheduleSuccessSFX;
    public AudioSource scheduleFailSFX;
    
    public List<Button> clickButtons = new();
    public List<Button> closeButtons = new();

    [SerializeField] private Slider bgmVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    [SerializeField] private List<BGMSource> bgmSources = new();
    
    public GameObject optionUI;
    
    [SerializeField] private BGMType bgmType;

    /// <summary>
    /// Slider 연결하면 아마 잘 동작할듯요?
    /// </summary>
    
    #region Built in Events
    public void Start() => RegisterEvents();

    public void Update() => UpdateBgmStateMachine();

    #endregion
    
    #region Registeration
    private void RegisterEvents()
    {
        RegisterSfxEvent();
        RegisterVolumeEvent();
    }

    private void RegisterSfxEvent()
    {
        foreach (var clickButton in clickButtons)
            clickButton.onClick.AddListener(() => PlayClick());
        
        foreach (var closeButton in closeButtons)
            closeButton.onClick.AddListener(() => PlayClose());
    }

    private void RegisterVolumeEvent()
    {
        bgmVolumeSlider.onValueChanged.AddListener(value => ChangeBgmVolume(value));
        sfxVolumeSlider.onValueChanged.AddListener(value => ChangeSfxVolume(value));
    }
    #endregion
    
    #region Play Sound Effect
    public void PlayClick() => clickSFX.Play();

    public void PlayClose() => closeSFX.Play();

    public void PlayScheduleResultSFX(ResultType resultType)
    {
        if (resultType == ResultType.Perfect || resultType == ResultType.Success)
            scheduleSuccessSFX.Play();
        else
            scheduleFailSFX.Play();
    }
    #endregion
    
    #region Volume Control

    private float ChangeBgmVolume(float value)
    {
        bgmPlayer.volume = value;

        return value;
    }

    private float ChangeSfxVolume(float value)
    {
        clickSFX.volume = value;
        closeSFX.volume = value;

        return value;
    }

    // 팝업 닫히면 호출해서 저장 (일단 로직은 구현 안 해도 될듯요)
    private void SaveBgmVolume(float value)
    {
    }

    private void SaveSfxVolume(float value)
    {
    }

    public void OnClickOptionOpen()
    {
        optionUI.SetActive(true);
    }

    public void OnClickOptionClose()
    {
        optionUI.SetActive(false);
        ChangeBgmVolume(bgmVolumeSlider.value);
        ChangeSfxVolume(sfxVolumeSlider.value);
    }
    #endregion
    
    #region BGM

    private void UpdateBgmStateMachine()
    {
        var uiType = GameManager.Instance.GetCurrentUIType();
        
        BGMType curBgmType;

        switch (uiType)
        {
            case UIType.MainMenu :  curBgmType = BGMType.Main; break;
            case UIType.Schedule :  curBgmType = BGMType.Schedule; break;
            case UIType.ScheduleProcessing : curBgmType = BGMType.ScheduleProcessing; break;
            
            default: curBgmType = BGMType.Main; break;
        }

        if (curBgmType != bgmType)
        {
            bgmType = curBgmType;
            OnBgmStateChanged();
        }
    }

    private void OnBgmStateChanged() => PlayBgm(GetBgmClipByBgmState(bgmType));

    private AudioClip GetBgmClipByBgmState(BGMType type)
    {
        foreach (var source in bgmSources)
                if (source.type == type)
                    return source.clip;

        return null;
    }

    private void PlayBgm(AudioClip clip)
    {
        bgmPlayer.clip = clip;
        bgmPlayer.Play();
    }

    #endregion
}

public enum BGMType
{
    Main,                   // Main 화면 BGM
    Schedule,               // Schedule 화면 BGM
    ScheduleProcessing,     // Schedule 진행 BGM
}

[Serializable]
public class BGMSource
{
    public BGMType type;
    public AudioClip clip;
}