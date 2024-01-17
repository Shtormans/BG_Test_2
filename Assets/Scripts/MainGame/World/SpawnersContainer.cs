using MainGame;
using System.Collections.Generic;
using UnityEngine;

public class SpawnersContainer : MonoBehaviour
{
    [SerializeField] private List<EnemySpawner> _enemySpawners;
    [SerializeField] private List<BonusSpawner> _bonusSpawners;

    public void SpawnEnemy(EnemyController enemy)
    {
        int index = Random.Range(0, _enemySpawners.Count - 1);

        enemy.transform.position = _enemySpawners[index].transform.position;
    }

    public void SpawnBonus(BonusController bonus)
    {
        int index = Random.Range(0, _bonusSpawners.Count - 1);

        bonus.transform.position = _bonusSpawners[index].transform.position;
    }
}
