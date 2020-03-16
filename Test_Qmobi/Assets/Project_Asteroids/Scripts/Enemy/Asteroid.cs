using UnityEngine;
using UnityNightPool;

/// <summary>
/// Общий класс для всех видов астероидов
/// </summary>
public class Asteroid : Enemy
{
    [SerializeField] private int[] _lessAsteroids;

    [SerializeField] private EnemyStats _stats;

    /// <summary>
    /// Создаёт астероиды меньшего размера при разрушении и отправляет обратно в пул большой астероид, в случае самого маленького триггерит взрыв
    /// </summary>
    public override void Return()
    {
        for(int i=0;i<_lessAsteroids.Length;i++)
        {
            PoolObject lessAsteroid = PoolManager.Get(_lessAsteroids[i]);
            lessAsteroid.transform.position = transform.position;
            lessAsteroid.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
            Asteroid asteroid = lessAsteroid.GetComponent<Asteroid>();
            asteroid.speed = Random.Range(speed-0.5f, speed +1f);
            asteroid.Move();
            asteroid.gameController = gameController;
        }
        if (_lessAsteroids.Length == 0)
        {
            base.Return();
        }
        else GetComponent<PoolObject>().Return();
    }

    /// <summary>
    /// Проверка выхода за пределы экрана
    /// </summary>
    protected override void CollisionWithBorders(Collider2D collision)
    {
        if (collision.CompareTag("BorderWidth")) ChangePosition(1);
        if (collision.CompareTag("BorderHeight")) ChangePosition(2);
    }

    /// <summary>
    /// Отправляет объект в противоположную сторону экрана
    /// </summary>
    /// <param name="number">Значение 1 зеркалит по оси х, значение 2 зеркалит по оси y</param>
    public void ChangePosition(int number)
    {
        lastCollisionCheck = Time.time;
        switch (number)
        {
            case 1:
                if(rb.position.x<0)
                rb.position = new Vector2(rb.position.x*-1 - 1, rb.position.y);
                else rb.position = new Vector2(rb.position.x*-1 + 1, rb.position.y);
                break;
            case 2:
                if(rb.position.y<0)
                rb.position = new Vector2(rb.position.x, rb.position.y*-1 - 1);
                else rb.position = new Vector2(rb.position.x, rb.position.y*-1 + 1);
                break;
        }
    }

    public override EnemyStats GetStats()
    {
        return _stats;
    }
}
