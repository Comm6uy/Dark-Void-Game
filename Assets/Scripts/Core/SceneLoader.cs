using UnityEngine;
using UnityEngine.SceneManagement;

namespace DarkVoid.Core
{
    public static class SceneLoader
    {
        public const string Bootstrap = "Bootstrap";
        public const string MainMenu = "MainMenu";
        public const string GameplayCore = "GamePlay_Core";
        public const string Level01 = "Level_01_HunterHouse";

        public static void LoadMainMenu()
        {
            SceneManager.LoadScene(MainMenu, LoadSceneMode.Single);
        }

        public static void LoadGameplay(string levelName = Level01)
        {
            // √рузим €дро геймпле€
            SceneManager.LoadScene(GameplayCore, LoadSceneMode.Single);

            // јддитивно Ч уровень
            if (Application.CanStreamedLevelBeLoaded(levelName))
                SceneManager.LoadScene(levelName, LoadSceneMode.Additive);
            else
                Debug.LogError($"[SceneLoader] —цена '{levelName}' не добавлена в Build Settings");
        }

        public static void ReloadCurrentLevel(string levelName)
        {
            SceneManager.UnloadSceneAsync(levelName);
            SceneManager.LoadScene(levelName, LoadSceneMode.Additive);
        }
    }
}