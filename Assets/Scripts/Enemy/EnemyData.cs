using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string name;
    public string maxHp;
    public int damage;
    public float attackDuration;
    public float attackCooldown;
    public float attackRange;
    public float baseSpeed;
    public Sprite enemySprite;
}
