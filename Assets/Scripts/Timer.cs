using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance;

    private Text timerText;
    private float time;
    private bool isRunning = false;

    public string startSceneName = "StartScene";
    public string[] deathSceneNames;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Update()
    {
        if (!isRunning || timerText == null) return;

        time += Time.deltaTime;
        UpdateTimerText();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 🔹 새 씬의 Text 자동 탐색
        FindTimerTextInScene();

        // 시작 씬
        if (scene.name == startSceneName)
        {
            ResetTimer();
            StartTimer();
            return;
        }

        // 데스 씬
        if (IsDeathScene(scene.name))
        {
            StopTimer();
        }
    }

    void FindTimerTextInScene()
    {
        GameObject obj = GameObject.FindWithTag("TimerText");

        if (obj != null)
        {
            timerText = obj.GetComponent<Text>();
            UpdateTimerText();
        }
        else
        {
            timerText = null; // 이 씬에는 타이머 UI 없음
        }
    }

    bool IsDeathScene(string sceneName)
    {
        foreach (string deathScene in deathSceneNames)
        {
            if (sceneName == deathScene)
                return true;
        }
        return false;
    }

    void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    void StartTimer()
    {
        isRunning = true;
    }

    void StopTimer()
    {
        isRunning = false;
    }

    void ResetTimer()
    {
        time = 0f;
        UpdateTimerText();
    }
}
//made by ChatGPT