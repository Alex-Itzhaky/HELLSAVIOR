using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _lightEnemyPrefab;
    [SerializeField] private GameObject _rangedEnemyPrefab;
    [SerializeField] private GameObject _mediumEnemyPrefab;
    [SerializeField] private GameObject _heavyEnemyPrefab;
    [SerializeField] private float _areaWidth;
    [SerializeField] private float _areaHeight;
    private GameObject _enemyToSpawn;

    private Vector2 GetRandomPosition()
    {
        float randomOffsetX = Random.Range(-_areaWidth / 2, _areaWidth / 2);
        float randomOffsetY = Random.Range(-_areaHeight / 2, _areaHeight / 2);
        Vector2 targetPosition = new Vector2(
            transform.position.x + randomOffsetX,
            transform.position.y + randomOffsetY
        );
        return targetPosition;
    }

    public void RequestEnemySpawn(EnemyType enemy)
    {
        switch (enemy)
        {
            case EnemyType.Light:
                _enemyToSpawn = _lightEnemyPrefab;
                break;
            case EnemyType.Ranged:
                _enemyToSpawn = _rangedEnemyPrefab;
                break;
            case EnemyType.Medium:
                _enemyToSpawn = _mediumEnemyPrefab;
                break;
            case EnemyType.Heavy:
                _enemyToSpawn = _heavyEnemyPrefab;
                break;
        }

        SpawnEnemy(_enemyToSpawn, GetRandomPosition());
    }

    private void SpawnEnemy(GameObject enemy, Vector2 spawnPoint)
    {
        if (_enemyToSpawn != null)
        {
            GameObject enemyInstance = Instantiate(enemy);
            enemyInstance.transform.position = spawnPoint;
        }
    }

    //Debugging

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y, .1f), new Vector3(_areaWidth, _areaHeight, .1f));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.orange;
        Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y, .1f), new Vector3(_areaWidth, _areaHeight, .1f));
    }

    [ContextMenu("Test EnemySpawner")]
    private void TestEnemySpawn()
    {
        RequestEnemySpawn(EnemyType.Light);
    }


}
