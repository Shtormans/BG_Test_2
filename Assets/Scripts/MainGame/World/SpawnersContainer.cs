using MainGame;
using System.Collections.Generic;
using UnityEngine;

public class SpawnersContainer : MonoBehaviour
{
    [SerializeField] private List<EnemySpawner> _enemySpawners;
    [SerializeField] private List<BonusSpawner> _bonusSpawners;

    private Dictionary<int, BonusController> _occupiedBonusSpawners = new();

    public int AmountOfEnemySpawners => _enemySpawners.Count;
    public int AmountOfBonusSpawners => _bonusSpawners.Count;
    public int AmountOfOccupiedBonusSpawners => _occupiedBonusSpawners.Count;

    public void ArrangeEnemy(EnemyController enemy)
    {
        int index = GetEnemySpawnerIndex();
        enemy.transform.position = _enemySpawners[index].transform.position;
    }

    public void ArrangeBonus(BonusController bonus)
    {
        int index = GetBonusSpawnerIndex();
        bonus.transform.position = _bonusSpawners[index].transform.position;

        _occupiedBonusSpawners[index] = bonus;
    }

    public void ClearBonusSpawner(BonusController bonus)
    {
        foreach (var pair in _occupiedBonusSpawners)
        {
            if (pair.Value == bonus)
            {
                _occupiedBonusSpawners.Remove(pair.Key);
                return;
            }
        }
    }

    private int GetEnemySpawnerIndex()
    {
        return Random.Range(0, _bonusSpawners.Count);
    }

    private int GetBonusSpawnerIndex()
    {
        if (_bonusSpawners.Count == _occupiedBonusSpawners.Count)
        {
            return 0;
        }

        int index = Random.Range(0, _bonusSpawners.Count);
        while (_occupiedBonusSpawners.TryGetValue(index, out var _))
        {
            index = (index + 1) % _bonusSpawners.Count;
        }

        return index;
    }
}
