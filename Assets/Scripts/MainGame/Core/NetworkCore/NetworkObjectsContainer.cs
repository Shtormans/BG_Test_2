using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class NetworkObjectsContainer : MonoBehaviour
    {
        private Dictionary<Type, List<NetworkBehaviour>> _objectsDictionary;

        private void Awake()
        {
            _objectsDictionary = new Dictionary<Type, List<NetworkBehaviour>>()
            {
                { typeof(PlayerBehaviour), new() },
                { typeof(Weapon), new() },
                { typeof(AnimatedSkin), new() }
            };
        }

        public void AddObject(NetworkBehaviour value)
        {
            var valueType = value.GetType();

            foreach (var type in _objectsDictionary.Keys)
            {
                if (valueType == type || valueType.IsSubclassOf(type))
                {
                    _objectsDictionary[type].Add(value);
                    return;
                }
            }

            _objectsDictionary.Add(valueType, new List<NetworkBehaviour>() { value });
        }

        public bool TryGetObjectById<T>(NetworkBehaviourId id, out T value)
        {
            List<NetworkBehaviour> values;
            value = default;

            if (!_objectsDictionary.TryGetValue(typeof(T), out values))
            {
                return false;
            }

            foreach (var item in values)
            {
                if (item.Id == id)
                {
                    value = (T)(object)item;

                    return true;
                }
            }

            return false;
        }
    }
}
