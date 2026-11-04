using UnityEngine;

public class HandEnemy : BaseEnemy
{
    [Header("Параметры Руки")]
    [SerializeField] private float speed = 2.5f;

    protected override void Awake()
    {
        base.Awake();
        if (agent != null) agent.enabled = false; // Игнорирует стены
    }

    protected override void HandleLightReaction(bool isInLight)
    {
        if (isInLight)
        {
            GameEvents.EnemyRepelled();
            Die();
        }
    }

    protected override bool CanMove() => !isDead;

    protected override void UpdateMovement()
    {
        // Не используем, так как движение переопределено в Update для плавности
    }

    protected override void Update()
    {
        base.Update();
        
        if (CanMove() && lightDetector != null && !lightDetector.IsInLight())
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                player.position,
                speed * Time.deltaTime
            );
        }
    }
}