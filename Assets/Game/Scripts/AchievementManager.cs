using UnityEngine;
using System.Collections.Generic;
using YG;

[System.Serializable]
public class Achievement
{
    public string id;
    public string title;
    public string description;
    public int targetAmount;
    public int currentAmount;
    public bool isCompleted;
    public int rewardCoins;
    public AchievementType type;
}

public enum AchievementType { FirstKill, KillEnemies20, FirstLevel, CompleteLevels10, FirstDeath }

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance;
    [SerializeField] private List<Achievement> achievements = new List<Achievement>();

    void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); return; }
    }

    void OnEnable()
    {
        YG2.onGetSDKData += LoadAchievementsFromCloud;
        GameEvents.OnEnemyKilled += HandleEnemyKilled;
        GameEvents.OnLevelCompleted += HandleLevelCompleted;
        GameEvents.OnPlayerDied += HandlePlayerDied;
    }

    void OnDisable()
    {
        YG2.onGetSDKData -= LoadAchievementsFromCloud;
        GameEvents.OnEnemyKilled -= HandleEnemyKilled;
        GameEvents.OnLevelCompleted -= HandleLevelCompleted;
        GameEvents.OnPlayerDied -= HandlePlayerDied;
    }

    private void LoadAchievementsFromCloud()
    {
        // Защита от null
        if (YG2.saves.achievementIds == null) YG2.saves.achievementIds = new List<string>();
        if (YG2.saves.achievementProgress == null) YG2.saves.achievementProgress = new List<int>();

        // Считываем прогресс
        for (int i = 0; i < YG2.saves.achievementIds.Count; i++)
        {
            string savedId = YG2.saves.achievementIds[i];
            int savedProgress = YG2.saves.achievementProgress[i];
            
            foreach (var ach in achievements)
            {
                if (ach.id == savedId)
                {
                    ach.currentAmount = savedProgress;
                    ach.isCompleted = ach.currentAmount >= ach.targetAmount;
                    break;
                }
            }
        }
        
        // Обновляем визуал, если панель открыта
        FindAnyObjectByType<ProgressPanelUI>()?.RefreshUI();
    }

    private void HandleEnemyKilled() { UpdateProgress(AchievementType.FirstKill, 1); UpdateProgress(AchievementType.KillEnemies20, 1); }
    private void HandleLevelCompleted(int levelId) { UpdateProgress(AchievementType.FirstLevel, 1); UpdateProgress(AchievementType.CompleteLevels10, 1); }
    private void HandlePlayerDied() { UpdateProgress(AchievementType.FirstDeath, 1); }

    private void UpdateProgress(AchievementType type, int amount)
    {
        bool isProgressChanged = false;

        foreach (var ach in achievements)
        {
            if (ach.type == type && !ach.isCompleted)
            {
                ach.currentAmount += amount;
                if (ach.currentAmount >= ach.targetAmount)
                {
                    ach.currentAmount = ach.targetAmount;
                    ach.isCompleted = true;
                    Debug.Log($"[AchievementManager] 🏆 Достижение открыто: {ach.title}");
                    
                    // Вот здесь мы используем Лидерборд - как приятный бонус в интерфейсе Яндекса
                    if (YandexInitializer.IsSDKReady) 
                        YG2.SetLeaderboard(ach.id, ach.currentAmount);
                }
                isProgressChanged = true;
            }
        }

        // Сохраняем в облако только если прогресс реально изменился
        if (isProgressChanged)
        {
            SaveAchievementsToCloud();
        }
    }

    private void SaveAchievementsToCloud()
    {
        YG2.saves.achievementIds.Clear();
        YG2.saves.achievementProgress.Clear();
        
        foreach (var ach in achievements)
        {
            YG2.saves.achievementIds.Add(ach.id);
            YG2.saves.achievementProgress.Add(ach.currentAmount);
        }

        YG2.SaveProgress();
    }

    public List<Achievement> GetAchievements() => achievements;
}