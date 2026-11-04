using UnityEngine;

public class ShadowEnemy : BaseEnemy
{
    [Header("Параметры Тени")]
    [SerializeField] private float speed = 1.8f;
    [SerializeField] private float lifetime = 15f;

    private float currentLifetime;
    private bool isFrozen = false;

    protected override void Start()
    {
        base.Start();
        currentLifetime = lifetime;
        if (agent != null) agent.speed = speed;
    }

    protected override void Update()
    {
        base.Update();

        currentLifetime -= Time.deltaTime;
        if (currentLifetime <= 0f)
        {
            Destroy(gameObject);
        }
    }

    protected override void HandleLightReaction(bool isInLight)
    {
        if (isInLight)
        {
            if (!isFrozen)
            {
                isFrozen = true;
                if (agent != null) agent.isStopped = true;
                GameEvents.EnemyRepelled();
            }
        }
        else
        {
            if (isFrozen)
            {
                isFrozen = false;
                if (agent != null) agent.isStopped = false;
            }
        }
    }

    protected override bool CanMove() => !isFrozen && !isDead;

    protected override void UpdateMovement()
    {
        if (agent != null && agent.isOnNavMesh)
        {
            agent.SetDestination(player.position);
        }
    }
}