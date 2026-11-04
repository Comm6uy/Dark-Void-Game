using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Collider2D))]
public abstract class BaseEnemy : MonoBehaviour
{
    [Header("Базовые параметры")]
    [SerializeField] protected string playerTag = "Player";
    [SerializeField] protected LightDetector lightDetector;
    [SerializeField] protected float pathUpdateInterval = 0.15f;

    protected Transform player;
    protected NavMeshAgent agent;
    protected bool isDead = false;
    private float pathTimer;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.updateRotation = false;
            agent.updateUpAxis = false;
        }

        if (lightDetector == null)
            lightDetector = GetComponent<LightDetector>();
    }

    protected virtual void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag(playerTag);
        if (p != null) player = p.transform;
    }

    protected virtual void Update()
    {
        if (isDead || player == null) return;

        bool inLight = lightDetector != null && lightDetector.IsInLight();
        HandleLightReaction(inLight);

        if (!isDead && CanMove())
        {
            pathTimer -= Time.deltaTime;
            if (pathTimer <= 0f)
            {
                UpdateMovement();
                pathTimer = pathUpdateInterval;
            }
        }
    }

    // Эти методы будут переопределяться в каждом конкретном враге
    protected abstract void HandleLightReaction(bool isInLight);
    protected abstract bool CanMove();
    protected abstract void UpdateMovement();

    public virtual void TakeDamage(int damage) { }

    protected virtual void Die()
    {
        if (isDead) return;
        isDead = true;
        GameEvents.EnemyKilled();
        Destroy(gameObject); // Враг уничтожается. Спавн новых врагов лучше делать через отдельный Spawner
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;
        if (other.CompareTag(playerTag))
        {
            GameManager.Instance?.GameOver();
        }
    }
}