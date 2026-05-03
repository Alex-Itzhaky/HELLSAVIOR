using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public EnemyType enemyType;
    public int maxHp;
    public int damage;
    public float attackDuration;
    public float attackCooldown;
    public float attackRange;
    public float baseSpeed;
    public float rotateSpeed = 100f;
    public Sprite enemySprite;
}
