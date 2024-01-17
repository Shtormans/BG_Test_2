using System;
using UnityEngine;

namespace MainGame
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EntityStats", order = 1)]
    public class EntityStats : ScriptableObject
    {
        [field:SerializeField]
        public EntityData EntityData { get; private set; }
    }

    [Serializable]
    public struct EntityData
    {
        public int Health;
        public float Speed;
    }
}
