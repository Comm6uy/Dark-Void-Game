using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Экран смерти")]
    [SerializeField] private GameObject deathScreen;

    [Header("Настройки раунда")]
    [SerializeField] private float levelTime = 300f; // 5 минут на уровень
    public float CurrentMoneyMultiplier { get; set; } = 1f;

    private float currentTime;
    private bool isGameOver = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (deathScreen != null) deathScreen.SetActive(false);
        currentTime = levelTime;
        StartCoroutine(AutoSaveRoutine());
    }

    void Update()
    {
        if (isGameOver) return;

        // Таймер раунда
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                currentTime = 0;
                GameOver(); // Время вышло - игрок проиграл
            }
        }
    }

    // Для бустера времени
    public void AddLevelTime(float amount)
    {
        currentTime += amount;
        Debug.Log($"[GameManager] Добавлено время! Осталось: {currentTime} сек.");
    }

    // Централизованный метод для получения монет (чтобы всегда работал бустер)
    public void AddCoins(int baseAmount)
    {
        int finalAmount = Mathf.RoundToInt(baseAmount * CurrentMoneyMultiplier);
        GameEvents.CoinsCollected(finalAmount);
        Debug.Log($"[GameManager] Получено монет: {finalAmount} (Множитель: x{CurrentMoneyMultiplier})");
    }

    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;
    
        YG2.SaveProgress();
        GameEvents.PlayerDied(); 
        Time.timeScale = 0f;
    
        if (deathScreen != null) deathScreen.SetActive(true);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        YG2.SaveProgress();
        if (YG2.isSDKEnabled) YG2.InterstitialAdvShow();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        YG2.SaveProgress();
        if (YG2.isSDKEnabled) YG2.InterstitialAdvShow();
        SceneManager.LoadScene("Menu");
    }

    private System.Collections.IEnumerator AutoSaveRoutine()
    {
        var waitForSeconds = new WaitForSeconds(60f);
        while (true)
        {
            yield return waitForSeconds;
            if (Time.timeScale > 0f) YG2.SaveProgress();
        }
    }
}