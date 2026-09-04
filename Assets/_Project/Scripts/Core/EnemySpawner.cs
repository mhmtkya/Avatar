using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")] 
    public EnemyController enemyPrefab;

    public float spawnRadius;
    public float spawnInterval;

    private IObjectPool<EnemyController> enemyPool;
    private float spawnTimer;
    private Transform playerTransform;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) playerTransform = player.transform;

        enemyPool = new ObjectPool<EnemyController>(
            createFunc: CreateEnemy,
            actionOnGet: OnGetEnemy,
            actionOnDestroy: OnDestroyEnemy,
            actionOnRelease: OnReleaseEnemy,
            defaultCapacity: 100,
            maxSize: 500);
    }

    private void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f && playerTransform != null)
        {
            SpawnEnemy();
            spawnTimer = spawnInterval;
        }
    }

    private void SpawnEnemy()
    {
        EnemyController enemy = enemyPool.Get();
        
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        Vector2 spawnPosition = (Vector2)playerTransform.position + (randomDirection * spawnRadius);
        
        enemy.transform.position = spawnPosition;
    }

    private EnemyController CreateEnemy()
    {
        EnemyController enemyInstance = Instantiate(enemyPrefab);
        enemyInstance.SetPool(enemyPool);
        return enemyInstance;
    }

    private void OnGetEnemy(EnemyController enemy)
    {
        enemy.gameObject.SetActive(true);
    }

    private void OnReleaseEnemy(EnemyController enemy)
    {
        enemy.gameObject.SetActive(false);
    }

    private void OnDestroyEnemy(EnemyController enemy)
    {
        Destroy(enemy.gameObject);
    }
    
    
    
}
