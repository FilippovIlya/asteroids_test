using System.Collections;
using UnityEngine;
using UnityNightPool;

/// <summary>
/// Класс летающей тарелки
/// </summary>
public class Ufo : Enemy
{
    [SerializeField] private UfoStats _stats;

    [HideInInspector] public Transform target;

    private bool _canRotate = false;

    public void Shot()
    {
        if (isDead) return;
         Vector3 targetPos = target.position;
         Vector2 direction = targetPos - transform.position;
         float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
         PoolObject bullet = PoolManager.Get(6);
         bullet.transform.position = transform.position;
         bullet.transform.rotation = Quaternion.Euler(0, 0, -angle);
         Bullet comp = bullet.GetComponent<Bullet>();
         comp.Init(_stats.shotSpeed, _stats.bulletDestroyDelay);
         StartCoroutine(ShotDelay());
    }

    private IEnumerator ShotDelay()
    {
        yield return new WaitForSeconds(_stats.shotDelay);
        Shot();
    }

    private void Update()
    {
        if (_canRotate)
        {
            Rotate();
            Move();
        }
    }

    public override EnemyStats GetStats()
    {
        return _stats;
    }

    protected override void CollisionWithPlayer(Collider2D collision)
    {
        if (collision.tag == "TriggerRange")
        {
            _canRotate = true;
        }
        base.CollisionWithPlayer(collision);
    }

    /// <summary>
    /// поворачивает в сторону игрока, когда вошли в радиус триггера
    /// </summary>
    private void Rotate()
    {
        Vector2 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, -angle);
    }
}

