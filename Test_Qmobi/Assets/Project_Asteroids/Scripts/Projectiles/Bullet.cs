using System.Collections;
using UnityEngine;
using UnityNightPool;

/// <summary>
/// Пуля. Принимает данные от стреляющего объекта
/// </summary>
public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;

    [SerializeField] private bool _isPlayerBullet;

    [HideInInspector] public float destroyDelay;

    /// <summary>
    /// Инициализация пули
    /// </summary>
    /// <param name="shotSpeed">Скорость полёта</param>
    /// <param name="destroyDelay">Задержка перед уничтожением</param>
    public void Init(float shotSpeed, float destroyDelay)
    {
        _rb.velocity = transform.up * shotSpeed;
        StartCoroutine(Destroying(destroyDelay));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isPlayerBullet)
        {
            if (collision.CompareTag("Enemy"))
            {
                Enemy enemy = collision.GetComponent<Enemy>();
                enemy.Return();
                enemy.GetPoints(enemy.GetStats());
                GetComponent<PoolObject>().Return();
            }
        }
        else
        {
            if (collision.CompareTag("Player"))
            {
                collision.GetComponent<PlayerController>().GetDamage();
                GetComponent<PoolObject>().Return();
            }
        }
    }

    private IEnumerator Destroying(float destroyDelay)
    {
        yield return new WaitForSeconds(destroyDelay);
        GetComponent<PoolObject>().Return();
    }
}
