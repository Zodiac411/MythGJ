using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    #region Fields and Properties
    private Camera cam;
    private float camHeight;
    private float camWidth;
    [SerializeField] private List<EnemyWaveProfile> waveProfiles = new List<EnemyWaveProfile>();
    private EnemyWaveStrategyResolver waveStrategyResolver;

    public GameObject verticalEnemyPrefab;
    public GameObject horizontalEnemyPrefab;
    public GameObject zigzagEnemyPrefab;

    private int waveCount = 0;
    [SerializeField]
    private float timeBetweenSpawns = 0.5f;
    [SerializeField]// Time between each enemy spawn
    private int enemiesPerWave = 0;
    #endregion

    #region Enums
    public enum SpawnSide { Top, Left, Right }
    #endregion

    #region Unity Methods
    void Start()
    {
        cam = Camera.main;
        camHeight = 2f * cam.orthographicSize;
        camWidth = camHeight * cam.aspect;
        EnsureDefaultWaveProfiles();
        waveStrategyResolver = new EnemyWaveStrategyResolver(waveProfiles);

        StartCoroutine(SpawnWaves());
    }

    void Update()
    {
        DrawSpawnLines();
    }
    #endregion

    private void EnsureDefaultWaveProfiles()
    {
        if (waveProfiles != null && waveProfiles.Count > 0)
        {
            return;
        }

        waveProfiles = new List<EnemyWaveProfile>
        {
            new EnemyWaveProfile { maxWaveInclusive = 2, pattern = EnemyWavePattern.Early },
            new EnemyWaveProfile { maxWaveInclusive = 7, pattern = EnemyWavePattern.Mid },
            new EnemyWaveProfile { maxWaveInclusive = int.MaxValue, pattern = EnemyWavePattern.Late }
        };
    }

    #region Spawn Logic
    IEnumerator SpawnWaves()
    {
        while (true) // Infinite loop to keep spawning waves
        {
            // Update logic to alternate enemy spawn types based on the wave count
            for (int i = 0; i < enemiesPerWave; i++)
            {
                SpawnCurrentWaveEnemy();
                yield return new WaitForSeconds(timeBetweenSpawns);
            }
            waveCount++;
           // Consider if you still need waveNumber or can merge with waveCount
            yield return new WaitForSeconds(5f); // Wait time before next wave
        }
    }
    void SpawnCurrentWaveEnemy()
    {
        IEnemyWaveStrategy waveStrategy = waveStrategyResolver.Resolve(waveCount);
        EnemySpawnRequest spawnRequest = waveStrategy.CreateSpawnRequest(
            verticalEnemyPrefab,
            horizontalEnemyPrefab,
            zigzagEnemyPrefab);

        SpawnEnemy(spawnRequest.side, spawnRequest.enemyPrefab);
    }
    void SpawnEnemy(SpawnSide side, GameObject enemyPrefab)
    {
        Vector2 spawnPosition = GetSpawnPosition(side);
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

    Vector2 GetSpawnPosition(SpawnSide side)
    {
        float x, y;

        switch (side)
        {
            case SpawnSide.Top:
                x = Random.Range(-camWidth / 4, camWidth / 4);
                y = cam.orthographicSize;
                break;
            case SpawnSide.Left:
                x = -camWidth / 2;
                y = Random.Range(-camHeight / 2, camHeight / 2);
                break;
            case SpawnSide.Right:
                x = camWidth / 2;
                y = Random.Range(-camHeight / 2, camHeight / 2);
                break;
            default:
                x = 0;
                y = 0;
                break;
        }

        return cam.transform.position + new Vector3(x, y, 0);
    }
    #endregion

    #region Debug Methods
    void DrawSpawnLines()
    {
        Vector3 camPosition = cam.transform.position;
        Debug.DrawLine(new Vector3(camPosition.x - camWidth / 2, camPosition.y + camHeight / 2, 0), new Vector3(camPosition.x + camWidth / 2, camPosition.y + camHeight / 2, 0), Color.red);
        Debug.DrawLine(new Vector3(camPosition.x - camWidth / 2, camPosition.y - camHeight / 2, 0), new Vector3(camPosition.x - camWidth / 2, camPosition.y + camHeight / 2, 0), Color.green);
        Debug.DrawLine(new Vector3(camPosition.x + camWidth / 2, camPosition.y - camHeight / 2, 0), new Vector3(camPosition.x + camWidth / 2, camPosition.y + camHeight / 2, 0), Color.blue);
    }
    #endregion
}
