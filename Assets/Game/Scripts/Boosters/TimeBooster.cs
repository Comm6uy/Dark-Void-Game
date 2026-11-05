using UnityEngine;

[CreateAssetMenu(fileName = "TimeBooster", menuName = "Boosters/Time Extension")]
public class TimeBooster : BaseBooster
{
    public float addedTime = 30f; // Добавляем 30 секунд

    public override void Activate()
    {
        // Предполагается, что в GameManager или LevelManager есть метод AddTime
        GameManager.Instance.AddLevelTime(addedTime);
    }
}