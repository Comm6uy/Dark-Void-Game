using UnityEngine;

[CreateAssetMenu(fileName = "EnergyBooster", menuName = "Boosters/Energy")]
public class EnergyBooster : BaseBooster
{
    public float duration = 15f;

    public override void Activate()
    {
        // Ищем фонарик игрока (в идеале кэшировать ссылку через GameManager)
        BaseFlashlight playerFlashlight = FindAnyObjectByType<BaseFlashlight>();
        if (playerFlashlight != null)
        {
            // Запускаем корутину через GameManager, так как ScriptableObject не может крутить корутины
            GameManager.Instance.StartCoroutine(EnergyRoutine(playerFlashlight));
        }
    }

    private System.Collections.IEnumerator EnergyRoutine(BaseFlashlight flashlight)
    {
        flashlight.IsEnergyActive = true;
        yield return new WaitForSeconds(duration);
        flashlight.IsEnergyActive = false;
    }
}