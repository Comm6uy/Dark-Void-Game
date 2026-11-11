using UnityEngine;

[CreateAssetMenu(fileName = "MoneyBooster", menuName = "Boosters/Money Multiplier")]
public class MoneyBooster : BaseBooster
{
    public float multiplier = 1.5f; // +50% к доходу

    public override void Activate()
    {
        // Устанавливаем множитель в GameManager или CurrencyManager
        GameManager.Instance.CurrentMoneyMultiplier = multiplier;
    }
}