using UnityEngine;
/// <summary>
/// Параметры для создания врагов
/// </summary>
[CreateAssetMenu(fileName = "SpawnerStats", menuName = "Data/SpawnerStats")]
public class SpawnerStats : ScriptableObject
{
    [Tooltip("Время между появлением астероидов")] public float spawnTime;
    [Tooltip("Шанс появления летающей тарелки")] public int chanceUfoSpawn;
    [Tooltip("Скорость врагов, каждые 100 очков берем следующий индекс")] public float[] enemySpeed;
    [Tooltip("х-коордианата точки создания")] public float width;
    [Tooltip("y-координата точки создания")] public float height;
}
