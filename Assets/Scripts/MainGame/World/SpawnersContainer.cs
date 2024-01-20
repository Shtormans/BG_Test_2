using MainGame;
using System.Collections.Generic;
using UnityEngine;

public class SpawnersContainer : MonoBehaviour
{
    [SerializeField] private List<EnemySpawner> _enemySpawners;
    [SerializeField] private List<BonusSpawner> _bonusSpawners;
    private int _spawnersInUse;
    private bool[] _isSpawnerInUse;

    public void SpawnEnemy(EnemyController enemy)
    {
        int index = GetSpawnerIndex(_enemySpawners.Count);
        enemy.transform.position = _enemySpawners[index].transform.position;
    }

    public void SpawnBonus(BonusController bonus)
    {
        int index = GetSpawnerIndex(_bonusSpawners.Count);
        bonus.transform.position = _bonusSpawners[index].transform.position;
    }

    public void ClearAfterSpawningWave()
    {
        _isSpawnerInUse = null;
        _spawnersInUse = 0;
    }

    private int GetSpawnerIndex(int length)
    {
        if (_spawnersInUse >= length - 1)
        {
            ClearAfterSpawningWave();
        }

        if (_isSpawnerInUse == null)
        {
            _isSpawnerInUse = new bool[length];
        }

        int index = Random.Range(0, length);

        while (_isSpawnerInUse[index])
        {
            index = (index + 1) % length;
        }

        _spawnersInUse++;
        _isSpawnerInUse[index] = true;
        return index;
    }
}
