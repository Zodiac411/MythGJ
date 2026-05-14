using System;
using System.Collections.Generic;
using UnityEngine;

public struct EnemySpawnRequest
{
    public EnemySpawner.SpawnSide side;
    public GameObject enemyPrefab;
}

[Serializable]
public struct EnemyWaveProfile
{
    public int maxWaveInclusive;
    public EnemyWavePattern pattern;
}

public enum EnemyWavePattern
{
    Early,
    Mid,
    Late
}

public interface IEnemyWaveStrategy
{
    EnemySpawnRequest CreateSpawnRequest(GameObject verticalEnemyPrefab, GameObject horizontalEnemyPrefab, GameObject zigzagEnemyPrefab);
}

public sealed class EnemyWaveStrategyResolver
{
    private readonly IReadOnlyList<EnemyWaveProfile> waveProfiles;

    public EnemyWaveStrategyResolver(IReadOnlyList<EnemyWaveProfile> waveProfiles)
    {
        this.waveProfiles = waveProfiles;
    }

    public IEnemyWaveStrategy Resolve(int waveCount)
    {
        EnemyWavePattern pattern = EnemyWavePattern.Late;

        if (waveProfiles != null)
        {
            foreach (EnemyWaveProfile profile in waveProfiles)
            {
                if (waveCount <= profile.maxWaveInclusive)
                {
                    pattern = profile.pattern;
                    break;
                }
            }
        }

        switch (pattern)
        {
            case EnemyWavePattern.Early:
                return new EarlyWaveStrategy();
            case EnemyWavePattern.Mid:
                return new MidWaveStrategy();
            default:
                return new LateWaveStrategy();
        }
    }
}

public sealed class EarlyWaveStrategy : IEnemyWaveStrategy
{
    public EnemySpawnRequest CreateSpawnRequest(GameObject verticalEnemyPrefab, GameObject horizontalEnemyPrefab, GameObject zigzagEnemyPrefab)
    {
        return new EnemySpawnRequest
        {
            side = EnemySpawner.SpawnSide.Top,
            enemyPrefab = verticalEnemyPrefab
        };
    }
}

public sealed class MidWaveStrategy : IEnemyWaveStrategy
{
    public EnemySpawnRequest CreateSpawnRequest(GameObject verticalEnemyPrefab, GameObject horizontalEnemyPrefab, GameObject zigzagEnemyPrefab)
    {
        if (Random.Range(0, 2) == 0)
        {
            return new EnemySpawnRequest
            {
                side = EnemySpawner.SpawnSide.Top,
                enemyPrefab = verticalEnemyPrefab
            };
        }

        return new EnemySpawnRequest
        {
            side = Random.Range(0, 2) == 0 ? EnemySpawner.SpawnSide.Left : EnemySpawner.SpawnSide.Right,
            enemyPrefab = horizontalEnemyPrefab
        };
    }
}

public sealed class LateWaveStrategy : IEnemyWaveStrategy
{
    public EnemySpawnRequest CreateSpawnRequest(GameObject verticalEnemyPrefab, GameObject horizontalEnemyPrefab, GameObject zigzagEnemyPrefab)
    {
        int enemyType = Random.Range(0, 3);

        switch (enemyType)
        {
            case 0:
                return new EnemySpawnRequest
                {
                    side = EnemySpawner.SpawnSide.Top,
                    enemyPrefab = verticalEnemyPrefab
                };
            case 1:
                return new EnemySpawnRequest
                {
                    side = Random.Range(0, 2) == 0 ? EnemySpawner.SpawnSide.Left : EnemySpawner.SpawnSide.Right,
                    enemyPrefab = horizontalEnemyPrefab
                };
            default:
                return new EnemySpawnRequest
                {
                    side = (EnemySpawner.SpawnSide)Random.Range(0, 3),
                    enemyPrefab = zigzagEnemyPrefab
                };
        }
    }
}
