using System;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public static event Action OnGameStart = delegate { };
    public static event Action<bool> OnGameFinish = delegate { };

    [SerializeField]
    UnityEvent onLoad = new UnityEvent();

    [SerializeField]
    UnityEvent onStart = new UnityEvent();

    [SerializeField]
    UnityEvent onResult = new UnityEvent();

    internal static bool IsGameOver { get; private set; } = false;
    internal static int GamePlayCount { get; private set; } = 0;
    internal static int Attempts { get; private set; } = 0;

    [SerializeField]
    TextMeshProUGUI levelText;
    [SerializeField]
    Canvas victoryPanel, gameOverPanel;
#if UNITY_EDITOR
    [ContextMenu("Remove Null Events")]
    void RemoveNullEntryFromEvents()
    {
        onLoad.RemoveNullEntry();
        onResult.RemoveNullEntry();
        onStart.RemoveNullEntry();
    }
#endif

    protected void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        Attempts = PlayerPrefs.GetInt("Attempts", 1);
        GamePlayCount = PlayerPrefs.GetInt("GamePlayCount", 1);
        var levelString = Helper.Build("LEVEL ", GamePlayCount.ToString()).ToString();
        onLoad?.Invoke();
        IsGameOver = false;
    }

    public void StartGame()
    {
        OnGameStart();
        onStart?.Invoke();
        LevelStartEvent(GamePlayCount, Attempts);
    }

    public void SetSpeed(int speed)
    {
        Time.timeScale = speed;
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    static void LevelStartEvent(int levelCount, int attempts) { }

    static void LevelCompleteEvent(int levelCount, int attempts) { }

    static void LevelFailedEvent(int levelCount, int attempts) { }
}
