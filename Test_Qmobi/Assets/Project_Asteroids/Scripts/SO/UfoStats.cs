using UnityEngine;
/// <summary>
/// Параметры летающей тарелки и ее пули
/// </summary>
[CreateAssetMenu(fileName = "UfoStats", menuName = "Data/UfoStats")]
public class UfoStats : EnemyStats
{
    [Tooltip("Скорость полёта выстрела")] public float shotSpeed;
    [Tooltip("Откат выстрела")] public float shotDelay;
    [Tooltip("Время жизни пули")] public float bulletDestroyDelay;
}
