using System.Collections;
using UnityEngine;
using UnityNightPool;

/// <summary>
/// Базовый класс для обоих типов врагов
/// </summary>
public class Enemy : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D rb;

    [HideInInspector] public float lastCollisionCheck;

    [HideInInspector] public GameController gameController;

    [HideInInspector] public float speed;

    [SerializeField] private SpriteRenderer _spriteRenderer;

    private GameObject _explosion;

    private float _checkCollisionDelay = 0.1f;

    protected bool isDead;

    private void Awake()
    {
        if (transform.childCount != 0) _explosion = transform.GetChild(0).gameObject;
    }

    private void OnEnable()
    {
        isDead = false;
        _spriteRenderer.enabled = true;
        if(_explosion!=null) _explosion.SetActive(false);
    }

    #region Collision

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CollisionWithPlayer(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!CheckCollision()) return;
        CollisionWithBorders(collision);
    }

    protected bool CheckCollision()
    {
        if (isDead) return false;
        if ((Time.time - lastCollisionCheck) > _checkCollisionDelay) return true;
        else return false;
    }

    protected virtual void CollisionWithPlayer(Collider2D collision)
    {
        if (isDead) return;
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().GetDamage();
            Return();
        }
    }

    protected virtual void CollisionWithBorders(Collider2D collision)
    {
        if (collision.CompareTag("BorderWidth") || collision.CompareTag("BorderHeight")) Return();
    }

    #endregion

    public void Move()
    {
        if (isDead) return;
        rb.velocity = transform.up * speed;
    }

    public virtual void Return()
    {
        if (isDead) return;
        Explosion();
    }

    /// <summary>
    /// Награда за убийство этого врага
    /// </summary>
    /// <param name="stats"></param>
    public void GetPoints(EnemyStats stats)
    {
        gameController.Points += stats.points;
    }

    public virtual EnemyStats GetStats()
    {
        return null;
    }

    /// <summary>
    /// Взрыв перед уничтожением
    /// </summary>
    private void Explosion()
    {
        isDead = true;
        _spriteRenderer.enabled = false;
        rb.velocity = Vector2.zero;
        _explosion.SetActive(true);
        StartCoroutine(ExplosionDelay(_explosion.GetComponentInChildren<Animator>().runtimeAnimatorController.animationClips[0].length));
    }

    private IEnumerator ExplosionDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        GetComponent<PoolObject>().Return();
    }
}
