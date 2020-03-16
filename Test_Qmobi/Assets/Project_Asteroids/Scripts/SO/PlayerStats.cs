using UnityEngine;
/// <summary>
/// Параметры игрока и его пули
/// </summary>
[CreateAssetMenu(fileName = "PlayerStats", menuName = "Data/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    [Tooltip("Максимальное здоровье игрока")] public int maxHealth;
    [Tooltip("Время воскрешения игрока")] public float respawnTime;
    [Tooltip("Скорость полёта выстрела")] public float shotSpeed;
    [Tooltip("Откат выстрела")] public float shotDelay;
    [Tooltip("Максимальная скорость игрока")] public float maxSpeed;
    [Tooltip("Время жизни пули")] public float bulletDestroyDelay;
    [Tooltip("Откат прыжка")] public float jumpDelay;
    [Tooltip("Время неуязвимости после смерти")] public float immuneDelay;
}
