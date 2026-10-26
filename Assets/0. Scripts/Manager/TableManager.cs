using TableSO.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class TableManager : Singleton<TableManager>
{
    [SerializeField] private TableCenter tableCenter;
    [SerializeField] private StatusSO status;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private Button skipBtn;

    private bool isDataReady = false;
    private bool isVideoEnd = false;

    protected override void Awake()
    {
        DontDestroyOnLoad(this);
        base.Awake();

        // 데이터 로드와 영상 준비를 동시에 시작
        tableCenter.OnInitialized += InitializeCallback;
        tableCenter.Initialize();

        videoPlayer.prepareCompleted += OnVideoPrepared;
        videoPlayer.loopPointReached += OnIntroEnd;
        videoPlayer.Prepare();

        skipBtn.onClick.AddListener(() => OnIntroEnd(videoPlayer));
    }

    private void OnVideoPrepared(VideoPlayer vp)
    {
        vp.Play();
    }

    private void OnIntroEnd(VideoPlayer vp)
    {
        isVideoEnd = true;
        TryLoadNextScene();
    }

    private void InitializeCallback()
    {
        LocalizationManager.Instance.Initialize();
        status.InitStatus();

        isDataReady = true;
        TryLoadNextScene();
    }

    private void TryLoadNextScene()
    {
        if (isVideoEnd && isDataReady)
            SceneManager.LoadScene("SelectionScene");
    }

    public T GetTable<T>() where T : ScriptableObject
    {
        return tableCenter.GetTable<T>();
    }
}
