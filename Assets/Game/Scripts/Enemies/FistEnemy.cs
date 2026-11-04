using UnityEngine;

public class FistEnemy : BaseEnemy
{
    [Header("Параметры Кулака")]
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float normalSpeed = 2f;
    [SerializeField] private float enragedSpeed = 4.2f;

    [Header("Спрайты фаз")]
    [SerializeField] private Sprite calmSprite; // Сюда положим fistCalm
    [SerializeField] private Sprite rageSprite; // Сюда положим fistRage

    private SpriteRenderer spriteRenderer;
    private int currentHealth;
    private bool isCoveringEye = false;
    private bool wasInLight = false;

    protected override void Awake()
    {
        base.Awake();
        // Получаем ссылку на компонент, который рисует спрайт
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void Start()
    {
        base.Start();
        currentHealth = maxHealth;
        if (agent != null) agent.speed = normalSpeed;
        
        // Устанавливаем спокойный спрайт при появлении врага
        if (spriteRenderer != null && calmSprite != null)
        {
            spriteRenderer.sprite = calmSprite;
        }
    }

    protected override void HandleLightReaction(bool isInLight)
    {
        if (isInLight && !wasInLight)
        {
            wasInLight = true;
            isCoveringEye = true;
            if (agent != null) agent.isStopped = true;

            TakeDamage(1);
            GameEvents.EnemyRepelled();
        }
        else if (!isInLight && wasInLight)
        {
            wasInLight = false;
            isCoveringEye = false;

            if (agent != null)
            {
                agent.isStopped = false;
                agent.speed = enragedSpeed; 
            }

            // МЕНЯЕМ СПРАЙТ НА ЯРОСТНЫЙ
            if (spriteRenderer != null && rageSprite != null)
            {
                spriteRenderer.sprite = rageSprite;
            }
        }
    }

    public override void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected override bool CanMove() => !isCoveringEye && !isDead;

    protected override void UpdateMovement()
    {
        if (agent != null && agent.isOnNavMesh)
        {
            agent.SetDestination(player.position);
        }
    }
}