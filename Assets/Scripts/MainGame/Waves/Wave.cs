using System.Collections.Generic;
using System;
using UnityEngine;

namespace MainGame
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Wave", order = 1)]
    public class Wave : ScriptableObject
    {
        public float WaveTime;

        public List<EnemyPair> Enemies;
        public List<BonusPair> Bonuses;
    }

    [Serializable]
    public struct EnemyPair
    {
        public EnemyController Enemy;
        public uint Amount;
    }

    [Serializable]
    public struct BonusPair
    {
        public BonusController Bonus;
        public uint Amount;
    }
}
