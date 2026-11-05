using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class LootChest : MonoBehaviour
{
    [Header("Настройки лута")]
    [SerializeField] private BaseBooster energyBooster;
    [SerializeField] private BaseBooster timeBooster;
    [SerializeField] private int minCoins = 10;
    [SerializeField] private int maxCoins = 50;

    private bool isOpened = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Игрок подошел к сундуку
        if (!isOpened && other.CompareTag("Player"))
        {
            OpenChest();
        }
    }

    private void OpenChest()
    {
        isOpened = true;

        // Генерируем случайное число от 0 до 99
        int chance = Random.Range(0, 100);

        if (chance < 40) // Шанс 40% на деньги
        {
            int coinsAmount = Random.Range(minCoins, maxCoins + 1);
            GameEvents.CoinsCollected(coinsAmount); // Используем ваш GameEvents[cite: 25]
            Debug.Log($"Из сундука выпало {coinsAmount} монет!");
        }
        else if (chance < 70) // Шанс 30% на бустер энергии
        {
            if (energyBooster != null) energyBooster.Activate();
            Debug.Log("Из сундука выпала Безлимитная энергия!");
        }
        else // Оставшиеся 30% на бустер времени
        {
            if (timeBooster != null) timeBooster.Activate();
            Debug.Log("Из сундука выпало Увеличение времени!");
        }

        // Анимация открытия, звук, партиклы...
        GetComponent<SpriteRenderer>().color = Color.gray; // Временно, красим в серый
        
        // Отключаем коллайдер, чтобы больше не взаимодействовать
        GetComponent<Collider2D>().enabled = false;
    }
}