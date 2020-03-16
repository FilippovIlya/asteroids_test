using System.Collections;
using UnityEngine;
using UnityNightPool;

/// <summary>
/// Создаёт астероиды и летающие тарелки, данные берем в stats
/// </summary>
public class Spawner : MonoBehaviour
{
    [SerializeField] private int _Asteroid;

    [SerializeField] private int _Ufo;

    [SerializeField] private Transform _targetForUfo;

    [SerializeField] private SpawnerStats _stats;

    [SerializeField] private GameController _gameController;

    private PoolObject _obj;
    
    void Start()
    {
        Spawn();
    }

    /// <summary>
    /// создаём объект на одной из 4-х сторон экрана, где 1-лево, 2-верх, 3-низ, 4-право
    /// затем сетим нужные данные в зависимости от того создали астероид или тарелку
    /// </summary>
    private void Spawn()
    {
        Vector3 startPos;
        Vector3 endPos;
        switch (Random.Range(1, 4))
        {
            case 1:
                 startPos = new Vector3(-_stats.width, Random.Range(-_stats.height, _stats.height), 1);
                 endPos = new Vector3(_stats.width, Random.Range(-_stats.height, _stats.height), 1);
                break;
            case 2:
                startPos = new Vector3(Random.Range(-_stats.width, _stats.width), -_stats.height, 1);
                endPos = new Vector3(Random.Range(-_stats.width, _stats.width), _stats.height, 1);
                break;
            case 3:
                startPos = new Vector3(_stats.width, Random.Range(-_stats.height, _stats.height), 1);
                endPos = new Vector3(-_stats.width, Random.Range(-_stats.height, _stats.height), 1);
                break;
            default:
                startPos = new Vector3(Random.Range(-_stats.width, _stats.width), _stats.height, 1);
                endPos = new Vector3(Random.Range(-_stats.width, _stats.width), -_stats.height, 1);
                break;
        }
        Vector2 direction = endPos - startPos;
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        if (SpawnUfo())
        {
            _obj = PoolManager.Get(_Ufo);
            Ufo component = _obj.GetComponent<Ufo>();
            _obj.transform.position = startPos;
            _obj.transform.rotation = Quaternion.Euler(0, 0, -angle);
            component.target = _targetForUfo;
            component.Shot();
        }
        else _obj = PoolManager.Get(_Asteroid);
        _obj.transform.position = startPos;
        _obj.transform.rotation = Quaternion.Euler(0, 0, -angle);
        Enemy enemy = _obj.GetComponent<Enemy>();
        enemy.gameController = _gameController;
        enemy.speed = GetSpeed();
        enemy.Move();
        StartCoroutine(SpawnDelay());
        
    }

    private IEnumerator SpawnDelay()
    {
        yield return new WaitForSeconds(_stats.spawnTime);
        Spawn();
    }

    /// <summary>
    /// рандомит спавнить нам тарелку или нет
    /// </summary>
    /// <returns></returns>
    private bool SpawnUfo()
    {
        var rr = Random.Range(1, 100);
        if (rr <= _stats.chanceUfoSpawn) return true;
        else return false;
    }

    /// <summary>
    /// каждые 100 очков увеличиваем среднюю скорость врага
    /// </summary>
    /// <returns>Возвращает скорость задаваемую врагу</returns>
    private float GetSpeed()
    {
        int index = _gameController.Points / 100;
        if (index < _stats.enemySpeed.Length)
        {
            float averageSpeed = _stats.enemySpeed[index];
            return Random.Range(averageSpeed - 1, averageSpeed + 1);
        }
        else
        {
            return _stats.enemySpeed[_stats.enemySpeed.Length-1];
        }
    }
}
