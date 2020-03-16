using System.Collections;
using UnityEngine;
using UnityNightPool;

/// <summary>
/// Считывание действий игрока, обработка коллизии корабля со границами, данные берем в stats
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerStats _stats;

    [SerializeField] private Rigidbody2D _rb;

    [SerializeField] private Animator _animator;

    [SerializeField] private Collider2D _coll;

    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private GameController _gameController;

    private bool _isDead;

    private int _currentHealth;

    private float _lastShotTime;

    private float _lastJumpTime;

    private float _lastRespawnTime;

    private float _lastCollisionCheck;

    private float _checkCollisionDelay = 0.1f;

    void Start()
    {
        _lastShotTime = -_stats.shotDelay;

        _currentHealth = _stats.maxHealth;

        _lastJumpTime = -_stats.jumpDelay;
    }

    void Update()
    {
        if (_isDead) return;
        ReadDirection();
        if (!Input.anyKey) return;
        ReadInput();
    }

    #region Input

    /// <summary>
    /// Считывание нажатых игроком клавиш и кнопок мыши
    /// </summary>
    private void ReadInput()
    {
        if (Input.GetMouseButton(1)) Shot();
        if (Input.GetKeyDown(KeyCode.W)) Move();
        if (Input.GetKeyDown(KeyCode.Space)) HyperSpaceJump();
    }

    /// <summary>
    /// Направляет корабль в сторону курсора мыши
    /// </summary>
    private void ReadDirection()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        float angle = Vector2.Angle(Vector2.right, mousePosition - transform.position);
        transform.eulerAngles = new Vector3(0f, 0f, transform.position.y < mousePosition.y ? angle : -angle);
    }

    #endregion

    #region Shooting

    private void Shot()
    {
        if (!IsShotReady()) return;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePos - transform.position;
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        PoolObject bullet = PoolManager.Get(1);
        bullet.transform.position = transform.position;
        bullet.transform.rotation = Quaternion.Euler(0, 0, -angle);
        Bullet comp = bullet.GetComponent<Bullet>();
        comp.Init(_stats.shotSpeed, _stats.bulletDestroyDelay);
        _lastShotTime = Time.time;
    }

    private bool IsShotReady()
    {
        if ((Time.time - _lastShotTime) > _stats.shotDelay) return true;
        else return false;
    }

    #endregion

    #region Moving

    private void Move()
    {
        if (Mathf.Abs(_rb.velocity.x) + Mathf.Abs(_rb.velocity.y) < 3f) _rb.AddForce(transform.right * _stats.maxSpeed);
    }

    private void HyperSpaceJump()
    {
        if (!IsJumpReady()) return;
        _rb.position = new Vector2(Random.Range(-10f, 10f), Random.Range(-5f, 5f));
        _lastJumpTime = Time.time;
    }

    private bool IsJumpReady()
    {
        if ((Time.time - _lastJumpTime) > _stats.jumpDelay) return true;
        else return false;
    }
    /// <summary>
    /// Отправляет объект в противоположную сторону экрана
    /// </summary>
    /// <param name="number">Значение 1 зеркалит по оси х, значение 2 зеркалит по оси y</param>
    public void ChangePosition(int number)
    {
        _lastCollisionCheck = Time.time;
        switch (number)
        {
            case 1:
                if (_rb.position.x < 0)
                    _rb.position = new Vector2(_rb.position.x * -1 - 1, _rb.position.y);
                else _rb.position = new Vector2(_rb.position.x * -1 + 1, _rb.position.y);
                break;
            case 2:
                if (_rb.position.y < 0)
                    _rb.position = new Vector2(_rb.position.x, _rb.position.y * -1 - 1);
                else _rb.position = new Vector2(_rb.position.x, _rb.position.y * -1 + 1);
                break;
        }
    }

    #endregion

    #region Collision

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!CheckCollision()) return;
        CollisionWithBorders(collision);
    }

    /// <summary>
    /// Проверка выхода за пределы экрана
    /// </summary>
    public void CollisionWithBorders(Collider2D collision)
    {
        if (collision.CompareTag("BorderWidth")) ChangePosition(1);
        if (collision.CompareTag("BorderHeight")) ChangePosition(2);
    }

    /// <summary>
    /// Метод получения урона, вызывается из пули врага
    /// </summary>
    public void GetDamage()
    {
        if (IsImmune()) return;
        _animator.SetBool("expl", true);
        _audioSource.Play();
        _coll.enabled = false;
        _isDead = true;
        _currentHealth--;
        _gameController.ChangeHealth(_currentHealth);
        if (_currentHealth > 0) StartCoroutine(Respawn());
        else
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        _gameController.GameOver();
    }

    private bool CheckCollision()
    {
        if ((Time.time - _lastCollisionCheck) > _checkCollisionDelay) return true;
        else return false;
    }

    /// <summary>
    /// Возрождение после потери жизни
    /// </summary>
    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(_stats.respawnTime);
        transform.position = Vector3.zero;
        _isDead = false;
        _coll.enabled = true;
        _animator.SetBool("expl", false);
        _lastRespawnTime = Time.time;
    }

    /// <summary>
    /// Иммунитет после возрождения
    /// </summary>
    private bool IsImmune()
    {
        if ((Time.time - _lastRespawnTime) > _stats.immuneDelay) return false;
        else return true;
    }

    #endregion
}
