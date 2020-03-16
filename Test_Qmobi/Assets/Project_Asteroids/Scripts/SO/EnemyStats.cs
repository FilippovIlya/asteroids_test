using UnityEngine;
/// <summary>
/// Базовый класс с параметрами врагов
/// </summary>
[CreateAssetMenu(fileName = "EnemyStats", menuName = "Data/EnemyStats")]
public class EnemyStats : ScriptableObject
{
    [Tooltip("Количество очков за убийство")] public int points;
}
