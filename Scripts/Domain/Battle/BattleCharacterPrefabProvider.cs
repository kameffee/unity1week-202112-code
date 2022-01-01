using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Unity1week202112.Domain
{
    public sealed class BattleCharacterPrefabProvider : IDisposable
    {
        private readonly string CharacterFolder = "Characters";

        private readonly Dictionary<int, Object> _cache = new Dictionary<int, Object>();

        public T Load<T>(int characterId) where T : Object
        {
            if (_cache.TryGetValue(characterId, out var asset))
            {
                return asset as T;
            }

            _cache[characterId] = Resources.Load<T>($"{CharacterFolder}/Character_{characterId:000}");
            return (T)_cache[characterId];
        }

        public void Dispose()
        {
            _cache.Clear();
        }
    }
}
